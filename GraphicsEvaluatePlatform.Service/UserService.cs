//#define UNITTEST

/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
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
using System.Transactions;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Service
 * 项目描述: 图像测评系统
 * 类 名 称: UserService
 * 说    明: 用户管理
 * 版 本 号: v1.0.0
 * 作    者: 刘桂林
 * 命名空间: GraphicsEvaluatePlatform.Service
 * 文件名称: UserService
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/4/25 10:00:40
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Service
{
    public static class UserService
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pager">分页查询参数</param>
        /// <returns>操作结果对象</returns>
        public static OperationResult GetUserList(BootstrapPager pager)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(pager, "pager");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("GetUserList 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }

            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    var unitid = "";
                    var filters = pager.filter.Split(',');
                    unitid = filters[1].Split(':')[1];
                    pager.filter = filters[0];

#if UNITTEST
                    var user_Type = 1;
                    string unit_FullPath = "泰坦软件研发部";
#else
                    var user_Type = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString().Trim();
                    string unit_FullPath = ServiceBase.GetInfo(ServiceBase.UNITFULLPATH).ToString().Trim();
#endif

                    string unitWhere = "";
                    //搜索关键字
                    if (pager.filter != "")
                    {
                        unitWhere = "Us_account like '%" + pager.filter + "%' or Us_name like '%" + pager.filter + "%'";
                    }

                    //用户权限
                    if (unitid == "-1")//选取全部
                    {
                        if (user_Type == "1")//超级管理员（获取所有单位数据）
                        {
                            unitWhere += "";
                        }
                        else if (user_Type == "2")//一般管理员（获取本级和下级机构数据）
                        {
                            var UnitName = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString().Trim();
                            var unitids = DataBll.Query("select u_Id from t_Units where u_FullPath = '" + UnitName + "' or u_FullPath like '%" + UnitName + "-%' or u_FullPath like '%-" + UnitName + "'");
                            var idstr = "";
                            foreach (DataRow iditem in unitids.Tables[0].Rows)
                                idstr += "'" + iditem[0].ToString() + "',";
                            idstr = idstr.Substring(0, idstr.Length - 1);
                            unitWhere += ((unitWhere == "" ? "" : " and ") + " UnitID in (" + idstr + ")");//本级和下级的机构id
                        }
                        else
                        {//一般操作员（获取本单位数据）
                            var UnitID = ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim();
                            unitWhere += ((unitWhere == "" ? "" : " and ") + " UnitID = '" + UnitID + "'");
                        }
                    }
                    else
                    {//选择某一单位
                        unitWhere += ((unitWhere == "" ? "" : " and ") + " UnitID = '" + unitid + "'");
                    }

                    var datas = DataBll.GetDataSetList("t_Users", pager.PageSize, pager.PageIndex, "", unitWhere, "Us_create_time desc", "Us_id");//分页
                    //string sql = "select * from t_Users where" + unitWhere;//不分页
                    //DataSet datas = DataBll.Query(sql);
                    var count = DataBll.GetCount("t_Users", unitWhere);

                    ret.Message = "查询成功";
                    ret.AppendData = new { total = count, rows = DataTrim.DataTableTrim(datas.Tables[0]) };



                    //if (user_Type.ToString().Trim() != "1")
                    //{
                    //    //不是超级管理员，获取本机构和下级机构的机构Id
                    //    DataTable uds = DataTrim.DataTableTrim(DataBll.Query("select u_Id from t_Units where u_FullPath='" + unit_FullPath + "'").Tables[0]);
                    //    if (uds.Rows.Count > 0)
                    //    {
                    //        unitWhere = "and UnitID in ('";
                    //        foreach (DataRow dr in uds.Rows)
                    //        {
                    //            unitWhere += dr["u_Id"].ToString().Trim() + ",";
                    //        }
                    //        unitWhere = unitWhere.TrimEnd(',');
                    //        unitWhere += "')";
                    //    }

                    //}
                    ////超级管理员查询所有人员，不是超级管理员只查询本机构和下级机构的人员

                    ////string sq="select"
                    ////var datas = DataBll.GetDataSetList("Users", pager.rows, pager.page, "", "(Us_name like '%" + pager.filter + "%' or Us_account like '%" + pager.filter + "%') " + (unitid == "-1" ? "" + unitWhere : ("and UnitID = '" + unitid + "'")), "Us_create_time desc", "Us_id");

                    //var startIndex = 1;
                    //var endIndex = 1;
                    //if (pager.PageIndex < 1)
                    //{
                    //    pager.PageSize = 10;
                    //}
                    //if (pager.PageIndex < 1)
                    //{
                    //    pager.PageIndex = 1;
                    //}
                    //startIndex = (pager.PageIndex - 1) * pager.PageSize;
                    //endIndex = pager.PageIndex * pager.PageSize;
                    //string sqlWhere;
                    //if (pager.filter != "")
                    //     sqlWhere = " where  (Us_name like '%" + pager.filter + "%'  or " + " Us_account like '%" + pager.filter + "%') " + (unitid == "-1" ? "" + unitWhere : ("where UnitID = '" + unitid + "'"));
                    //else
                    //    sqlWhere = unitid == "-1" ? "" + unitWhere : ("where UnitID = '" + unitid + "'");

                    //var sql = "select * from ( select row_number() over (order by Us_Create_time desc) rownumber , us.* from t_users us left join t_Units ut on us.UnitID = ut.u_Id  " + sqlWhere + " ) ret where ret.rownumber > " + startIndex + " and ret.rownumber<=" + endIndex + " order by ret.rownumber asc";

                    //var retft = DataBll.Query(sql).Tables[0];
                    ////去掉空格
                    //retft = DataTrim.DataTableTrim(retft);

                    //var count = DataBll.GetCount("t_Users", "(Us_name like '%" + pager.filter + "%') " + (unitid == "-1" ? "" + unitWhere : ("and UnitID = '" + unitid + "'")));

                    //ret.Message = "查询成功";
                    //ret.AppendData = new
                    //{
                    //    total = count,
                    //    rows = retft
                    //};

#if UNITTEST
                   // OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "查询用户列表成功", "");
                    UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-查询用户列表", "查询成功", "");
#else
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim(), ServiceBase.GetIPAddress().ToString().Trim(), DateTime.Now, "查询用户列表成功", "");
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString().Trim(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString().Trim(), ServiceBase.GetIPAddress().Trim(), DateTime.Now, "系统设置-用户管理-查询用户列表", "查询成功", "");
#endif
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("GetUserList 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "GetUserList 发生异常", ex.Message);
            }
            return ret;
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static OperationResult AddUser(Dictionary<string, object> user)
        {
            try
            {
                PublicHelper.CheckArgument(user, "user");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("AddUser 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }

            if (user["Us_remark"] != null && user["Us_remark"].ToString().Length > 255)
                return new OperationResult(OperationResultType.ParamError, "备注的长度不能超过255个字符", new { Success = false, Message = "备注的长度不能超过255个字符" });

            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    //判断用户名是否重复(同一机构下)
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Us_account",user["Us_account"]);
                    dic.Add("UnitID", user["UnitID"]);
                    DataTable ds = DataTrim.DataTableTrim(DataBll.GetModel("", "t_users", dic).Tables[0]); 
                    if (ds.Rows.Count >= 1) {
                        return new OperationResult(OperationResultType.Error, "用户名已存在", new { Success = false, Message = "用户名已存在" });
                    }

                    if (user["UnitID"].ToString() == "-1")
                    {
#if UNITTEST
                        user["UnitID"] = 1;//ServiceBase.GetInfo(ServiceBase.UNITID);
                        user["Unit_name"] = "泰坦软件研发部";//ServiceBase.GetInfo(ServiceBase.UNITFULLNAME);
#else
                        user["UnitID"] = ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim();
                        user["Unit_name"] = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString().Trim();
#endif
                    }

                    user["Us_Password"] = EncryptPasswordFactory.GetEncipher(EncryptType.MD5).EncryptFor(user["Us_Password"].ToString().Trim());
                    user.Add("Us_create_time", DateTime.Now);
#if UNITTEST
                    user.Add("Us_create_name", "圆西瓜");
#else
                    user.Add("Us_create_name", ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString().Trim());
#endif
                    user.Add("BanExpire", DateTime.Now.AddMinutes(-1));
                    var dsret = DataBll.Add(user, "t_Users");

#if UNITTEST
                   // OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "新增用户成功,内容: " + JsonUtil.ToJson(user) + "", "");
                    //UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-新增用户", "新增名为:" + user["Us_name"] + "的用户成功", "");
#else
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim(), ServiceBase.GetIPAddress(), DateTime.Now, "新增用户成功,内容: " + JsonUtil.ToJson(user) + "", "");
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString().Trim(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString().Trim(), ServiceBase.GetIPAddress().Trim(), DateTime.Now, "系统设置-用户管理-新增用户", "新增名为:" + user["Us_name"] + "的用户成功", "");
#endif
                    ret.Message = "新增用户成功,请到用户组管理为用户分配用户组以获取基础权限或直接到权限管理为用户单独分配权限";
                    ret.AppendData = new { Success = true, Message = ret.Message };
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("AddUser 发生异常", ex);
                if (ex.Message.IndexOf("不能在具有唯一索引") != -1)
                    return new OperationResult(OperationResultType.ParamError, "用户名已存在", new { Success = false, Message = "用户名已存在" });
                else
                    return new OperationResult(OperationResultType.ParamError, "新增用户失败", new { Success = false, Message = "新增用户失败" });
            }
            return ret;
        }
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static OperationResult UpdateUser(Dictionary<string, object> user)
        {
            try
            {
                PublicHelper.CheckArgument(user, "user");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("UpdateUser 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }

            OperationResult ret = new OperationResult(OperationResultType.Success);
            var dicCondition = new Dictionary<string, object>();
            dicCondition.Add("Us_id", user["Us_id"]);
            var tempds = DataBll.GetModel("", "t_Users", dicCondition);
            if (tempds.Tables[0].Rows.Count == 0)
            {
                ret = new OperationResult(OperationResultType.ParamError);
                ret.Message = "无法找到指定用户";
                return ret;
            }

            //判断用户名是否重复(同一机构下)
            string sqlrepeat = "select * from t_Users where Us_account = '" + user["Us_account"] + "' and UnitID = '" + user["UnitID"] + "' and Us_id != '" + user["Us_id"] + "'";
            DataSet ds = DataBll.Query(sqlrepeat);
            if (ds.Tables[0].Rows.Count > 0) {
                return new OperationResult(OperationResultType.Error, "用户名已存在", new { Success = false, Message = "用户名已存在" });
            }
            try
            {
                using (var tran = new TransactionScope())
                {
                    var retobj = (DataSet)DataBll.UpdateReturnObj(user, "t_Users", dicCondition, "Us_id");
                   
#if UNITTEST
                    //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "更新ID为: " + user["Us_id"] + " 的用户成功", "");
                    UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-更新用户信息", "更新ID为: " + user["Us_id"] + " 的用户成功", "");
#else
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim(), ServiceBase.GetIPAddress().Trim(), DateTime.Now, "更新ID为: " + user["Us_id"].ToString().Trim() + " 的用户成功", "");
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString().Trim(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString().Trim(), ServiceBase.GetIPAddress().Trim(), DateTime.Now, "系统设置-用户管理-更新用户信息", "更新ID为: " + user["Us_id"] + " 的用户成功", "");

                    var currentusid = ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim();
                    if (currentusid.Trim() == user["Us_id"].ToString().Trim())
                    {
                        var jsonstr = JsonUtil.ToJson(DataTrim.DataTableTrim(retobj.Tables[0]));
                        
                        jsonstr = jsonstr.Substring(1);
                        jsonstr = jsonstr.Substring(0, jsonstr.Length - 1);
                        Users us = JsonUtil.ConvertToObject<Users>(jsonstr);
                        ServiceBase.SetInfo(ServiceBase.USERINFO, us);
                    }
#endif

                    ret = new OperationResult(OperationResultType.Success);
                    ret.Message = "用户信息修改成功";
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("UpdateUser 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "UpdateUser 发生异常", ex);
            }
            return ret;
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static OperationResult DeleteUser(string ids)
        {
            try
            {
                PublicHelper.CheckArgument(ids, "ids");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("DeleteUser 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }

            OperationResult ret = new OperationResult(OperationResultType.Success);

            if (ids == "")
            {
                ret = new OperationResult(OperationResultType.ParamError);
                ret.Message = "用户ID错误";
                return ret;
            }
            string[] arrids = ids.Split(',');
            if (arrids.Length == 0)
            {
                ret = new OperationResult(OperationResultType.ParamError);
                ret.Message = "用户ID错误";
                return ret;
            }
            try
            {
                using (var tran = new TransactionScope())
                {
                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                    foreach (var item in arrids)
                    {
                        dicCondition.Clear();
                        dicCondition.Add("Us_id", item);

                        //删除用户和用户组对应关系
                        DataBll.Delete(dicCondition, "t_UserAndUserGroup");
                        //删除用户功能权限
                        DataBll.Delete(dicCondition, "t_UserFunction");
                        //删除用户数据权限
                       // DataBll.Delete(dicCondition, "UserCategory");
                        //删除用户自定义字段顺序记录
                      //  DataBll.Delete(dicCondition, "UserDataSeqset");
                        //删除用户记录
                        DataBll.Delete(dicCondition, "t_Users");

#if UNITTEST
                       // OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "删除ID为: " + item + "的用户成功", "");
                        UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-删除用户", "删除ID为: " + item + "的用户成功", "");
#else
                        OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim(), ServiceBase.GetIPAddress(), DateTime.Now, "删除ID为: " + item + "的用户成功", "");
                        UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString().Trim(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString().Trim(), ServiceBase.GetIPAddress().Trim(), DateTime.Now, "系统设置-用户管理-删除用户", "删除ID为: " + item + "的用户成功", "");
#endif
                    }
                    tran.Complete();
                }
                ret = new OperationResult(OperationResultType.Success);
                ret.Message = "删除用户信息成功";
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("DeleteUser 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "DeleteUser 发生异常", ex);
            }
            return ret;
        }
        /// <summary>
        /// 初始化用户密码
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static OperationResult InitPassword(string ids)
        {
            try
            {
                PublicHelper.CheckArgument(ids, "ids");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("InitPassword 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }

            OperationResult ret = new OperationResult(OperationResultType.Success);

            if (ids == "")
            {
                ret = new OperationResult(OperationResultType.ParamError);
                ret.Message = "用户ID错误";
                return ret;
            }
            string[] arrids = ids.Split(',');
            if (arrids.Length == 0)
            {
                ret = new OperationResult(OperationResultType.ParamError);
                ret.Message = "用户ID错误";
                return ret;
            }
            try
            {
                using (var tran = new TransactionScope())
                {
                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                    Dictionary<string, object> dicvalue = new Dictionary<string, object>();
                    foreach (var item in arrids)
                    {
                        dicCondition.Clear();
                        dicCondition.Add("Us_id", item);

                        dicvalue.Clear();
                        dicvalue.Add("Us_password", EncryptPasswordFactory.GetEncipher(EncryptType.MD5).EncryptFor("123"));

                        DataBll.Update(dicvalue, "t_Users", dicCondition, "Us_id");

#if UNITTEST
                       // OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "初始化ID为: " + item + "的用户密码为: 123", "");
                        UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-初始化用户密码", "初始化ID为: " + item + "的用户的密码为: 123", "");
#else
                        OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "初始化ID为: " + item + "的用户密码为: 123", "");
                        UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户管理-初始化用户密码", "初始化ID为: " + item + "的用户的密码为: 123", "");
#endif
                    }
                    tran.Complete();
                }
                ret = new OperationResult(OperationResultType.Success);
                ret.Message = "初始化用户密码成功";
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("InitPassword 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "InitPassword 发生异常", ex);
            }
            return ret;
        }
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="NewPssword"></param>
        /// <returns></returns>
        public static OperationResult ChangePassword(string UserID, string NewPssword)
        {
            try
            {
                PublicHelper.CheckArgument(UserID, "UserID");
                PublicHelper.CheckArgument(NewPssword, "NewPssword", false, false);
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("ChangePassword 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }

            OperationResult ret = new OperationResult(OperationResultType.Success);
            if (UserID == "")
            {
                ret = new OperationResult(OperationResultType.ParamError);
                ret.Message = "用户ID错误";
                return ret;
            }
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            dicCondition.Add("Us_id", UserID);
            var User = DataBll.GetModel("", "t_Users", dicCondition);
            if (User.Tables[0].Rows.Count == 0)
                return new OperationResult(OperationResultType.IllegalOperation, "该用户不存在");

            Dictionary<string, object> dicValues = new Dictionary<string, object>();
            dicValues.Add("Us_Password", EncryptPasswordFactory.GetEncipher(EncryptType.MD5).EncryptFor(NewPssword));
            try
            {
                using (var tran = new TransactionScope())
                {
                    DataBll.Update(dicValues, "t_Users", dicCondition, "Us_id");
#if UNITTEST
                    //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "修改用户: " + User.Tables[0].Rows[0]["Us_account"] + " 的密码成功", "");
                    UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-修改用户密码", "修改用户: " + User.Tables[0].Rows[0]["Us_account"] + " 的密码成功", "");
#else
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim(), ServiceBase.GetIPAddress().Trim(), DateTime.Now, "修改用户: " + User.Tables[0].Rows[0]["Us_account"].ToString().Trim() + " 的密码成功", "");
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString().Trim(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString().Trim(), ServiceBase.GetIPAddress().ToString().Trim(), DateTime.Now, "系统设置-用户管理-修改用户密码", "修改用户: " + User.Tables[0].Rows[0]["Us_account"].ToString().Trim() + " 的密码成功", "");
#endif
                    ret = new OperationResult(OperationResultType.Success);
                    ret.Message = "密码修改成功";

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("ChangePassword 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error, "ChangePassword 发生异常", ex);
            }
            return ret;
        }
        /// <summary>
        /// 启用或禁用用户
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static OperationResult ChangeUserStatus(string type, string ids)
        {
            try
            {
                PublicHelper.CheckArgument(type, "type");
                PublicHelper.CheckArgument(ids, "ids");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("ChangeUserStatus 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);

            try
            {
                var strids = ids.Split(',');
                Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                Dictionary<string, object> dicValues = new Dictionary<string, object>();
                using (var tran = new TransactionScope())
                {
                    foreach (var item in strids)
                    {
                        dicCondition.Clear();
                        dicValues.Clear();
                        dicValues.Add("Us_status", type);
                        dicCondition.Add("Us_id", item);
                        if (type == "true")
                            dicValues.Add("BanExpire", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        DataBll.Update(dicValues, "t_Users", dicCondition, "Us_id");
#if UNITTEST
                       // OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, (type == 1 ? "启用" : "禁用") + "ID为:" + item + " 的用户成功", "");
                        UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-" + (type == "true" ? "启用" : "禁用") + "用户", "用户ID为: " + item, "");
#else
                        OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, (type == "true" ? "启用" : "禁用") + "ID为:" + item + " 的用户成功", "");
                        UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户管理-" + (type == "true" ? "启用" : "禁用") + "用户", "用户ID为: " + item, "");
#endif
                    }

                    tran.Complete();
                }

                ret = new OperationResult(OperationResultType.Success);
                var data = new { Success = true, Message = (type == "true" ? "启用" : "禁用") + "用户成功" };
                ret.AppendData = data;
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("ChangeUserStatus 发生异常", ex);
                return new OperationResult(OperationResultType.Error, "发生异常", new { Success = true, Message = (type == "true" ? "启用" : "禁用") + "用户失败" });
            }
            return ret;
        }



        public static DataSet GetUserModel(string userName)
        {
            Dictionary<string, object> dicw = new Dictionary<string, object>();
            dicw.Add("Us_account", userName);
            return DataBll.GetModel("", "t_Users", dicw);
        }
        public static DataSet GetUserModel(Guid userId)
        {
            Dictionary<string, object> dicw = new Dictionary<string, object>();
            dicw.Add("Us_id", userId);
            return DataBll.GetModel("", "t_Users", dicw);
        }
    }
}
