/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Client.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEvaluatePlatform.Client.Models;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Repository;
using System.Transactions;
using System.Data;
using GraphicsEvaluatePlatform.Client.Basics;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Client.Common;
using GraphicsEvaluatePlatform.Client.ViewModels;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Client.Services
 * 项目描述: 
 * 类 名 称: UserService
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Client.Services
 * 文件名称: UserService
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/19 9:37:10
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Client.Services
{
    class UserService : IUserService
    {
        /// <summary>
        /// 将DataTable转为List集合
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<UserItemViewModel> GetAllUserList(DataTable dt)
        {
            List<UserItemViewModel> userList = new List<UserItemViewModel>();

            foreach (DataRow d in dt.Rows)
            {
                UserItemViewModel user = new UserItemViewModel();
                UserModel userModel = new UserModel();
                userModel.Us_id = d["Us_id"].ToString();
                userModel.Us_name = d["Us_name"].ToString();
                userModel.Us_account = d["Us_account"].ToString();
                userModel.Us_Password = d["Us_Password"].ToString();
                userModel.Us_type = d["Us_type"].ToString();
                userModel.Us_create_time = d["Us_create_time"].ToString();
                if (d["Us_status"].ToString() == "True")
                {
                    userModel.Us_status = "激活";
                }
                else
                {
                    userModel.Us_status = "冻结";
                }
                userModel.Us_remark = d["Us_remark"].ToString();
                user.userModel = userModel;
                user.IsSelected = false;
                userList.Add(user);
            }
            return userList;
        }
        /// <summary>
        /// 获取用户数据列表
        /// </summary>
        /// <param name="pageNum">当前页</param>
        /// <param name="currentlyPage">页码</param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public OperationResult GetList(int pageNum, int currentlyPage, Dictionary<string, object> dic)
        {
            //检查登录参数
            try
            {
                PublicHelper.CheckArgument(pageNum, "pageNum");
                PublicHelper.CheckArgument(currentlyPage, "currentlyPage");
                PublicHelper.CheckArgument(dic, "dic");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("GetList 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);

            try
            {
                using (var tran = new TransactionScope())
                {
                    string where = "";
                    //if (dic.ContainsKey("UnitID"))
                    //{
                    //    where = "UnitID='" + dic["UnitID"].ToString() + "' ";
                    //}
                    var ds = DataBll.GetDataSetList("t_Users", pageNum, currentlyPage, "Us_id,Unit_name,Us_account,Us_name,Us_Password,Us_status,Us_type,Us_create_time,Us_create_name,Us_remark", where, " Us_create_time asc", "Us_account");
                    var total = DataBll.GetCount("t_Users", where);
                    ret = new OperationResult(OperationResultType.Success);
                    ret.AppendData = new PageData
                    {
                        total = total,
                        current = ds.Tables[0].Rows.Count,
                        rows = ds
                    };
                    ret.Message = "查询列表成功";

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
        /// <param name="model"></param>
        /// <returns></returns>
        public OperationResult AddUser(UserModel model)
        {
            try
            {
                PublicHelper.CheckArgument(model, "model");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("AddUser 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);

            try
            {
                var UnitID = BaseService.UNITID;
                //判断是否重复
                int count = DataBll.GetCount("t_Users", "UnitID='" + UnitID + "' and Us_account='" + model.Us_account + "'");
                if (count > 0)
                {
                    ret.Message = "该用户账号已存在";
                    ret.ResultType = OperationResultType.Error;
                    return ret;
                }
                using (var tran = new TransactionScope())
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    Dictionary<string, object> dicwhere = new Dictionary<string, object>();
                    dic = ModelToDictionary.EntityToDictionary<UserModel>(model);//将model转为键值对
                    dic["Us_id"]= Guid.NewGuid();
                    dic[ "Us_create_time"]= DateTime.Now;
                    dic["Us_create_name"]= BaseService.USERNAME;
                    dic["UnitID"] = UnitID;
                    DataSet dsret = DataBll.Add(dic, "t_Users");
                    if (dsret != null)
                    {
                        ret.Message = "新增用户成功！";
                        ret.AppendData = new { Success = true, Message = ret.Message };
                    }
                    else
                    {
                        ret.Message = "新增用户失败！";
                        ret.AppendData = new { Success = false, Message = ret.Message };
                    }

                    ret.Message = "新增用户成功！";
                    ret.AppendData = new { Success = true, Message = ret.Message };
                    tran.Complete();
                }

            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("AddUser 发生异常", ex);
                if (ex.Message.IndexOf("不能在具有唯一索引") != -1)
                    return new OperationResult(OperationResultType.ParamError, "用户账号已存在", new { Success = false, Message = "用户账号已存在" });
                else
                    return new OperationResult(OperationResultType.ParamError, "新增失败", new { Success = false, Message = "新增用户失败" });
            }
            return ret;
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public OperationResult DeleteUser(string ids)
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
                        //删除用户自定义字段顺序记录
                        //  DataBll.Delete(dicCondition, "UserDataSeqset");
                        //删除用户记录
                        DataBll.Delete(dicCondition, "t_Users");

#if UNITTEST
                       // OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "删除ID为: " + item + "的用户成功", "");
                        UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户管理-删除用户", "删除ID为: " + item + "的用户成功", "");
#else
                        //OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim(), ServiceBase.GetIPAddress(), DateTime.Now, "删除ID为: " + item + "的用户成功", "");
                        // UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString().Trim(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString().Trim(), ServiceBase.GetIPAddress().Trim(), DateTime.Now, "系统设置-用户管理-删除用户", "删除ID为: " + item + "的用户成功", "");
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
        /// 编辑用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OperationResult EditUser(UserModel model)
        {
            try
            {
                PublicHelper.CheckArgument(model, "model");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserService").Error("UpdateUser 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }

            OperationResult ret = new OperationResult(OperationResultType.Success);
            var UnitID = BaseService.UNITID;
            //判断是否重复
            int count = DataBll.GetCount("t_Users", "UnitID='" + UnitID + "' and Us_account='" + model.Us_account + "'");
            if (count > 0)
            {
                ret.Message = "该用户账号已存在";
                ret.ResultType = OperationResultType.Error;
                return ret;
            }
            try
            {
                using (var tran = new TransactionScope())
                {
                    if (string.IsNullOrEmpty(model.Us_create_time))
                    {
                        model.Us_create_time = DateTime.Now.ToString();
                    }
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    Dictionary<string, object> dicwhere = new Dictionary<string, object>();
                    dic = ModelToDictionary.EntityToDictionary<UserModel>(model);//将model转为键值对
                    dicwhere.Add("Us_id", model.Us_id);

                    dic.Remove("Us_id");
                    bool brest = DataBll.Update(dic, "t_Users", dicwhere, model.Us_id);

                    if (brest)
                    {
                        ret.Message = "修改用户成功！";
                        ret.AppendData = new { Success = true, Message = ret.Message };
                    }
                    else
                    {
                        ret.Message = "修改用户失败！";
                        ret.AppendData = new { Success = false, Message = ret.Message };
                    }
                    ret.Message = "修改用户成功！";
                    ret.AppendData = new { Success = true, Message = ret.Message };
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

    }
}
