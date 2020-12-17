//#define UNITTEST
/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2017. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using System.Data;
using GraphicsEvaluatePlatform.Repository;
using GraphicsEvaluatePlatform.Model;
using System.Data.SqlClient;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using System.Transactions;
using GraphicsEvaluatePlatform.Infrastructure.Encrypt;
using System.Web;



/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Service
 * 项目描述: 
 * 类 名 称: Permission
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Service
 * 文件名称: Permission
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/4/25 9:42:48
 ***********************************************************************/



namespace GraphicsEvaluatePlatform.Service
{
    public static class PermissionManageService
    {
        private static List<DataPermissionTree> ListResult { get; set; }

        /// <summary>
        /// 获取权限用户列表
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
                Logger.GetLogger("PermissionManageService").Error("GetUserList 发生异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数错误");
            }

            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                var unitid = "";
                var filters = pager.filter.Split(',');
                unitid = filters[1].Split(':')[1];
                pager.filter = filters[0];

#if UNITTEST
                var user_Type = "1";
                string unit_FullPath = "泰坦软件研发部";
#else
                var user_Type = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString().Trim();
                string unit_FullPath = ServiceBase.GetInfo(ServiceBase.UNITFULLPATH).ToString().Trim();
#endif
                string unitWhere = "";
                //搜索关键字
                if (pager.filter != "") {
                    unitWhere = "Us_account like '%" + pager.filter + "%' or Us_name like '%" + pager.filter + "%'";
                }

                //用户权限
                if (unitid == "-1")//选取全部
                {
                    if (user_Type == "1")//超级管理员（获取所有单位数据）
                    {
                        unitWhere += ((unitWhere == "" ? "" : " and ") + " Us_status = 'True'");
                    }
                    else if (user_Type == "2")//一般管理员（获取本级和下级机构数据）
                    {
                        var UnitName = ServiceBase.GetInfo(ServiceBase.UNITFULLNAME).ToString().Trim();
                        var unitids = DataBll.Query("select u_Id from t_Units where u_FullPath = '" + UnitName + "' or u_FullPath like '%" + UnitName + "-%' or u_FullPath like '%-" + UnitName + "'");
                        var idstr = "";
                        foreach (DataRow iditem in unitids.Tables[0].Rows)
                            idstr += "'" + iditem[0].ToString() + "',";
                        idstr = idstr.Substring(0, idstr.Length - 1);
                        unitWhere += ((unitWhere == "" ? "" : " and ") + " UnitID in (" + idstr + ") and Us_status = 'True'");//本级和下级的机构id
                    }
                    else {//一般操作员（获取本单位数据）
                        var UnitID = ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim();
                        unitWhere += ((unitWhere == "" ? "" : " and ") + " UnitID = '" + UnitID + "'");
                    }
                }else {//选择某一单位
                    unitWhere += ((unitWhere == "" ? "" : " and ") + " UnitID = '" + unitid + "'");
                }

                //var datas = DataBll.GetDataSetList("t_Users", pager.PageSize, pager.PageIndex, "", unitWhere, "Us_create_time desc", "Us_id");//分页
                string sql = "select * from t_Users where" + unitWhere;//不分页
                DataSet datas = DataBll.Query(sql);
                var count = DataBll.GetCount("t_Users", unitWhere);

                ret.Message = "查询成功";
                ret.AppendData = new { total = count, rows = DataTrim.DataTableTrim(datas.Tables[0]) };

                //if (user_Type.ToString().Trim() != "1")
                //{
                //    //不是超级管理员，获取本机构和下级机构的机构Id
                //    DataTable uds = DataTrim.DataTableTrim(DataBll.Query("select u_Id from t_Units where u_FullPath='" + unit_FullPath + "'").Tables[0]);
                //    if (uds.Rows.Count > 0)
                //    {
                //        unitWhere = "and UnitID in (";
                //        foreach (DataRow dr in uds.Rows)
                //        {
                //            unitWhere += dr["u_Id"].ToString() + ",";
                //        }
                //        unitWhere = unitWhere.TrimEnd(',');
                //        unitWhere += ")";
                //    }

                //}
                ////超级管理员查询所有人员，不是超级管理员只查询本机构和下级机构的人员
                //var datas = DataBll.GetDataSetList("t_Users", pager.PageSize, pager.PageIndex, "", " (Us_name like '%" + pager.filter + "%' or Us_account like '%" + pager.filter + "%') " + (unitid == "-1" ? "" + unitWhere : ("and UnitID = '" + unitid + "'")) + " and Us_status='true'", "Us_create_time desc", "Us_id");
                //var count = DataBll.GetCount("t_Users", "(Us_name like '%" + pager.filter + "%' or Us_account like '%" + pager.filter + "%') " + (unitid == "-1" ? "" + unitWhere : ("and UnitID = '" + unitid + "'")) + " and Us_status='true'");               
                //ret.AppendData = new
                //{
                //    total = count,
                //    rows = DataTrim.DataTableTrim(datas.Tables[0])
                //};

                OperationLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERID).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "查询用户列表成功", "");
                UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-权限管理-查询用户权限", "查询成功", "");

            }
            catch (Exception ex)
            {
                Logger.GetLogger("PermissionManageService").Error("GetUserList 发生异常", ex);
                ret = new OperationResult(OperationResultType.Error);
            }
            return ret;
        }


        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public static OperationResult GetUserPermission(string userId)
        {

            try
            {
                PublicHelper.CheckArgument(userId, "userId");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("GetUserPermission").Error("userId,list 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult op = new OperationResult(OperationResultType.Success);
            try
            {
                List<PermissionTree> treeList = GetAllMenuList();
                List<PermissionTree> CopyTreeList = new List<PermissionTree>();
                DataTable useFuncList = DataTrim.DataTableTrim(DataBll.Query("select * from t_UserFunction where Us_id='" + userId + "'").Tables[0]);
             
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
                    GetPermissionSelect(t, useFuncList, userId);
                });

                UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-权限管理-查询用户权限", "查询成功", "");

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
        /// 数据权限---用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public static OperationResult GetDataUserPermission(string userId, string unitId)
        {
            try
            {
                PublicHelper.CheckArgument(userId, "userId");
                PublicHelper.CheckArgument(unitId, "unitId");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("GetDataUserPermission").Error("userId,list 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult ret = new OperationResult(OperationResultType.Success);
            try
            {
                if (userId == "")
                    userId = "00000000-0000-0000-0000-000000000000";
                string sql = "select Cate_id,Cate_name,Cate_parent_id,CodeType,Cate_full_name from category where UnitID='" + unitId + "' or Cate_parent_id ='00000000-0000-0000-0000-000000000000' order by Cate_sequence asc";
                DataSet ds = DataBll.Query(sql);
                List<DataPermissionTree> listCategory = new List<DataPermissionTree>();
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    listCategory.Add(new DataPermissionTree { id = dt.Rows[i]["Cate_id"].ToString(), text = dt.Rows[i]["Cate_name"].ToString(), ParentId = dt.Rows[i]["Cate_parent_id"].ToString(), state = "open", codetype = dt.Rows[i]["codetype"].ToString(), Cate_full_name = dt.Rows[i]["Cate_full_name"].ToString() });
                }
                ListResult = new List<DataPermissionTree>();

                DataSet useFuncList = DataBll.Query("select * from UserCategory where Us_id='" + userId + "'");

                //生成 树节点时，根据 pid=0的根节点 来生成  
                GetCategoryTree(listCategory, "00000000-0000-0000-0000-000000000000");

                ListResult.ForEach(
                 t => GetDataPermission(t, useFuncList, userId));
                ret.AppendData = ListResult;
            }
            catch (DataAccessException e)
            {
                ret.ResultType = OperationResultType.Error;
                ret.Message = e.Message;
            }

            return ret;
        }


        #region  分类树

        private static bool GetCategoryTree(List<DataPermissionTree> list, string parentId)
        {
            var children = list.Where(x => x.ParentId == parentId).ToList();
            if (children.Count > 0)
            {
                foreach (var item in children)
                {
                    BindChildren(list, item);
                    ListResult.Add(item);
                }
            }

            return true;
        }
        private static bool BindChildren(List<DataPermissionTree> list, DataPermissionTree model)
        {
            var children = list.Where(x => x.ParentId == model.id).ToList();
            if (children.Count > 0)
            {
                model.children = children;
                model.state = "closed";
                foreach (var item in children)
                {
                    BindChildren(list, item);
                }
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 绘制权限树
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="useFuncList"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static bool GetDataPermission(DataPermissionTree tr, DataSet useFuncList, string userId)
        {

            tr = GetDataPermissionDetail(tr, useFuncList, userId);
            if (tr.children != null)
            {
                List<DataPermissionTree> list = (List<DataPermissionTree>)tr.children;
                list.ForEach(t =>
                {
                    GetDataPermission(t, useFuncList, userId);
                });
            }

            return true;
        }

        /// <summary>
        /// 获取权限选择
        /// </summary>
        private static bool GetPermissionSelect(PermissionTree tr, DataTable useFuncList, string userId)
        {
            if (tr.text != "首页")
                tr.permissionsSelect = GetPermissionSelectDetail(tr.id, useFuncList, userId);
            if (tr.children != null)
            {
               
                List<PermissionTree> list = (List<PermissionTree>)tr.children;
                list.ForEach(t =>
                {
                    GetPermissionSelect(t, useFuncList, userId);
                });
            }

            return true;
        }


        private static DataPermissionTree GetDataPermissionDetail(DataPermissionTree tr, DataSet userDataPerList, string userId)
        {
            if (userDataPerList.Tables[0].Rows.Count > 0)
            {
                DataRow[] drs = userDataPerList.Tables[0].Select("Cate_id='" + tr.id + "'");
                if (drs.Count() > 0)
                {
                    DataRow dr = drs[0];
                    if (dr["Uc_isChangeFiles"].ToString() == "True")
                        tr.Uc_isChangeFiles = "<input type=\"checkbox\"  name=\"Uc_isChangeFiles\" dataparentid=\"" + tr.ParentId + "\" id=\"" + tr.id + "\" checked=\"checked\" fullpath=\"" + tr.Cate_full_name + "\" onclick='clickUserDataCheckbox(this);'/>";
                    else
                        tr.Uc_isChangeFiles = "<input type=\"checkbox\" id=\"" + tr.id + "\"  name=\"Uc_isChangeFiles\" dataparentid=\"" + tr.ParentId + "\" fullpath=\"" + tr.Cate_full_name + "\" onclick='clickUserDataCheckbox(this);'/>";
                    if (dr["Uc_isSonCate"].ToString() == "True")
                        tr.Uc_isSonCate = "<input type=\"checkbox\"  name=\"Uc_isSonCate\" dataparentid=\"" + tr.ParentId + "\" id=\"" + tr.id + "\" checked=\"checked\" fullpath=\"" + tr.Cate_full_name + "\" onclick='clickUserDataCheckbox(this);'/>";
                    else
                        tr.Uc_isSonCate = "<input type=\"checkbox\"  name=\"Uc_isSonCate\" dataparentid=\"" + tr.ParentId + "\" id=\"" + tr.id + "\" fullpath=\"" + tr.Cate_full_name + "\" onclick='clickUserDataCheckbox(this);'/>";
                    if (dr["Uc_isViewFiles"].ToString() == "True")
                        tr.Uc_isViewFiles = "<input type=\"checkbox\"  name=\"Uc_isViewFiles\" dataparentid=\"" + tr.ParentId + "\" id=\"" + tr.id + "\" checked=\"checked\" fullpath=\"" + tr.Cate_full_name + "\" onclick='clickUserDataCheckbox(this);'/>";
                    else
                        tr.Uc_isViewFiles = "<input type=\"checkbox\"  name=\"Uc_isViewFiles\" dataparentid=\"" + tr.ParentId + "\" id=\"" + tr.id + "\" fullpath=\"" + tr.Cate_full_name + "\" onclick='clickUserDataCheckbox(this);'/>";
                }
                else
                {
                    tr.Uc_isChangeFiles = "<input type=\"checkbox\"  name=\"Uc_isChangeFiles\" dataparentid=\"" + tr.ParentId + "\" id=\"" + tr.id + "\" fullpath=\"" + tr.Cate_full_name + "\" onclick='clickUserDataCheckbox(this);'/>";
                    tr.Uc_isSonCate = "<input type=\"checkbox\"  name=\"Uc_isSonCate\" dataparentid=\"" + tr.ParentId + "\" class=\"isSonCate\" id=\"" + tr.id + "\" fullpath=\"" + tr.Cate_full_name + "\" onclick='clickUserDataCheckbox(this);'/>";
                    tr.Uc_isViewFiles = "<input type=\"checkbox\"  name=\"Uc_isViewFiles\" dataparentid=\"" + tr.ParentId + "\" id=\"" + tr.id + "\" fullpath=\"" + tr.Cate_full_name + "\" onclick='clickUserDataCheckbox(this);'/>";
                }
            }
            else
            {
                tr.Uc_isChangeFiles = "<input type=\"checkbox\"  name=\"Uc_isChangeFiles\" dataparentid=\"" + tr.ParentId + "\" id=\"" + tr.id + "\" fullpath=\"" + tr.Cate_full_name + "\" onclick='clickUserDataCheckbox(this);'/>";
                tr.Uc_isSonCate = "<input type=\"checkbox\"  name=\"Uc_isSonCate\" dataparentid=\"" + tr.ParentId + "\" class=\"isSonCate\" id=\"" + tr.id + "\" fullpath=\"" + tr.Cate_full_name + "\" onclick='clickUserDataCheckbox(this);'/>";
                tr.Uc_isViewFiles = "<input type=\"checkbox\"  name=\"Uc_isViewFiles\" dataparentid=\"" + tr.ParentId + "\" id=\"" + tr.id + "\" fullpath=\"" + tr.Cate_full_name + "\" onclick='clickUserDataCheckbox(this);'/>";
            }
            tr.codetype = "<input type=\"checkbox\"  name=\"uc_codetype\" uc_cate_id=\"" + tr.id + "\" id=\"" + tr.id + "\" fullpath=\"" + tr.Cate_full_name + "\" onclick='clickUserDataCheckbox(this);'/>";
           
            return tr;
        }


        /// <summary>
        /// 获取菜单权限控件字符串
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="usePerList"></param>
        /// <returns></returns>
        private static string GetPermissionSelectDetail(string menuId, DataTable userPerList, string userId)
        {

            string html = "";//html控件
            string hiddenValue = "";//隐藏操作值
            DataTable menuPerList = DataTrim.DataTableTrim(DataBll.Query("select * from t_SystemFunction where Func_type=1 and Func_parent_id='" + menuId + "' order by Func_sequence asc").Tables[0]);//该菜单所拥有的功能
            
#if UNITTEST
            string CurrentUserId = "177a50ae-b82c-4e6c-b2c5-a03f5eb8fbd6";
            string userType = "1";
#else
            string CurrentUserId = ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim();
            string userType = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString().Trim();
#endif


            foreach (DataRow dr in menuPerList.Rows)
            {

                DataSet userFuncPers = DataBll.Query("select * from t_UserFunction where Func_id='" + dr["Func_id"] + "' and Us_id='" + userId + "'");//该用户在该菜单中拥有的权限

                DataSet CurrentUserFuncPers = DataBll.Query("select * from t_UserFunction where Func_id='" + dr["Func_id"] + "' and Us_id='" + CurrentUserId + "'");//当前登录者在该菜单拥有的权限

                int selectIndex = 0;//权限选择默认项 （0.禁止，1.允许，2..身份验证，3上级授权）

                int CurrentUserSelectIndex = 0; //当前登录者（拥有设置权限的用户），该功能的权限选择默认项
                string valueStr = dr["Func_id"] + "+" + dr["Func_name"] + "+" + dr["Func_full_name"] + "+" + dr["Func_urlPath"];//将权限的所有值都放在这个字段中传给前台
                string selectStr = "";//select 控件html
                string pname = dr["Func_name"].ToString();//权限名
                string selected = "selected = \"selected\"";
                string noselected = "";
                //设置的用户，有该功能权限记录数量
                int thisFuncCount = userFuncPers.Tables[0].Select("Func_id='" + dr["Func_id"] + "'").Count();
                //设置者（拥有设置权限的用户），有该功能权限记录数量
                int currentUserThisFunCount = CurrentUserFuncPers.Tables[0].Select("Func_id='" + dr["Func_id"] + "'").Count();
                if (thisFuncCount > 0)
                {
                    selectIndex = Convert.ToInt32((userFuncPers.Tables[0].Select("Func_id='" + dr["Func_id"] + "'")[0]["Func_grade"]).ToString());
                }

                if (currentUserThisFunCount > 0)
                {
                    CurrentUserSelectIndex = Convert.ToInt32((CurrentUserFuncPers.Tables[0].Select("Func_id='" + dr["Func_id"] + "'")[0]["Func_grade"]).ToString());
                }

                //超级管理员，取到所有权限
                if (userType == "1")
                {
                    selectStr += pname + "<select class=\"selectoption\" name=\"" + menuId + "\" value=2 id=\"sel_" + dr["Func_id"] + "\">";
                    selectStr += " <option value=\"0\" " + (selectIndex == 0 ? selected : noselected) + " >禁止</option>";
                    selectStr += "<option value=\"1\" " + (selectIndex == 1 ? selected : noselected) + " >允许</option>";
                    selectStr += "<option value=\"2\" " + (selectIndex == 2 ? selected : noselected) + " >身份验证</option>";
                    selectStr += "<option value=\"3\" " + (selectIndex == 3 ? selected : noselected) + " >上级授权</option>";
                    selectStr += "</select><input type=\"hidden\" name=\"hidden_value\" value=\"" + valueStr + "\" />";
                }
                else  //不是超级管理员只能取到自己的权限
                {
                    //用户为当前登录者
                    if (userId == CurrentUserId)
                    {
                        if (selectIndex == 1)
                        {
                            selectStr += pname + "<select class=\"selectoption\" name=\"" + menuId + "\" value=2 id=\"sel_" + dr["Func_id"] + "\">";
                            selectStr += " <option value=\"0\" " + (selectIndex == 0 ? selected : noselected) + " >禁止</option>";
                            selectStr += "<option value=\"1\" " + (selectIndex == 1 ? selected : noselected) + " >允许</option>";
                            selectStr += "<option value=\"2\" " + (selectIndex == 2 ? selected : noselected) + " >身份验证</option>";
                            selectStr += "<option value=\"3\" " + (selectIndex == 3 ? selected : noselected) + " >上级授权</option>";
                            selectStr += "</select><input type=\"hidden\" name=\"hidden_value\" value=\"" + valueStr + "\" />";
                        }
                        if (selectIndex == 2)
                        {
                            selectStr += pname + "<select class=\"selectoption\" name=\"" + menuId + "\" value=2 id=\"sel_" + dr["Func_id"] + "\">";
                            selectStr += " <option value=\"0\" " + (selectIndex == 0 ? selected : noselected) + " >禁止</option>";
                            selectStr += "<option value=\"2\" " + (selectIndex == 2 ? selected : noselected) + " >身份验证</option>";
                            selectStr += "<option value=\"3\" " + (selectIndex == 3 ? selected : noselected) + " >上级授权</option>";
                            selectStr += "</select><input type=\"hidden\" name=\"hidden_value\" value=\"" + valueStr + "\" />";
                        }
                        if (selectIndex == 3)
                        {
                            selectStr += pname + "<select class=\"selectoption\" name=\"" + menuId + "\" value=2 id=\"sel_" + dr["Func_id"] + "\">";
                            selectStr += " <option value=\"0\" " + (selectIndex == 0 ? selected : noselected) + " >禁止</option>";
                            selectStr += "<option value=\"3\" " + (selectIndex == 3 ? selected : noselected) + " >上级授权</option>";
                            selectStr += "</select><input type=\"hidden\" name=\"hidden_value\" value=\"" + valueStr + "\" />";
                        }
                    }
                    else  //自己有权限给别人设置权限
                    {

                        if (CurrentUserSelectIndex == 1)
                        {
                            selectStr = pname + "<select class=\"selectoption\" name=\"" + menuId + "\" value=2 id=\"sel_" + dr["Func_id"] + "\">";
                            selectStr += " <option value=\"0\" " + (selectIndex == 0 ? selected : noselected) + " >禁止</option>";
                            selectStr += "<option value=\"1\" " + (selectIndex == 1 ? selected : noselected) + " >允许</option>";
                            selectStr += "<option value=\"2\" " + (selectIndex == 2 ? selected : noselected) + " >身份验证</option>";
                            selectStr += "<option value=\"3\" " + (selectIndex == 3 ? selected : noselected) + " >上级授权</option>";
                            selectStr += "</select><input type=\"hidden\" name=\"hidden_value\" value=\"" + valueStr + "\" />";
                        }
                        if (CurrentUserSelectIndex == 2)
                        {
                            selectStr += pname + "<select class=\"selectoption\" name=\"" + menuId + "\" value=2 id=\"sel_" + dr["Func_id"] + "\">";
                            selectStr += " <option value=\"0\" " + (selectIndex == 0 ? selected : noselected) + " >禁止</option>";
                            selectStr += "<option value=\"2\" " + (selectIndex == 2 ? selected : noselected) + " >身份验证</option>";
                            selectStr += "<option value=\"3\" " + (selectIndex == 3 ? selected : noselected) + " >上级授权</option>";
                            selectStr += "</select><input type=\"hidden\" name=\"hidden_value\" value=\"" + valueStr + "\" />";
                        }
                        if (CurrentUserSelectIndex == 3)
                        {
                            selectStr += pname + "<select class=\"selectoption\" name=\"" + menuId + "\" value=2 id=\"sel_" + dr["Func_id"] + "\">";
                            selectStr += " <option value=\"0\" " + (selectIndex == 0 ? selected : noselected) + " >禁止</option>";
                            selectStr += "<option value=\"3\" " + (selectIndex == 3 ? selected : noselected) + " >上级授权</option>";
                            selectStr += "</select><input type=\"hidden\" name=\"hidden_value\" value=\"" + valueStr + "\" />";
                        }
                    }


                }
                if (html == "")
                {
                    html = selectStr;
                }
                else
                {
                    if (selectStr != "")
                        html += "&nbsp;|&nbsp;" + selectStr;
                }

            }
            return html + " " + "<input id=\"" + menuId + "\" type=\"hidden\" name=\"field＿name\" value=\"" + hiddenValue.Replace("\"", "'") + "\"> ";
        }


        /// <summary>
        /// 获取所有的菜单列表，PermissionTree
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
                    model.Level= dr["Depth"].ToString().Trim();
                    treeList.Add(model);

                //}
            }
            return treeList;
        }


        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <param name="dt">所有菜单</param>
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
                    model.id = dr["Func_id"].ToString().Trim();
                    model.text = dr["Func_name"].ToString();
                    model.children = getChildMenuList(dt, dr);
                    treeList.Add(model);
                }
            }
            return treeList;
        }


        /// <summary>
        /// 保存用户权限
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static OperationResult SaveUserPermission(string data, string userId)
        {
            try
            {
                PublicHelper.CheckArgument(userId, "userId");
                PublicHelper.CheckArgument(data, "data");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("PermissionManage").Error("userId,data 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult op = new OperationResult(OperationResultType.Success);
            op.ResultType = OperationResultType.Success;
            op.Message = "保存权限成功";

            List<PermissionModel> list = JsonUtil.ConvertToObject<List<PermissionModel>>(data);


            try
            {
                using (var tran = new TransactionScope(TransactionScopeOption.Required,new TransactionOptions (){ IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {


                    //该用户是否已经存在权限
                    int count = DataBll.GetCount("t_UserFunction", "Us_id='" + userId + "'");
                    if (count > 0)
                    {

                        Dictionary<string, object> dicw = new Dictionary<string, object>();
                        dicw.Add("Us_id", userId);
                        DataBll.Delete(dicw, "t_UserFunction");

                    }
#if UNITTEST
                    string CurrentUserId = "CEC304E9-D83D-4D95-9A70-76B0D5EFE60E";
#else
                    string CurrentUserId = ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim();
#endif

                    foreach (PermissionModel item in list)
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("Func_id", item.Func_id.Trim());
                        dic.Add("Full_url", item.Full_url.Trim());
                        dic.Add("Us_id", userId.Trim());
                        dic.Add("Func_grade", item.Func_grade.ToString().Trim());
                        dic.Add("createByUserId", CurrentUserId.Trim());


                        Dictionary<string, object> preDicw = new Dictionary<string, object>();
                        preDicw.Add("Uf_id", Guid.NewGuid().ToString());

                        //先预插入数据
                        DataSet reDs = DataBll.Add(preDicw, "t_UserFunction");
                        Dictionary<string, object> wdic = new Dictionary<string, object>();
                        wdic.Add("Uf_id", reDs.Tables[0].Rows[0]["Uf_id"].ToString().Trim()); 

                        DataBll.Update(dic, "t_UserFunction", wdic, "Uf_id");
                    }
                    UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置-权限管理-为用户分配权限", "分配成功", "");
                    tran.Complete();
                }

            }
            catch (Exception ex)
            {
                Logger.GetLogger("EFileLendManageService").Error("Approve发生异常", ex);
                op.Message = "操作发生异常";
                op.ResultType = OperationResultType.Error;
            }
            return op;
        }


        /// <summary>
        /// 获取当前用户权限,从数据库查询（存入session,或者用于判断权限是否冲突）
        /// </summary>
        /// <returns></returns>
        public static DataSet GetCurrentUserPermission()
        {
            string uid = ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim();
            Dictionary<string, object> dicw = new Dictionary<string, object>();
            dicw.Add("Us_id", uid);
            DataSet ds = DataBll.GetList("", "t_UserFunction", dicw);
            return ds;
        }


        /// <summary>
        /// 获取用户数据权限（登录时存入session）
        /// </summary>
        /// <returns></returns>
        public static DataSet GetCurrentUserDataPermission()
        {
            string uid = ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim();
            Dictionary<string, object> dicw = new Dictionary<string, object>();
            dicw.Add("Us_id", uid);
            DataSet ds = DataBll.GetList("", "t_UserCategory", dicw);
            return ds;

        }

        /// <summary>
        ///从session中去到当前登录用户的权限
        /// </summary>
        /// <returns></returns>
        public static List<PermissionModel> GetCurrentUserPermissionBySession()
        {

            List<PermissionModel> list = new List<PermissionModel>();
            DataSet ds = ((DataSet)ServiceBase.GetInfo(ServiceBase.USER_PERMISSION));
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in DataTrim.DataTableTrim(ds.Tables[0]).Rows)
                    {
                        PermissionModel model = new PermissionModel();
                        model.Func_id = item["Func_id"].ToString();
                        model.Us_id = item["Us_id"].ToString();
                        model.Full_url = item["Full_url"].ToString();
                        model.Func_grade = Convert.ToInt32(item["Func_grade"].ToString());

                        list.Add(model);
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }
        }



        /// <summary>
        /// 根据条件获取权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Full_url"></param>
        /// <returns></returns>
        public static DataSet GetPermissionMdoel(string userId, string Full_url)
        {
            Dictionary<string, object> dicw = new Dictionary<string, object>();
            dicw.Add("Us_id", userId);
            dicw.Add("Full_url", Full_url);
            return DataBll.GetList("", "t_UserFunction", dicw);

        }


        /// <summary>
        /// 获取权限级别
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static int GetCurrentUserPermissionGrade(string url)
        {

            url = url.Trim().ToLower();
            //session不存在，登录信息无效
            if (HttpContext.Current.Session == null)
            {
                return -1;
            }
            DataSet ds = (DataSet)ServiceBase.GetInfo(ServiceBase.USER_PERMISSION);
          
            //超级管理员,拥有所有权限
            if (ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString().Trim() == "1")
            {
                return 1;
            }
            else
            {
                if (DataTrim.DataTableTrim(ds.Tables[0]).Rows.Count > 0)
                {
                    DataRow[] dr = DataTrim.DataTableTrim(ds.Tables[0]).Select("Full_url='" + url + "'");
                    if (dr.Count() > 0)
                        return Convert.ToInt32(dr[0]["Func_grade"].ToString().Trim());
                    else
                        return 0;

                }
                else
                {
                    //表示没有权限
                    return 0;
                }
            }
        }



        /// <summary>
        ///（打开操作框前先做验证） 是否通过上级授权(导入、导出)
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="url">全路径</param>
        public static OperationResult GetActionHaveAuthoritiesHigher(string username, string pwd, string url)
        {
            try
            {
                PublicHelper.CheckArgument(username, "username");
                PublicHelper.CheckArgument(pwd, "pwd");
                PublicHelper.CheckArgument(url, "url");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("PermissionManageService").Error(" GetActionHaveAuthoritiesHigher 参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "抱歉参数传输有误");

            }
            OperationResult op = new OperationResult(OperationResultType.Success);
            pwd = EncryptPasswordFactory.GetEncipher(EncryptType.MD5).EncryptFor(pwd).Trim();

            DataSet ReUser = UserService.GetUserModel(username);
            if (ReUser.Tables[0].Rows.Count == 1)
            {
                if (ReUser.Tables[0].Rows[0]["Us_Password"].ToString().Trim() != pwd.Trim())
                {//密码不正确
                    op.ResultType = OperationResultType.Error;
                    op.Message = "该上级用户密码错误！";
                    return op;
                }
                //超级管理员
                if (ReUser.Tables[0].Rows[0]["Us_type"].ToString().Trim() == "1")
                {
                    SetAuthoritiesHigherRecords(username, url, ReUser);
                    op.ResultType = OperationResultType.Success;
                    op.Message = "通过验证！";
                    return op;

                }
                DataSet up = PermissionManageService.GetPermissionMdoel(ReUser.Tables[0].Rows[0]["Us_id"].ToString().Trim(), url);
                if (DataTrim.DataTableTrim(up.Tables[0]).Rows.Count != 1)
                {
                    //上级用户权限表中没有权限，
                    op.ResultType = OperationResultType.Error;
                    op.Message = "该上级用户没有该权限！";
                    return op;
                }
                else
                {
                    if (Convert.ToInt32(up.Tables[0].Rows[0]["Func_grade"].ToString().Trim()) == 3)
                    {//上级用户没有该功能的权限
                        op.ResultType = OperationResultType.Error;
                        op.Message = "该上级用户没有该权限！";
                        return op;
                    }
                    else
                    {//上级用户有权限
                        SetAuthoritiesHigherRecords(username, url, ReUser);
                        op.ResultType = OperationResultType.Success;
                        op.Message = "通过验证！";
                        return op;
                    }
                }
            }
            else
            {
                op.ResultType = OperationResultType.Error;
                op.Message = "该上级用户账号不存在！";
                return op;

            }
        }

        /// <summary>
        /// 是否通过身份验证
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static OperationResult GetActionHaveAuthentication(string pwd)
        {
            try
            {
                PublicHelper.CheckArgument(pwd, "pwd");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("PermissionManageService").Error(" GetActionHaveAuthoritiesHigher 参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "抱歉参数传输有误");

            }
            OperationResult op = new OperationResult(OperationResultType.Success);
            pwd = EncryptPasswordFactory.GetEncipher(EncryptType.MD5).EncryptFor(pwd).Trim();

            Users UserInfo = (Users)ServiceBase.GetInfo(ServiceBase.USERINFO);
            string password = UserInfo.Us_Password;
            if (password != pwd.Trim())
            {
                //当密码不正确时
                op.Message = "密码出错，请重新输入！";
                op.ResultType = OperationResultType.Error;
            }
            else
            {
                op.Message = "验证通过";
                op.ResultType = OperationResultType.Success;

            }

            return op;
        }


     

        /// <summary>
        /// 登录时检查当前登录者的权限（检查登录者的权限是否跟设置者的权限发生冲突）
        /// 1、登陆者权限等级比设置者的权限等级高，权限降级
        /// 2、设置者不存在该权限,删除该权限
        /// 3、 设置者已被删除，删除权限
        /// </summary>
        public static bool CheckCurrentUserIsConflict()
        {
            string userId = ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim();
            DataTable ds = DataTrim.DataTableTrim(GetCurrentUserPermission().Tables[0]);
            //设置者是否是超级管理员
            bool isSuperManager = true;

            //是否存在设置者
            bool isExitCreateByUser = true;

            Users user = (Users)ServiceBase.GetInfo(ServiceBase.USERINFO);
            if (user.Us_type == 1)
                return true;

            //设置者id
            string createByUserId = "";

            if (ds.Rows.Count > 0)
            {
                createByUserId = ds.Rows[0]["createByUserId"].ToString();
                //不存在设置者，可能直接从用户组获得权限，跳过下面的步骤
                if (createByUserId == "")
                    return true;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Us_id", createByUserId);
                //取到设置者
                DataTable CreatUserDs = DataTrim.DataTableTrim(DataBll.GetList("", "t_Users", dic).Tables[0]);
                //存在设置者
                if (CreatUserDs.Rows.Count > 0)
                {
                    string userType = CreatUserDs.Rows[0]["Us_type"].ToString();
                    if (userType != "1")
                        isSuperManager = false;
                }
                else
                {
                    isExitCreateByUser = false;
                }
            }
            if (!isSuperManager)
            {
                //设置者不是超级管理员
                if (isExitCreateByUser)
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Us_id", createByUserId);
                    //设置者权限
                    DataTable CreaterPermisson = DataTrim.DataTableTrim(DataBll.GetList("", "t_UserFunction", dict).Tables[0]);
                    try
                    {
                        using (var tran = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                        {
                            //存在设置者
                            foreach (DataRow dr in ds.Rows)
                            {
                                DataRow[] dtr = CreaterPermisson.Select("Func_id='" + dr["Func_id"].ToString().Trim() + "'");
                                //设置者存在该权限
                                if (dtr.Count() > 0)
                                {
                                    //设置者该权限等级，1允许访问 2身份验证 3 上级授权
                                    int grade = Convert.ToInt32(dtr[0]["Func_grade"].ToString().Trim());
                                    int currentGrade = Convert.ToInt32(dr["Func_grade"].ToString().Trim());
                                    //登陆者权限等级比设置者的权限等级高，权限降级
                                    if (currentGrade < grade)
                                    {
                                        //更新条件
                                        Dictionary<string, object> dicw = new Dictionary<string, object>();
                                        dicw.Add("Uf_id", dr["Uf_id"].ToString());
                                        dicw.Add("Func_grade", grade);

                                        //更新内容
                                        Dictionary<string, object> di = new Dictionary<string, object>();
                                        di.Add("Func_grade", grade);
                                        DataBll.Update(di, "t_UserFunction", dicw, "Uf_id");
                                    }
                                }
                                else  //设置者不存在该权限,删除该权限
                                {
                                    Dictionary<string, object> dicw = new Dictionary<string, object>();
                                    dicw.Add("Uf_id", dr["Uf_id"].ToString());
                                    DataBll.Delete(dicw, "t_UserFunction");

                                }
                            }
                            tran.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.GetLogger("PermissionManageService").Error("CheckCurrentUserIsConflict 发生异常", ex);
                    }

                }
                else
                {
                    //设置者已被删除，删除权限
                    using (var tran = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                    {
                        try
                        {
                            foreach (DataRow dr in ds.Rows)
                            {
                                Dictionary<string, object> dicw = new Dictionary<string, object>();
                                dicw.Add("Uf_id", dr["Uf_id"].ToString());
                                DataBll.Delete(dicw, "t_UserFunction");
                            }
                            tran.Complete();
                        }
                        catch (Exception ex)
                        {
                            Logger.GetLogger("PermissionManageService").Error("CheckCurrentUserIsConflict 发生异常", ex);
                        }
                    }

                }
            }
            return true;
        }

        /// <summary>
        /// 保存用户数据权限
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userId"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public static OperationResult SaveDataUserPermission(string data, string userId, string unitId)
        {
            try
            {
                PublicHelper.CheckArgument(userId, "userId");
                PublicHelper.CheckArgument(unitId, "unitId");
                PublicHelper.CheckArgument(data, "data");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("PermissionManage").Error(" SaveDataUserPermission userId,unitId,data 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult op = new OperationResult(OperationResultType.Success);
            op.ResultType = OperationResultType.Success;
            op.Message = "保存权限成功";

            List<DataUserPermissionModel> list = JsonUtil.ConvertToObject<List<DataUserPermissionModel>>(data);


            try
            {
                using (var tran = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {


                    //该用户是否已经存在权限
                    int count = DataBll.GetCount("UserCategory", "Us_id='" + userId + "'");
                    if (count > 0)
                    {

                        Dictionary<string, object> dicw = new Dictionary<string, object>();
                        dicw.Add("Us_id", userId);
                        DataBll.Delete(dicw, "UserCategory");

                    }
#if UNITTEST
                    string CurrentUserId = "744bd8bb-126c-45a4-91ca-79b2ce940bd0";
#else
                    string CurrentUserId = ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim();
#endif

                    foreach (DataUserPermissionModel item in list)
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();


                        dic.Add("Us_id", userId);

                        dic.Add("Uc_isSonCate", item.Uc_isSonCate == "undefined" ? "false" : item.Uc_isSonCate);
                        dic.Add("Uc_isChangeFiles", item.Uc_isChangeFiles);
                        dic.Add("Uc_isViewFiles", item.Uc_isViewFiles);
                        dic.Add("Cate_id", item.Cate_id);
                        dic.Add("UnitID", unitId);


                        Dictionary<string, object> preDicw = new Dictionary<string, object>();
                        preDicw.Add("Uc_id", Guid.NewGuid().ToString());

                        //先预插入数据
                        DataSet reDs = DataBll.Add(preDicw, "UserCategory");
                        Dictionary<string, object> wdic = new Dictionary<string, object>();
                        wdic.Add("Uc_id", reDs.Tables[0].Rows[0]["Uc_id"].ToString());

                        DataBll.Update(dic, "UserCategory", wdic, "Uc_id");
                    }
                    tran.Complete();
                }

            }
            catch (Exception ex)
            {
                Logger.GetLogger("EFileLendManageService").Error("Approve发生异常", ex);
                op.Message = "操作发生异常";
                op.ResultType = OperationResultType.Error;
            }
            return op;
        }


        /// <summary>
        /// 为加入用户组的用户设置用户组拥有的权限
        /// </summary>
        /// <param name="addUserIds"></param>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public static bool SetUserPermissionByAddUserToUserGroup(string[] addUserIds, string userGroupId)
        {

            DataTable ds = DataTrim.DataTableTrim(DataBll.Query("select * from t_UserGroupFunction where Ug_id='" + userGroupId + "'").Tables[0]);
            if (ds.Rows.Count > 0)
            {
                string sql = "";
                foreach (var item in addUserIds)
                {

                    foreach (DataRow dr in ds.Rows)
                    {
                        sql += "Insert into t_UserFunction(Us_id,Func_id,Func_grade,Full_url) values('" + item + "','" + dr["Func_id"] + "'," + dr["Func_grade"] + ",'" + dr["Full_url"] + "');";
                    }
                }
                if (sql.Length > 1)
                    DataBll.Query(sql);
            }
            return true;
        }

        /// <summary>
        /// 记录上级授权记录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="url"></param>
        /// <param name="ds"></param>
        public static void SetAuthoritiesHigherRecords(string username, string url, DataSet ds)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Aid", Guid.NewGuid().ToString().ToString());
            dic.Add("AUs_id", ds.Tables[0].Rows[0]["Us_id"].ToString().Trim());
            dic.Add("AUser_name", username.Trim());
            dic.Add("AUnit_name", ds.Tables[0].Rows[0]["Unit_name"].ToString().Trim());
            dic.Add("AUnit_id", ds.Tables[0].Rows[0]["UnitID"].ToString().Trim());
            dic.Add("AUser_type", Convert.ToInt32(ds.Tables[0].Rows[0]["Us_type"].ToString().Trim()));
            dic.Add("Au_time", DateTime.Now);
            dic.Add("Us_id", ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim());
            dic.Add("Us_name", ServiceBase.GetInfo(ServiceBase.USERNAME).ToString().Trim());
            dic.Add("Url", url);
            dic.Add("Unit_id", ServiceBase.GetInfo(ServiceBase.UNITID).ToString().Trim());
           // dic.Add("Unit_name", ServiceBase.GetInfo(ServiceBase.UNITNAME).ToString().Trim());
            dic.Add("User_type", ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString().Trim());

            Dictionary<string, object> dicw = new Dictionary<string, object>();
            dicw.Add("Func_urlPath", url);
            dicw.Add("Func_type", 1);

            DataTable dt = DataTrim.DataTableTrim(DataBll.GetModel("", "t_SystemFunction", dicw).Tables[0]);
            if (dt.Rows.Count > 0)
            {
                dic.Add("Func_full_name", dt.Rows[0]["Func_full_name"]);
            }
            DataBll.Add(dic, "t_AuthoritiesHigherRecords");

            UserLogService.InsertLog(ServiceBase.GetInfo(ServiceBase.USERNAME).ToString(), ServiceBase.GetInfo(ServiceBase.USERFULLNAME).ToString(), ServiceBase.GetIPAddress(), DateTime.Now, "系统设置—操作日志-上级授权","授权成功", "");
        }
    }
}

