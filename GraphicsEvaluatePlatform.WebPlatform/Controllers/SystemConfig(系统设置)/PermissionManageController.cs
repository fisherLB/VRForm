using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Service;
using GraphicsEvaluatePlatform.WebPlatform.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraphicsEvaluatePlatform.WebPlatform.Controllers
{
    [PermissionFilter]
    //权限管理
    public class PermissionManageController : Controller
    {
        // GET: PermissionManage
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取action权限级别
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [SkipPermission]
        public JsonResult GetUserActionGrade(string url)
        {
            int grade = PermissionManageService.GetCurrentUserPermissionGrade(url);
            var json = new MessageModel("", true, grade); ;
            if (grade == 0)
            {
                json = new MessageModel("", false, grade); ;
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [SkipPermission]
        public string GetUserList(BootstrapPager pager)
        {
            if (pager.filter == null)
                pager.filter = "";
            OperationResult ret = PermissionManageService.GetUserList(pager);
            string retJson = "";
            retJson = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd hh:mm:ss");
            return retJson;
        }
        /// <summary>
        /// 获取用户组权限列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [SkipPermission]
        public string GetUserGroupList(BootstrapPager pager)
        {
            if (pager.filter == null)
                pager.filter = "";
            var ret = UserGroupPermissionService.GetUserGroupList(pager);
            return JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd");
        }
        [SkipPermission]
        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public string GetUserPermission(BootstrapPager pager)
        {
            string userId = pager.filter;
            OperationResult ret = PermissionManageService.GetUserPermission(userId);
            string retJson = "";
            retJson = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd hh:mm:ss");
            return retJson;
        }
        [SkipPermission]
        /// <summary>
        /// 获取session中的权限
        /// </summary>
        /// <returns></returns>
        public string GetUserPermissionBySession()
        {
            MessageModel retJson = new MessageModel("", true, PermissionManageService.GetCurrentUserPermissionBySession());
            string UserType = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString();
            if (UserType == "1")
            {
                retJson.Message = "当前登录用户为超级管理员";
            }
            return JsonUtil.ToJson(retJson, "yyyy-MM-dd hh:mm:ss");
        }
        [SkipPermission]
        /// <summary>
        /// 获取用户组权限
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public string GetUserGroupPermission(GridPager pager)
        {
            string userGroupId = pager.filter;
            OperationResult ret = UserGroupPermissionService.GetUserGroupPermission(userGroupId);
            string retJson = "";
            retJson = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd hh:mm:ss");
            return retJson;
        }

        #region  保存用户、用户组权限
        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="type">调用的方法</param>
        /// <param name="userId">用户id</param>
        /// <param name="userGroupId">用户组id</param>
        /// <returns></returns>
        public string SavePermission(string data, string type, string userId = "", string userGroupId = "")
        {
            if (type == "user")
            {
                return SaveUserPermission(data, userId);
            }
            else
            {
                return SaveUserGroupPermission(data, userGroupId);
            }
        }


        [SkipPermission]
        /// <summary>
        /// 保存用户权限
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string SaveUserPermission(string data, string userId)
        {
            OperationResult result = PermissionManageService.SaveUserPermission(data, userId);
            MessageModel re = new MessageModel(result);
            return JsonUtil.ToJson(re, "yyyy-MM-dd hh:mm:ss");
        }
        [SkipPermission]
        /// <summary>
        /// 保存用户组权限
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public string SaveUserGroupPermission(string data, string userGroupId)
        {
            OperationResult result = UserGroupPermissionService.SaveUserGroupPermission(data, userGroupId);
            MessageModel re = new MessageModel(result);
            return JsonUtil.ToJson(re, "yyyy-MM-dd hh:mm:ss");
        }

        #endregion
        /// <summary>
        /// 获取机构树
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        //[SkipPermission]
        //public string GetUnitCombobox(int? type = 0)
        //{
        //    var ret = UnitManageService.GetUnitComboTree();
        //    if (type != 1)
        //        ((List<ComboTree>)ret.AppendData).Insert(0, new ComboTree { id = -1, text = "全部" });
        //    return JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
        //}
    }
}