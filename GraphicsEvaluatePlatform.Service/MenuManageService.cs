/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEvaluatePlatform.Model;
using System.Data;
using GraphicsEvaluatePlatform.Repository;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Service
 * 项目描述: 
 * 类 名 称: MenuService
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.Service
 * 文件名称: MenuService
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/3 11:34:01
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Service
{
    public static class MenuManageService
    {

        private static List<MenuTree> ListResult { get; set; }

        /// <summary>
        /// 获取所有功能列表(登录成功，将数据存入session)
        /// </summary>
        /// <returns></returns>
        public static List<MenuViewModel> GetAllMenuList()
        {
            List<MenuViewModel> result = new List<MenuViewModel>();
            int index = 1;
            Dictionary<string, object> dicw = new Dictionary<string, object>();
            dicw.Add("Func_parent_id", "is null");

            DataSet ParentMenus = DataBll.Query("select * from dbo.t_SystemFunction where Func_parent_id is null or Func_parent_id=''  order by Func_sequence asc");

            //获取存入内存的权限
            DataSet ds = (DataSet)ServiceBase.GetInfo(ServiceBase.USER_PERMISSION);
            foreach (DataRow item in DataTrim.DataTableTrim(ParentMenus.Tables[0]).Rows)
            {
                MenuViewModel mv = new MenuViewModel();
                mv.menuid = item["Func_id"].ToString();
                mv.menuname = item["Func_name"].ToString();
                mv.icon = item["Func_icon"].ToString();
                mv.children = new List<MenusViewModel>();
                mv.menuorder = Convert.ToInt32(item["Func_sequence"]);
                mv.index = index;
                mv.Action = item["Func_urlPath"].ToString();
                index++;
                Dictionary<string, object> childrenDicw = new Dictionary<string, object>();
                childrenDicw.Add("Func_parent_id", item["Func_id"]);
                childrenDicw.Add("Func_type", 0);
                DataSet childrenMenu = DataBll.GetList("", "t_SystemFunction", childrenDicw, "order by Func_sequence asc");
                //存在子菜单
                if (childrenMenu.Tables.Count > 0)
                {
                    foreach (DataRow critem in DataTrim.DataTableTrim(childrenMenu.Tables[0]).Rows)
                    {
                        //为超级管理员拥有所有菜单
                        if (ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString() == "1")
                        {
                            MenusViewModel defaultMs = new MenusViewModel();
                            defaultMs.id = critem["Func_id"].ToString();
                            defaultMs.text = critem["Func_name"].ToString();
                            defaultMs.attributes = "";
                            defaultMs.menuorder = Convert.ToInt32(item["Func_sequence"]);
                            defaultMs.Action = critem["Func_urlPath"].ToString();
                            mv.children.Add(defaultMs);
                        }
                        else
                        {
                            //不为超级管理员，判断是否存在该菜单的权限，存在添加入菜单列表;
                            if ((ds.Tables[0].Select("Full_url='" + critem["Func_urlPath"].ToString() + "'").Length > 0))
                            {
                                MenusViewModel defaultMs = new MenusViewModel();
                                defaultMs.id = critem["Func_id"].ToString();
                                defaultMs.text = critem["Func_name"].ToString();
                                defaultMs.attributes = "";
                                defaultMs.menuorder = Convert.ToInt32(item["Func_sequence"]);
                                defaultMs.Action = critem["Func_urlPath"].ToString();
                                mv.children.Add(defaultMs);

                            }
                        }
                    }
                }
                result.Add(mv);
            }
            return result;
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult AddMenuInfo(Dictionary<string, object> model)
        {
            try
            {
                PublicHelper.CheckArgument(model, "model");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("MenuManageService").Error("model 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult result = new OperationResult(OperationResultType.Success);

            model.Remove("Func_id");
            model["Func_id"] = Guid.NewGuid().ToString();
            try
            {
                //计算功能菜单深度
                if (model["Func_parent_id"].ToString().Trim() != null && model["Func_parent_id"].ToString().Trim() != "")
                {
                    var dicCondition = new Dictionary<string, object>();
                    dicCondition.Add("Func_id", model["Func_parent_id"].ToString().Trim());
                    var rettarunit = DataBll.GetModel("Depth", "t_SystemFunction", dicCondition);
                    model["Depth"] = int.Parse(rettarunit.Tables[0].Rows[0]["Depth"].ToString().Trim()) + 1;
                }
                else
                {
                    model["Depth"] = 0;
                }

                var ret = DataBll.Add( model, "t_SystemFunction");
                if (ret.Tables[0].Rows.Count>0)
                    result.Message = "添加成功";
                else
                {
                    result.Message = "添加失败";
                    result.ResultType = OperationResultType.Error;
                }

            }
            catch (Exception ex)
            {
                result.ResultType = OperationResultType.Error;
                result.Message = "添加失败";
                Logger.GetLogger("MenuManageService").Error("Message:" + ex.Message + "Inner:" + ex.InnerException);

            }
            return result;
        }



        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult EditMenuInfo(Dictionary<string, object> model)
        {
            try
            {
                PublicHelper.CheckArgument(model, "model");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("MenuManageService").Error("model 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult result = new OperationResult(OperationResultType.Success);

            try
            {
                Dictionary<string, object> dicw = new Dictionary<string, object>();
                dicw.Add("Func_id", model["Func_id"].ToString().Trim());
                if (model["Func_parent_id"].ToString().Trim() == "")
                {
                    model.Remove("Func_parent_id");
                }
                model.Remove("Func_id");

                ////计算功能菜单深度
                //if (model["Func_parent_id"].ToString() != null || model["Func_parent_id"].ToString() != "")
                //{
                //    var dicCondition = new Dictionary<string, object>();
                //    dicCondition.Add("Func_id", model["Func_parent_id"]);
                //    var rettarunit = DataBll.GetModel("Depth", "t_SystemFunction", dicCondition);
                //    model["Depth"] = int.Parse(rettarunit.Tables[0].Rows[0]["Depth"].ToString().Trim()) + 1;
                //}
                //else
                //{
                //    model["Depth"] = 0;
                //}
                bool re = DataBll.Update(model, "t_SystemFunction", dicw, "Func_id");
                if (re)
                    result.Message = "保存成功";
                else
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "保存失败";
                }
            }
            catch (Exception ex)
            {
                result.ResultType = OperationResultType.Error;
                result.Message = "保存失败";
                Logger.GetLogger("MenuManageService").Error("Message:" + ex.Message + "Inner:" + ex.InnerException);

            }
            return result;
        }



        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult DelMenuInfo(Dictionary<string, object> dicw)
        {
            try
            {
                PublicHelper.CheckArgument(dicw, "model");
            }
            catch (Exception ex)
            {
                Logger.GetLogger("MenuManageService").Error("model 传入参数异常", ex);
                return new OperationResult(OperationResultType.ParamError, "参数异常");
            }
            OperationResult result = new OperationResult(OperationResultType.Success);
            try
            {
                int count = DataBll.GetCount("t_SystemFunction", "Func_parent_id='" + dicw["Func_id"] + "'");
                if (count > 0)
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "删除失败,该菜单下存在子菜单，不能删除！";
                }
                else
                {
                    bool re = DataBll.Delete(dicw, "t_SystemFunction");
                    if (re)
                        result.Message = "删除成功";
                    else
                    {
                        result.ResultType = OperationResultType.Error;
                        result.Message = "删除失败";
                    }
                }
            }
            catch (Exception ex)
            {
                result.ResultType = OperationResultType.Error;
                result.Message = "删除失败";
                Logger.GetLogger("MenuManageService").Error("Message:" + ex.Message + "Inner:" + ex.InnerException);
            }
            return result;
        }

        /// <summary>
        /// 子菜单列表(功能管理列表)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult GetMenuList()
        {

            OperationResult result = new OperationResult(OperationResultType.Success);
            try
            {

                var ds = DataBll.Query("select * from dbo.t_SystemFunction order by Func_sequence asc");
                var dsStr = ds.Tables[0];
                //去掉字符串后面的空格
                dsStr = DataTrim.DataTableTrim(dsStr);
                //List<MenuTree> listTree = new List<MenuTree>();
                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    MenuTree model = new MenuTree();
                //    model.Func_id = dr["Func_id"].ToString().Trim();
                //    model.Func_name = dr["Func_name"].ToString().Trim();
                //    model.Func_sequence = dr["Func_sequence"].ToString().Trim();
                //    model.Func_urlPath = dr["Func_urlPath"].ToString().Trim();
                //    model.Func_full_name = dr["Func_full_name"].ToString().Trim();
                //    model.ParentId = dr["Func_parent_id"].ToString().Trim();
                //    model.Func_code = dr["Func_code"].ToString().Trim();
                //    model.Func_type = dr["Func_type"].ToString().Trim();
                //    model.Func_icon = dr["Func_icon"].ToString().Trim();
                //    model.Depth = dr["Depth"].ToString().Trim();
                //    listTree.Add(model);
                //}

                //ListResult = new List<MenuTree>();
                //CreatChildTree(listTree);

                //result.AppendData = new { total = ds.Tables[0].Rows.Count, rows = ListResult };
                result.AppendData = new
                {
                   
                    rows = dsStr
                };
            }
            catch (Exception ex)
            {
                result.ResultType = OperationResultType.Error;
                Logger.GetLogger("MenuManageService").Error("Message:" + ex.Message + "Inner:" + ex.InnerException);
            }
            return result;
        }



        /// <summary>
        /// 子菜单列表(功能管理列表)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult GetMenus()
        {

            OperationResult result = new OperationResult(OperationResultType.Success);
            try
            {

                var ds = DataBll.Query("select * from dbo.t_SystemFunction order by Func_sequence asc");
                var dsStr = ds.Tables[0];
                //去掉字符串后面的空格
                dsStr = DataTrim.DataTableTrim(dsStr);
                List<MenuTree> listTree = new List<MenuTree>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MenuTree model = new MenuTree();
                    model.Func_id = dr["Func_id"].ToString().Trim();
                    model.Func_name = dr["Func_name"].ToString().Trim();
                    model.Func_sequence = dr["Func_sequence"].ToString().Trim();
                    model.Func_urlPath = dr["Func_urlPath"].ToString().Trim();
                    model.Func_full_name = dr["Func_full_name"].ToString().Trim();
                    model.ParentId = dr["Func_parent_id"].ToString().Trim();
                    model.Func_code = dr["Func_code"].ToString().Trim();
                    model.Func_type = dr["Func_type"].ToString().Trim();
                    model.Func_icon = dr["Func_icon"].ToString().Trim();
                    model.Depth = dr["Depth"].ToString().Trim();
                    listTree.Add(model);
                }

                ListResult = new List<MenuTree>();
                CreatChildTree(listTree);

                result.AppendData = new { total = ds.Tables[0].Rows.Count, rows = listTree };
                //result.AppendData = new
                //{

                //    rows = dsStr
                //};
            }
            catch (Exception ex)
            {
                result.ResultType = OperationResultType.Error;
                Logger.GetLogger("MenuManageService").Error("Message:" + ex.Message + "Inner:" + ex.InnerException);
            }
            return result;
        }


        /// <summary>
        /// 获取分类树数据
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private static bool CreatChildTree(List<MenuTree> list)
        {
            var children = list.Where(x => x.ParentId.Trim() == "").ToList();
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
        private static bool BindChildren(List<MenuTree> list, MenuTree model)
        {
            var children = list.Where(x => x.ParentId.Trim() == model.Func_id.Trim()).ToList();
            if (children.Count > 0)
            {
                model.children = children;
                foreach (var item in children)
                {
                    BindChildren(list, item);
                }
            }
           
            return true;
        }

        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <param name="menuList">所有菜单</param>
        /// <param name="parentMenu">父菜单</param>
        /// <returns>List<TreeModel></returns>
        private static List<MenuTree> getChildMenuList(string Func_id)
        {

            List<MenuTree> treeList = new List<MenuTree>();
            Dictionary<string, object> dicw = new Dictionary<string, object>();
            dicw.Add("Func_parent_id", Func_id);
            DataSet ds = DataBll.GetList("", "t_SystemFunction", dicw);
            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MenuTree model = new MenuTree();
                    model.Func_id = dr["Func_id"].ToString().Trim();
                    model.Func_name = dr["Func_name"].ToString().Trim();
                    model.Func_sequence = dr["Func_sequence"].ToString().Trim();
                    model.Func_urlPath = dr["Func_urlPath"].ToString().Trim();
                    model.Func_full_name = dr["Func_full_name"].ToString().Trim();
                    model.Func_code = dr["Func_code"].ToString().Trim();
                    model.Func_type = dr["Func_type"].ToString().Trim();
                    treeList.Add(model);
                }
            }

            return treeList;
        }
    }
}
