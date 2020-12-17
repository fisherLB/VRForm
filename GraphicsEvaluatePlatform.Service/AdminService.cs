//#define UNITTEST

/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2017. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Infrastructure.Encrypt;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Service
 * 项目描述: 
 * 类 名 称: AdminService
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: 覃明健
 * 命名空间: GraphicsEvaluatePlatform.Service
 * 文件名称: AdminService
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/5/3 16:56:41
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Service
{
    public static class AdminService
    {
        /// <summary>
        /// 用户登录系统
        /// </summary>
        /// <param name="model">登录视图模型</param>
        /// <returns>操作结果对象</returns>
        public static OperationResult UserLogin(LoginModel model)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(model, "model");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("AdminService").Error("UserLogin 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }

            //声明返回值
            var ret = new OperationResult(OperationResultType.Success);

            try
            {
                using (var tran = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    //声明查询条件
                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                    //声明更新目标
                    Dictionary<string, object> dicUpdateParam = new Dictionary<string, object>();

                    //根据传入的帐号判断系统是否存在该帐号
                    dicCondition.Add("Us_account", model.LoginName);
                    var retData = DataTrim.DataTableTrim(DataBll.GetModel("", "t_Users", dicCondition).Tables[0]);
                    if (retData.Rows.Count != 1)
                    {
                        return new OperationResult(OperationResultType.QueryNull, "无此帐号记录");
                    }

                    //将该帐号存入临时变量等待使用
                    var dr = retData.Rows[0];

                    //判断帐号是否被冻结
                    if (DateTime.Parse(dr["BanExpire"].ToString()) > DateTime.Now)
                    {
                        //UserLogService.InsertLog(dr["Us_account"].ToString(), dr["Us_name"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统登录", "尝试登录已被冻结的帐号:冻结结束时间: " + dr["BanExpire"].ToString(), "");
                        //OperationLogService.InsertLog(dr["Us_id"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "尝试登录已被冻结的帐号", "冻结结束时间: " + dr["BanExpire"].ToString());
                        tran.Complete();
                        return new OperationResult(OperationResultType.Error, "已连续登录失败达5次,帐号被停用24小时.\n帐号将于:" + dr["BanExpire"].ToString() + "后恢复登录.\n也可以联系管理员恢复登录");
                    }

                    //判断帐号是否被禁用
                    if (dr["Us_status"].ToString().ToLower() == "false")
                    {
                        //UserLogService.InsertLog(dr["Us_account"].ToString().Trim(), dr["Us_name"].ToString().Trim(), ServiceBase.GetIPAddress(), DateTime.Now, "系统登录", "尝试登录已被禁用的帐号", "");
                        //OperationLogService.InsertLog(dr["Us_id"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "尝试登录已被禁用的帐号", "");
                        //tran.Complete();
                        return new OperationResult(OperationResultType.Error, "该帐号已被系统管理员禁用");
                    }

                    //为临时变量设置密码
                    var pwd = EncryptPasswordFactory.GetEncipher(EncryptType.MD5).EncryptFor(model.Password);
                    //为更新登录情况信息设置查询条件
                    dicCondition.Add("Us_Password", dr["Us_Password"]);
                    //检查密码是否正确
                    if (dr["Us_Password"].ToString() != pwd)
                    {
                        //提取连续登录失败次数
                        var count = int.Parse(dr["LoginFaildCount"].ToString());
                        //之前已经失败了4次,再次失败则冻结24小时,否则失败次数+1, 并且将失败次数保存
                        if (count >= 4)
                        {
                            dicUpdateParam.Add("LoginFaildCount", 0);
                            dicUpdateParam.Add("Us_status", 0);
                            dicUpdateParam.Add("BanExpire", DateTime.Now.AddDays(1));
                            DataBll.Update(dicUpdateParam, "t_Users", dicCondition, "Us_id");
                            UserLogService.InsertLog(dr["Us_account"].ToString(), dr["Us_name"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统登录", "连续登录失败多次冻结:冻结结束时间: " + dicUpdateParam["BanExpire"].ToString(), "");
                            OperationLogService.InsertLog(dr["Us_id"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "连续登录失败多次冻结", "冻结结束时间: " + dicUpdateParam["BanExpire"].ToString());
                            tran.Complete();
                            return new OperationResult(OperationResultType.Error, "密码错误,已连续登录失败达5次,帐号被停用24小时.\n帐号将于:" + dicUpdateParam["BanExpire"].ToString().Trim() + "后恢复登录.\n也可以联系管理员恢复登录");
                        }
                        else
                        {
                            dicUpdateParam.Add("LoginFaildCount", count + 1);
                            DataBll.Update(dicUpdateParam, "t_Users", dicCondition, "Us_id");
                            UserLogService.InsertLog(dr["Us_account"].ToString(), dr["Us_name"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统登录", "登录系统失败:失败次数累计: " + dicUpdateParam["LoginFaildCount"].ToString().Trim() + "次.", "");
                            OperationLogService.InsertLog(dr["Us_id"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "登录系统失败", "失败次数累计:" + dicUpdateParam["LoginFaildCount"].ToString() + "次.");
                            tran.Complete();
                            return new OperationResult(OperationResultType.Error, "密码错误,帐号会在连续登录失败5次后被停用24小时.\n当前已失败" + dicUpdateParam["LoginFaildCount"].ToString() + "次.");
                        }

                    }

                    //帐号密码正确并且没有处于被冻结状态则刷新角色是否被禁止或冻结,并且将连续失败次数归零
                    using (var tran2 = new TransactionScope())
                    {
                        dicUpdateParam.Add("Us_status", "True");
                        dicUpdateParam.Add("LoginFaildCount", 0);
                        DataBll.Update(dicUpdateParam, "t_Users", dicCondition, "Us_id");
                        tran2.Complete();
                    }

                    //写入session

                    var userstr = JsonUtil.ToJson(DataTrim.DataTableTrim(dr.Table));
                    userstr = userstr.TrimStart('[').TrimEnd(']');
                    var UserModel = JsonUtil.ConvertToObject<Users>(userstr);

                    ServiceBase.SetInfo(ServiceBase.USERINFO, UserModel);
                    ServiceBase.SetInfo(ServiceBase.USERID, dr["Us_id"]);
                    ServiceBase.SetInfo(ServiceBase.USERNAME, dr["Us_account"]);
                    ServiceBase.SetInfo(ServiceBase.USERPASSWORD, dr["Us_Password"]);
                    ServiceBase.SetInfo(ServiceBase.USERFULLNAME, dr["Us_name"]);
                    ServiceBase.SetInfo(ServiceBase.USERTYPE, dr["Us_type"]);
                 
                    //获取单位信息
                    dicCondition.Clear();
                    dicCondition.Add("u_Id", dr["UnitID"]);
                    var tbUnit = DataTrim.DataTableTrim(DataBll.GetModel("", "t_Units", dicCondition).Tables[0]);
                    if (tbUnit.Rows.Count == 0)
                        return new OperationResult(OperationResultType.Error, "无法获取该帐号所属单位的信息,禁止登录");

                    var drUnit = tbUnit.Rows[0];

                    if (drUnit["u_IsEnable"].ToString().Trim() != "True")
                        return new OperationResult(OperationResultType.Error, "您所属的机构已被管理员冻结");

                    var unitstr = JsonUtil.ToJson(drUnit, "yyyy-MM-dd HH:mm:ss");

                    var UnitModel = JsonUtil.ConvertToObject<Units>(unitstr);
                 
                    ServiceBase.SetInfo(ServiceBase.UNITINFO, UnitModel);
                    ServiceBase.SetInfo(ServiceBase.UNITID, drUnit["u_Id"]);
                    ServiceBase.SetInfo(ServiceBase.UNITFULLNAME, drUnit["u_Name"]);
                    ServiceBase.SetInfo(ServiceBase.UNITNAME, dr["Unit_name"]);
                    ServiceBase.SetInfo(ServiceBase.UNITFULLPATH, drUnit["u_FullPath"]);

                    ////检查登录人权限是否跟给他设置权限的权限管理员的权限是否冲突
                    //PermissionManageService.CheckCurrentUserIsConflict();

                    ////获取用户的功能权限
                    DataSet PermissionList = PermissionManageService.GetCurrentUserPermission();
                    ServiceBase.SetInfo(ServiceBase.USER_PERMISSION, PermissionList);

                    //获取各级菜单
                    List<MenuViewModel> MenuList = MenuManageService.GetAllMenuList();
                    ServiceBase.SetInfo(ServiceBase.MENULIST, MenuList);

                    //写入日志
                    UserLogService.InsertLog(dr["Us_account"].ToString(), dr["Us_name"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统登录", "登录成功", "");
                    OperationLogService.InsertLog(dr["Us_id"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "登录成功", "");

                    ret.Message = "登录成功";
                    tran.Complete();
                }

                System.Threading.Thread.Sleep(new Random().Next(500, 3000));
            }
            catch (Exception ex)
            {
                Logger.GetLogger("AdminService").Error("UserLogin 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error);
            }

            return ret;
        }

        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="OriPassword">原始密码</param>
        /// <param name="NewPssword">新密码</param>
        /// <returns>操作结果对象</returns>
        public static OperationResult ChangePassword(string UserID, string OriPassword, string NewPssword)
        {
            //检查参数
            try
            {
                PublicHelper.CheckArgument(UserID, "UserID");
                PublicHelper.CheckArgument(OriPassword, "OriPassword", false, false);
                PublicHelper.CheckArgument(NewPssword, "NewPssword", false, false);
            }
            catch (Exception ex)
            {
                Logger.GetLogger("AdminService").Error("ChangePassword 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            //声明返回对象
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    //检查ID是否存在
                    if (UserID == "")
                    {
                        ret = new OperationResult(OperationResultType.ParamError);
                        ret.Message = "用户ID错误";
                        return ret;
                    }
                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                    dicCondition.Add("Us_id", UserID);
                    var retdata =DataTrim.DataTableTrim(DataBll.GetModel("", "t_Users", dicCondition).Tables[0]);
                    if (retdata.Rows.Count == 0)
                    {
                        ret = new OperationResult(OperationResultType.ParamError);
                        ret.Message = "用户ID错误";
                        return ret;
                    }
                    //将用户设置为临时变量方便使用
                    var User = retdata.Rows[0];
                    //检查原始密码是否正确
                    if (User["Us_Password"].ToString().Trim() != EncryptPasswordFactory.GetEncipher(EncryptType.MD5).EncryptFor(OriPassword))
                    {
#if UNITTEST
                        UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "首页-用户修改密码", "修改密码失败,原始密码错误", "");
                        OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "修改密码失败", "原始密码错误");
#else
                        UserLogService.InsertLog(User["Us_account"].ToString(), User["Us_name"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "首页-用户修改密码", "修改密码失败,原始密码错误", "");
                        OperationLogService.InsertLog(User["Us_id"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "修改密码失败", "原始密码错误");
#endif

                        ret = new OperationResult(OperationResultType.Error);
                        ret.Message = "原始密码错误";
                        return ret;
                    }

                    //检查新密码是否合理
                    if (NewPssword.Trim().Length == 0)
                    {
#if UNITTEST
                        UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "首页-用户修改密码", "修改密码失败,新密码不合规则", "");
                        OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "修改密码失败", "新密码不合规则");
#else
                        UserLogService.InsertLog(User["Us_account"].ToString(), User["Us_name"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "首页-用户修改密码", "修改密码失败,新密码不合规则", "");
                        OperationLogService.InsertLog(User["Us_id"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "修改密码失败", "新密码不合规则");
#endif

                        ret = new OperationResult(OperationResultType.Error);
                        ret.Message = "新密码不合规则";
                        return ret;
                    }

                    //设置新密码并且保存
                    User["Us_Password"] = EncryptPasswordFactory.GetEncipher(EncryptType.MD5).EncryptFor(NewPssword);

                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("Us_Password", User["Us_Password"]);
                    DataBll.Update(values, "t_Users", dicCondition, "Us_id");

#if UNITTEST
                    UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "首页-用户修改密码", "修改密码成功", "");
                    OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "修改密码成功", "");
#else
                    Users us = (Users)ServiceBase.GetInfo(ServiceBase.USERINFO);
                    us.Us_Password = User["Us_Password"].ToString().Trim();
                    ServiceBase.SetInfo(ServiceBase.USERINFO, us);

                    UserLogService.InsertLog(User["Us_account"].ToString(), User["Us_name"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "首页-用户修改密码", "修改密码成功", "");
                    OperationLogService.InsertLog(User["Us_id"].ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "修改密码成功", "");
#endif

                    ret = new OperationResult(OperationResultType.Success);
                    ret.Message = "密码修改成功";

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("AdminService").Error("ChangePassword 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "修改密码时发生异常", ex.Message);
            }
            return ret;
        }
    }
}
