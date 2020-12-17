//#define UNITTEST
/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
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
 * 项目描述: 图像测评系统
 * 类 名 称: UserGroupService
 * 说    明: 用户组管理
 * 版 本 号: v1.0.0
 * 作    者: Administrator
 * 命名空间: GraphicsEvaluatePlatform.Service
 * 文件名称: UserGroupService
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/2 10:44:46
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Service
{
    public static class UserGroupService
    {

        public static OperationResult GetUserGroupCombobox()
        {
            try
            {
                using (var tran = new TransactionScope())
                {
#if UNITTEST
                    var unitID = "d079a6f3-5547-4313-a144-f952d524416d";//ServiceBase.GetInfo(ServiceBase.UNITID).ToString();
#else
                    var unitID = ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim();
#endif
                    Dictionary<string, object> dicCondiction = new Dictionary<string, object>();
                    dicCondiction.Add("UnitID", unitID);
                    dicCondiction.Add("Ug_type!", 0);
                    var dsug = DataBll.GetList("Ug_id, Ug_name", "t_UserGroup", dicCondiction);
                    var retdata = new List<object>();
                    if (dsug.Tables[0].Rows.Count == 0)
                    {
                        retdata.Add(new
                        {
                            id = "",
                            text = "无记录"
                        });
                    }
                    else
                    {
                        var rows = dsug.Tables[0].Rows;
                        foreach (DataRow item in rows)
                        {
                            retdata.Add(new
                            {
                                id = item["Ug_id"].ToString().Trim(),
                                text = item["ug_Name"].ToString().Trim()
                            });
                        }
                    }

                    tran.Complete();
                    return new OperationResult(OperationResultType.Success, "查询结果成功", retdata);
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupService").Error("GetUserGroupCombobox 发生异常", ex);
            }
            return new OperationResult(OperationResultType.Success, "查询结果成功", new List<object>() { new { id = "", text = "无记录" } });
        }

        /// <summary>
        /// 获取用户组列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public static OperationResult GetUserGroupList(BootstrapPager pager)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                PublicHelper.CheckArgument(pager, "pager");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupService").Error("GetUserGroupList 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }
            try
            {
                using (var tran = new TransactionScope())
                {
                    var unitid = "";
                    var filters = pager.filter.Split(',');
                    unitid = filters[1].Split(':')[1].Trim();//单位id
                    pager.filter = filters[0];//查询关键字
                    var ustype = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString().Trim();//用户类型

                    var wherestr = "";//查询条件               
                    if (pager.filter != "")
                    {
                        wherestr = "Ug_name like '%" + pager.filter + "%'";
                    }

                    if (unitid == "-1")//查询全部
                    {
                        if (ustype == "1")//如果是超级管理员
                        {
                            wherestr += "";

                        }
                        else if (ustype == "2")//如果是一般管理员
                        {
                            var UnitName = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString().Trim();
                            var unitids = DataBll.Query("select u_Id from t_Units where u_FullPath = '" + UnitName + "' or u_FullPath like '%" + UnitName + "-%' or u_FullPath like '%-" + UnitName + "'");
                            var idstr = "";
                            foreach (DataRow iditem in unitids.Tables[0].Rows)
                                idstr += "'" + iditem[0].ToString() + "',";
                            idstr = idstr.Substring(0, idstr.Length - 1);
                            wherestr += ((wherestr == "" ? "" : " and ") + " UnitID in (" + idstr + ")");
                        }
                        else//如果是一般操作员
                        {
                            var UnitID = ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim();
                            wherestr += ((wherestr == "" ? "" : " and ") + " UnitID = '" + UnitID + "'");
                        }
                    }
                    else {
                        wherestr += ((wherestr == "" ? "" : " and ") + " UnitID = '" + unitid + "'");
                    }

                    var datas = DataBll.GetDataSetList("t_UserGroup", pager.PageSize, pager.PageIndex, "", wherestr, "Ug_create_time desc", "Ug_id");
                    var count = DataBll.GetCount("t_UserGroup", wherestr);

                    ret.AppendData = new { total = count, rows = DataTrim.DataTableTrim(datas.Tables[0]) };

#if UNITTEST
                    OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "查询用户组列表成功", "");
                    UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户组管理-查询用户组列表", "查询成功", "");
#else
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "查询用户组列表成功", "");
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户组管理-查询用户组列表", "查询成功", "");
#endif

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupService").Error("GetUserGroupList 发生异常", ex);
            }
            return ret;
        }

        /// <summary>
        /// 添加用户组
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static OperationResult AddUserGroup(Dictionary<string, object> group)
        {
            try
            {
                PublicHelper.CheckArgument(group, "group");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupService").Error("AddUserGroup 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }

            OperationResult ret = new OperationResult(OperationResultType.Success);

            if (group["UnitID"].ToString().Trim() == "-1")
#if UNITTEST
                group["UnitID"] = "d079a6f3-5547-4313-a144-f952d524416d";
#else
                group["UnitID"] = ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim();
#endif

            group.Add("Ug_create_time", DateTime.Now);
#if UNITTEST
            group.Add("Ug_create_name", "圆西瓜");
#else
            group.Add("Ug_create_name", ServiceBase.GetInfo(ServiceBase.USERFULLNAME));
            
#endif

            group.Add("Ug_IsEnable", true);
            var guid = Guid.NewGuid().ToString().ToString();
            group.Add("Ug_id",guid);

            var temp = DataBll.GetCount("t_UserGroup", "Ug_Name = '" + group["Ug_Name"].ToString().Trim() + "' and UnitID = '" + group["UnitID"].ToString().Trim()+"'");
            if (temp != 0)
                return new OperationResult(OperationResultType.ParamError, "用户组已存在", new { Success = false, Message = "用户组已存在" });

            try
            {
                using (var tran = new TransactionScope())
                {
                    var added = DataBll.Add(group, "t_UserGroup");
#if UNITTEST
                    //OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "新增名为:" + group["Ug_Name"] + "的用户组成功", "");
                    UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户组管理-新增用户组", "新增名为:" + group["Ug_Name"] + "的用户组成功", "");
#else
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "新增名为:" + group["Ug_Name"] + "的用户组成功", "");
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户组管理-新增用户组", "新增名为:" + group["Ug_Name"] + "的用户组成功", "");
#endif
                    ret.Message = "新增用户组成功,请到系统设置-权限管理中为用户组分配权限模版.";
                    ret.AppendData = new { Success = true, Message = ret.Message };
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupService").Error("AddUserGroup 发生异常", ex);
                return new OperationResult(OperationResultType.Error, "发生异常", ex.Message);
            }
            return ret;
        }
        /// <summary>
        /// 修改用户组
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static OperationResult EditUserGroup(Dictionary<string, object> group)
        {
            try
            {
                PublicHelper.CheckArgument(group, "userGroup");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupService").Error("EditUserGroup 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }

            OperationResult ret = new OperationResult(OperationResultType.Success, "编辑用户组成功");
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();

            if (group["UnitID"].ToString() == "-1")
#if UNITTEST
                group["UnitID"] = "d079a6f3-5547-4313-a144-f952d524416d";//ServiceBase.GetInfo(ServiceBase.UNITID);
#else
                group["UnitID"] = ServiceBase.GetInfo(ServiceBase.UNITID);
#endif

            dicCondition.Add("Ug_Name", group["Ug_Name"]);
            dicCondition.Add("UnitID", group["UnitID"]);

            //var tempgroup = DataBll.GetModel("", "t_UserGroup", dicCondition);
            //if (tempgroup.Tables[0].Rows.Count != 0 && tempgroup.Tables[0].Rows[0]["Ug_Name"].ToString().Trim() != group["Ug_Name"].ToString().Trim())
            //    return new OperationResult(OperationResultType.ParamError, "用户组已存在", new { Success = false, Message = "用户组已存在" });

            //判断用户名是否重复(同一机构下)
            string sqlrepeat = "select * from t_UserGroup where Ug_Name = '" + group["Ug_Name"] + "' and UnitID = '" + group["UnitID"] + "' and Ug_id != '" + group["Ug_id"] + "'";
            DataSet ds = DataBll.Query(sqlrepeat);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return new OperationResult(OperationResultType.Error, "用户组名已存在", new { Success = false, Message = "用户组名已存在" });
            }

            try
            {
                using (var tran = new TransactionScope())
                {
                    dicCondition.Clear();
                    dicCondition.Add("Ug_id", group["Ug_id"]);
                    DataBll.Update(group, "t_UserGroup", dicCondition, "Ug_id");
#if UNITTEST
                  //  OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "编辑ID为:" + group["Ug_id"] + "的用户组成功", "");
                    UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户组管理-编辑用户组", "编辑ID为:" + group["Ug_id"] + "的用户组成功", "");
#else
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "编辑ID为:" + group["Ug_id"] + "的用户组成功", "");
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户组管理-编辑用户组", "编辑ID为:" + group["Ug_id"] + "的用户组成功", "");
#endif

                    var data = new
                    {
                        Success = true,
                        Message = "编辑用户组成功"
                    };
                    ret.AppendData = data;
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("OrgnazationUserGroupService").Error("EditUserGroup 发生异常", ex);
                return new OperationResult(OperationResultType.Error, "发生异常", new { Success = false, Message = "编辑用户组失败" });
            }
            return ret;
        }
        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static OperationResult DeleteUserGroup(string ids)
        {
            try
            {
                PublicHelper.CheckArgument(ids, "ids");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupService").Error("DeleteUserGroup 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }

            if (ids.Length == 0)
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            var arrID = ids.Split(',');
            if (arrID.Length == 0)
                return new OperationResult(OperationResultType.ParamError, "参数异常");

            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    string returnstring = "";

                    ids = "'" + ids + "'";
                    ids = ids.Replace(",", "\',\'");
                    var grouplst = DataBll.Query("select * from t_UserGroup where Ug_id in (" + ids + ")");

                    foreach (DataRow item in grouplst.Tables[0].Rows)
                    {
                        int count = DataBll.GetCount("t_UserAndUserGroup", "Ug_id = '" + item["Ug_id"].ToString().Trim() + "'");
                        if (count > 0)
                            returnstring += item["Ug_Name"].ToString().Trim() + ":" + count + ",";
                    }

                    if (returnstring.Length > 0)
                    {
                        returnstring = returnstring.TrimEnd(',');
                        var LstError = returnstring.Split(',');
                        foreach (var item in LstError)
                        {
                            var arrErrorID = item.Split(':');
#if UNITTEST
                           // OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "删除名为:" + arrErrorID[0] + "的用户组失败,有" + arrErrorID[1] + "个用户属于组", "");
                            UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户组管理-删除用户组", "删除名为:" + arrErrorID[0] + "的用户组失败,有" + arrErrorID[1] + "个用户属于组", "");
#else
                            OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "删除名为:" + arrErrorID[0] + "的用户组失败,有" + arrErrorID[1] + "个用户属于组", "");
                            UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户组管理-删除用户组", "删除名为:" + arrErrorID[0] + "的用户组失败,有" + arrErrorID[1] + "个用户属于组", "");
#endif
                        }
                    }

                    if (!returnstring.Equals(""))
                    {
                        ret.AppendData = returnstring;
                        ret.Message = "有用户组内有用户,无法删除";
                        ret.ResultType = OperationResultType.IllegalOperation;
                        return ret;
                    }
                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                    foreach (DataRow item in grouplst.Tables[0].Rows)
                    {
                        dicCondition.Clear();
                        dicCondition.Add("Ug_id", item["Ug_id"]);
                        DataBll.Delete(dicCondition, "t_UserGroup");//用户组
                        DataBll.Delete(dicCondition, "t_UserAndUserGroup");//用户组添加用户表
                        DataBll.Delete(dicCondition, "t_UserGroupFunction");//用户组权限表
                        //DataBll.Delete(dicCondition, "UserGroupCategory");
#if UNITTEST
                       // OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "删除名为:" + item["Ug_Name"] + "的用户组成功", "");
                        UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户组管理-删除用户组", "删除名为:" + item["Ug_Name"] + "的用户组成功", "");
#else
                        OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "删除名为:" + item["Ug_Name"] + "的用户组成功", "");
                        UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户组管理-删除用户组", "删除名为:" + item["Ug_Name"] + "的用户组成功", "");
#endif
                    }

                    ret.AppendData = new
                    {
                        Success = true,
                        Message = "删除用户组成功"
                    };
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupService").Error("DeleteUserGroup 发生异常", ex);
                return new OperationResult(OperationResultType.Error, "发生异常", new { Success = false, Message = "删除用户组失败" });
            }
            return ret;
        }

        /// <summary>
        /// 获取用户组用户
        /// </summary>
        /// <param name="GroupID">用户组id</param>
        /// <param name="type">0:获取不在用户组内的用户，1：获取在用户组内的用户</param>
        /// <param name="UnitID">单位id</param>
        /// <returns></returns>
        public static OperationResult GetUserList(string GroupID, string type, string UnitID)
        {
            try
            {
                PublicHelper.CheckArgument(GroupID, "GroupID");
                PublicHelper.CheckArgument(type, "type", true);
                PublicHelper.CheckArgument(UnitID, "UnitID", true);
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupService").Error("GetUserList 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                if (GroupID == "")
                {
                    return new OperationResult(OperationResultType.ParamError, "参数异常");
                }
                if (UnitID == "-1")
                {
                    UnitID = ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim();
                }
                    
                var sqlstr = "";
                var sqlcount = "";
                if (type == "0")
                {
                    //sqlstr = "select us.* from t_users us where us_id not in( select us_id from t_UserAndUserGroup usg where usg.Ug_id = '" + GroupID + "') and UnitID='" + UnitID + "' order by us_create_time desc";
                    sqlstr = "select us.* from t_users us where us_id not in( select us_id from t_UserAndUserGroup usg ) and UnitID='" + UnitID + "' order by us_create_time desc";
                    sqlcount = "select count(*) from t_users us where us_id not in( select us_id from t_UserAndUserGroup usg where usg.Ug_id = '" + GroupID + "') and UnitID='" + UnitID + "'";
                }
                else {
                    sqlstr = "select us.* from t_Users us left join t_UserAndUserGroup usg on us.Us_id = usg.Us_id where usg.Ug_id = '" + GroupID + "' and UnitID = '" + UnitID + "' order by us_create_time desc";
                    sqlcount = "select count(*) from t_Users us left join t_UserAndUserGroup usg on us.Us_id = usg.Us_id where usg.Ug_id = '" + GroupID + "' and UnitID = '" + UnitID + "'";
                }                   
                var retds = DataBll.Query(sqlstr).Tables[0];
                retds = DataTrim.DataTableTrim(retds); //去空格
                string count = DataBll.Query(sqlcount).Tables[0].Rows[0][0].ToString().Trim(); 
                                                                    
                ret.Message = "查询成功";
                ret.AppendData = new
                {
                    total = count,
                    rows = retds
                };

                //添加用户日志和操作日志
                OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "查询用户组列表成功", "");
                UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户组管理-查询用户组列表", "查询成功", "");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupService").Error("GetUserList 发生异常", ex);
                return new OperationResult(OperationResultType.Error, "发生异常");
            }
            return ret;
        }

        public static OperationResult ChangeGroupUsers(string[] deleted, string[] added, string groupID)
        {
            try
            {
                PublicHelper.CheckArgument(deleted, "deleted");
                PublicHelper.CheckArgument(added, "added");
                PublicHelper.CheckArgument(groupID, "groupID");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupService").Error("ChangeGroupUsers 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常", new { Success = false, Message = "为组分配用户失败, 传入参数错误" });
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                using (var tran = new TransactionScope())
                {
                    if (deleted.Length > 0)
                    {
                        string deletedids = "";
                        foreach (var item in deleted)
                            deletedids += "'" + item + "',";
                        deletedids = deletedids.TrimEnd(',');
                        DataBll.Query("delete from t_UserAndUserGroup where Us_id in (" + deletedids + ")");
                        //DataBll.Query("delete from t_UserCategory where Us_id in (" + deletedids + ")");
                        //DataBll.Query("delete from t_UserAndUserGroup where us_id in (" + deletedids + ")");
                    }

                    //删除这些用户旧的权限数据
                    if (added.Length > 0)
                    {
                        string addedids = "";
                        foreach (var item in added)
                            addedids += "'" + item + "',";
                        addedids = addedids.TrimEnd(',');
                        //删除用户的功能权限
                        DataBll.Query("delete from t_UserFunction where Us_id in (" + addedids + ")");
                        //删除用户的数据权限
                        //DataBll.Query("delete from t_UserCategory where Us_id in (" + addedids + ")");
                    }

                    var ugfplist = DataTrim.DataTableTrim(DataBll.Query("select * from t_UserGroupFunction where Ug_id = '" + groupID + "'").Tables[0]);
                   // var ugcplist = DataBll.Query("select * from t_UserGroupCategory where Ug_id = '" + groupID + "'");
                    //配置为用户组的权限数据
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    foreach (var item in added)
                    {
                        values.Clear();
                        values.Add("Us_id", item);
                        values.Add("Ug_id", groupID);
                        values.Add("Utg_id",Guid.NewGuid());
                        Dictionary<string, object> ufp = new Dictionary<string, object>();
                       // Dictionary<string, object> ucp = new Dictionary<string, object>();
                        foreach (DataRow drfp in ugfplist.Rows)
                        {
                            ufp.Clear();
                            ufp.Add("Func_id", drfp["Func_id"]);
                            ufp.Add("Func_grade", drfp["Func_grade"]);
                            ufp.Add("Full_url", drfp["Full_url"]);
                            ufp.Add("Us_id", item);
                            DataBll.Add(ufp, "t_UserFunction");
                        }

                        //foreach (DataRow drcp in ugcplist.Tables[0].Rows)
                        //{
                        //    ucp.Clear();
                        //    ucp.Add("Cate_id", drcp["Cate_id"]);
                        //    ucp.Add("Uc_isSonCate", drcp["Ugc_isSonCate"]);
                        //    ucp.Add("Uc_isChangeFiles", drcp["Ugc_isChangeFiles"]);
                        //    ucp.Add("Uc_isViewFiles", drcp["Ugc_isViewFiles"]);
                        //    ucp.Add("UnitID", drcp["Unit_code"]);
                        //    ucp.Add("Us_id", item);
                        //    DataBll.Add(ucp, "UserCategory");
                        //}

                        DataBll.Add(values, "t_UserAndUserGroup");
                    }

                    //为加入用户组的用户设置用户组拥有的功能权限
                    PermissionManageService.SetUserPermissionByAddUserToUserGroup(added, groupID);
                    //为加入用户组的用户设置用户组所拥有的数据权限
                    // UserGroupPermissionService.SetUserDataPermissionByAddUserToUserGroup(added, groupID);

#if UNITTEST
                    OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, "为用户组:" + groupID + "添加" + deleted.Length + "个用户", "");
                    UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户组管理-为组配置用户", "为用户组:" + groupID + "添加" + deleted.Length + "个用户", "");
#else
                    OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "为用户组:" + groupID + "添加" + deleted.Length + "个用户", "");
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户组管理-为组配置用户", "为用户组:" + groupID + "添加" + deleted.Length + "个用户", "");
#endif
                    ret.Message = "为用户组分配用户成功,现在组内用户都拥有与用户组相同的权限配置了.";
                    var data = new
                    {
                        Success = true,
                        Message = ret.Message
                    };
                    ret.AppendData = data;

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupService").Error("GetUserList 发生异常", ex);
                return new OperationResult(OperationResultType.Error, "发生异常", new { Success = false, Message = "为组分配用户失败,操作发生异常" });
            }
            return ret;
        }

        /// <summary>
        /// 启用禁用
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static OperationResult ChangeUserGroupStatus(string type, string ids)
        {
            try
            {
                PublicHelper.CheckArgument(type, "type", true);
                PublicHelper.CheckArgument(ids, "ids");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupService").Error("ChangeUserGroupStatus 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);

            try
            {
                using (var tran = new TransactionScope())
                {
                    var strids = ids.Split(',');
                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                    Dictionary<string, object> dicValues = new Dictionary<string, object>();
                    dicValues.Add("Ug_IsEnable", type);
                    foreach (var item in strids)
                    {
                        dicCondition.Clear();
                        dicCondition.Add("Ug_id", item);
                        DataBll.Update(dicValues, "t_UserGroup", dicCondition, "Ug_id");
#if UNITTEST
                       // OperationLogService.InsertLog(new Guid().ToString(), "192.168.1.62", DateTime.Now, (type == 1 ? "启用" : "禁用") + "ID为:" + item + " 的用户组成功", "");
                        UserLogService.InsertLog("admin", "圆西瓜", "192.168.1.62", DateTime.Now, "系统设置-用户组管理-" + (type == "false" ? "启用" : "禁用") + "用户", "用户组ID为: " + item, "");
#else
                        OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, (type == "true" ? "启用" : "禁用") + "ID为:" + item + " 的用户组成功", "");
                        UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-用户组管理-" + (type == "true" ? "启用" : "禁用") + "用户", "用户组ID为: " + item, "");
#endif
                    }
                    tran.Complete();
                }
                ret = new OperationResult(OperationResultType.Success);
                var data = new { Success = true, Message = (type == "true" ? "启用" : "禁用") + "用户组成功" };
                ret.AppendData = data;
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupService").Error("ChangeUserGroupStatus 发生异常", ex);
                return new OperationResult(OperationResultType.Error, "发生异常", new { Success = true, Message = (type == "true" ? "启用" : "禁用") + "用户组失败" });
            }
            return ret;
        }
    }
}
