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
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Service
 * 项目描述: 
 * 类 名 称: UserGroupPermissionService
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: Administrator
 * 命名空间: GraphicsEvaluatePlatform.Service
 * 文件名称: UserGroupPermissionService
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/4 9:31:53
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Service
{
    public static class UserGroupPermissionService
    {

        private static List<DataUserGroupPermissionTree> ListResult { get; set; }

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
                            wherestr += ((wherestr == "" ? "" : " and ") + " Ug_IsEnable ='True'");

                        }
                        else if (ustype == "2")//如果是一般管理员
                        {
                            var UnitName = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString().Trim();
                            var unitids = DataBll.Query("select u_Id from t_Units where u_FullPath = '" + UnitName + "' or u_FullPath like '%" + UnitName + "-%' or u_FullPath like '%-" + UnitName + "'");
                            var idstr = "";
                            foreach (DataRow iditem in unitids.Tables[0].Rows)
                                idstr += "'" + iditem[0].ToString() + "',";
                            idstr = idstr.Substring(0, idstr.Length - 1);
                            wherestr += ((wherestr == "" ? "" : " and ") + " UnitID in (" + idstr + ") and Ug_IsEnable ='True'");
                        }
                        else//如果是一般操作员
                        {
                            var UnitID = ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim();
                            wherestr += ((wherestr == "" ? "" : " and ") + " UnitID = '" + UnitID + "'");
                        }
                    }
                    else
                    {
                        wherestr += ((wherestr == "" ? "" : " and ") + " UnitID = '" + unitid + "'");
                    }

                    //var datas = DataBll.GetDataSetList("t_UserGroup", pager.PageSize, pager.PageIndex, "", wherestr, "Ug_create_time desc", "Ug_id");
                    string sql = "select * from t_UserGroup where" + wherestr;
                    DataSet datas = DataBll.Query(sql);
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
        /// 获取用户组权限
        /// </summary>
        /// <param name="userGroupId">用户组ID</param>
        /// <returns></returns>
        public static OperationResult GetUserGroupPermission(string userGroupId)
        {

            try
            {
                PublicHelper.CheckArgument(userGroupId, "userGroupId");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("OrgnazationUserAndPermission").Error("userId,list 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult op = new OperationResult(OperationResultType.Success);
            try
            {
                List<PermissionTree> treeList = GetAllMenuList();
                List<PermissionTree> CopyTreeList = new List<PermissionTree>();
                DataSet useGroupFuncList = DataBll.Query("select * from t_UserGroupFunction where Ug_id='" + userGroupId + "'");
                foreach (var itm in treeList)
                {
                    if (itm.ParentId == "")
                    {
                        var tree = new PermissionTree();
                        tree.id = itm.id;
                        tree.ParentId = itm.ParentId;
                        tree.permissionsSelect = itm.permissionsSelect;
                        tree.iconCls = itm.iconCls;
                        tree.data = itm.data;
                        tree.children = itm.Checked;
                        tree.Remark = itm.Remark;
                        tree.text = itm.text;
                        tree.Level = itm.Level;
                        CopyTreeList.Add(tree);
                    }

                }
                treeList.ForEach(t =>
                {
                    GetPermissionSelect(t, useGroupFuncList, userGroupId);
                });

                UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-权限管理-查询用户组权限", "查询成功", "");

                op.AppendData = treeList;
            }
            catch (DataAccessException e)
            {
                op.ResultType = OperationResultType.Error;
                op.Message = e.Message;
            }

            return op;
        }


        /// <summary>
        /// 获取权限选择
        /// </summary>
        private static bool GetPermissionSelect(PermissionTree tr, DataSet useGroupFuncList, string userGroupId)
        {
            if (tr.text != "首页")
                tr.permissionsSelect = GetPermissionSelectDetail(tr.id, useGroupFuncList, userGroupId);
            if (tr.children != null)
            {
                List<PermissionTree> list = (List<PermissionTree>)tr.children;
                list.ForEach(t =>
                {
                    GetPermissionSelect(t, useGroupFuncList, userGroupId);
                });
            }
            return true;
        }


        /// <summary>
        /// 获取菜单权限控件字符串
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="usePerList"></param>
        /// <returns></returns>
        private static string GetPermissionSelectDetail(string menuId, DataSet useGroupFuncList, string userGroupId)
        {

            string html = "";//html控件
            string hiddenValue = "";//隐藏操作值
            DataTable menuPerList = DataTrim.DataTableTrim(DataBll.Query("select * from t_SystemFunction where Func_type=1 and Func_parent_id='" + menuId + "' order by Func_sequence asc").Tables[0]);//该菜单所拥有的功能



            foreach (DataRow dr in menuPerList.Rows)
            {
                DataTable userFuncPers = DataTrim.DataTableTrim(DataBll.Query("select * from t_UserGroupFunction where Func_id='" + dr["Func_id"] + "' and Ug_id='" + userGroupId + "'").Tables[0]);//该用户在该菜单中拥有的权限

                int selectIndex = 0;//权限选择默认项 （0.禁止，1.允许，2..身份验证，3上级授权）
                string valueStr = dr["Func_id"] + "+" + dr["Func_name"] + "+" + dr["Func_full_name"] + "+" + dr["Func_urlPath"];//将权限的所有值都放在这个字段中传给前台
                string selectStr = "";//select 控件html
                string pname = dr["Func_name"].ToString();//权限名
                string selected = "selected = \"selected\"";
                string noselected = "";
                foreach (DataRow du in userFuncPers.Rows)
                {
                    //判断权限
                    if (du["Func_id"].ToString() == dr["Func_id"].ToString())
                    {
                        selectIndex = Convert.ToInt32(du["Func_grade"]);
                    }
                }
                selectStr = pname + "<select class=\"selectoptionGroup\" name=\"" + menuId + "\" value=2 id=\"sel_usergroup_" + dr["Func_id"] + "\"><option value=\"0\" " + (selectIndex == 0 ? selected : noselected) + " >禁止</option>" +
                            "<option value=\"1\" " + (selectIndex == 1 ? selected : noselected) + " >允许</option>" +
                            "<option value=\"2\" " + (selectIndex == 2 ? selected : noselected) + " >身份验证</option>" +
                            "<option value=\"3\" " + (selectIndex == 3 ? selected : noselected) + " >上级授权</option></select><input type=\"hidden\" name=\"hidden_value_usergroup\" value=\"" + valueStr + "\" />";
                if (html == "")
                {
                    html = selectStr;
                }
                else
                {
                    html += "&nbsp;|&nbsp;" + selectStr;
                }

            }
            return html + " " + "<input id=\"" + menuId + "\" type=\"hidden\" name=\"field＿name\" value=\"" + hiddenValue.Replace("\"", "'") + "\"> ";
        }


        /// <summary>
        /// 获取所有的菜单列表，以PermissionTree的形式存着
        /// </summary>
        /// <returns></returns>
        private static List<PermissionTree> GetAllMenuList()
        {
            List<PermissionTree> treeList = new List<PermissionTree>();

            DataSet ds = DataBll.Query("select * from dbo.t_SystemFunction where Func_type=0  order by Func_sequence asc");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //if (dr["Func_parent_id"].ToString().Trim() == "")
                //{
                PermissionTree model = new PermissionTree();
                model.ParentId = dr["Func_parent_id"].ToString().Trim(); ;
                model.id = dr["Func_id"].ToString().Trim();
                model.text = dr["Func_name"].ToString().Trim();
                model.children = getChildMenuList(ds.Tables[0], dr);
                model.Level = dr["Depth"].ToString().Trim();
                treeList.Add(model);

                // }
            }
            return treeList;
        }


        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <param name="menuList">所有菜单</param>
        /// <param name="parentMenu">父菜单</param>
        /// <returns>List<TreeModel></returns>
        private static List<PermissionTree> getChildMenuList(DataTable dt, DataRow parentMenu)
        {

            List<PermissionTree> treeList = new List<PermissionTree>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Func_parent_id"].ToString() == parentMenu["Func_id"].ToString())
                {
                    PermissionTree model = new PermissionTree();
                    model.id = dr["Func_id"].ToString();
                    model.text = dr["Func_name"].ToString();
                    model.children = getChildMenuList(dt, dr);
                    treeList.Add(model);
                }
            }
            return treeList;
        }


        /// <summary>
        /// 保存用户组权限
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public static OperationResult SaveUserGroupPermission(string data, string userGroupId)
        {
            try
            {
                PublicHelper.CheckArgument(userGroupId, "userGroupId");
                PublicHelper.CheckArgument(data, "data");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("UserGroupPermissionService").Error("userGroupId,data 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult op = new OperationResult(OperationResultType.Success);
            op.ResultType = OperationResultType.Success;
            op.Message = "权限保存成功";

            List<UserGroupPermissionModel> list = JsonUtil.ConvertToObject<List<UserGroupPermissionModel>>(data);

            using (var tran = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))

            {
                try
                {
                    //该用户是否已经存在权限,存在删除旧权限
                    int count = DataBll.GetCount("t_UserGroupFunction", "Ug_id='" + userGroupId + "'");
                    if (count > 0)
                    {
                     
                        string desql = "delete t_UserGroupFunction where Ug_id='" + userGroupId + "'";

                        DataBll.Query(desql);
                    }

                    foreach (UserGroupPermissionModel item in list)
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("Ugf_id", Guid.NewGuid());
                        dic.Add("Func_id", item.Func_id);
                        dic.Add("Full_url", item.Full_url);
                        dic.Add("Ug_id", userGroupId);
                        dic.Add("Func_grade", item.Func_grade);

                        var tp2 = DataBll.Add(dic, "t_UserGroupFunction");
                    }
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-权限管理-保存用户组权限", "保存成功", "");
                    //提交事务
                    tran.Complete();

                }
                catch(Exception ex)
                {
                    throw ex;
                    op.ResultType = OperationResultType.Error;
                    op.Message = "保存权限失败";
                }


            }
            return op;
        }
    }
}
