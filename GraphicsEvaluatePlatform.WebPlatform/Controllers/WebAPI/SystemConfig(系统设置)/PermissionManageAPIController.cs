using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Service;
using GraphicsEvaluatePlatform.WebPlatform.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace GraphicsEvaluatePlatform.WebPlatform.Controllers.WebAPI
{
    [PermissionFilterAPI]
    public class PermissionManageAPIController : ApiController
    {
        /// <summary>
        /// 获取action权限级别
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [SkipPermission]
        [HttpGet]
        [Route("api/PermissionManage/GetUserActionGrade")]
        public string GetUserActionGrade(string url)
        {
            int grade = PermissionManageService.GetCurrentUserPermissionGrade(url);
            var json = new MessageModel("", true, grade) ;
            if (grade == 0)
            {
                json = new MessageModel("", false, grade) ;
            }
            return JsonUtil.ToJson(json);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [SkipPermission]
        [HttpGet]
        [Route("api/PermissionManage/GetUserList")]
        public string GetUserList([FromUri]BootstrapPager pager)
        {
            if (pager.filter == null)
                pager.filter = "";
            OperationResult ret = PermissionManageService.GetUserList(pager);
            string retJson = "";
            retJson = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd hh:mm:ss");
            return retJson;
        }
        /// <summary>
        /// 获取用户组列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [SkipPermission]
        [HttpGet]
        [Route("api/PermissionManage/GetUserGroupList")]
        public string GetUserGroupList([FromUri]BootstrapPager pager)
        {
            if (pager.filter == null)
                pager.filter = "";
            var ret = UserGroupPermissionService.GetUserGroupList(pager);
            return JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd");
        }

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [SkipPermission]
        [HttpGet]
        [Route("api/PermissionManage/GetUserPermission")]
        public string GetUserPermission([FromUri]BootstrapPager pager)
        {
            string userId = pager.filter;
            OperationResult ret = PermissionManageService.GetUserPermission(userId);
            string retJson = "";
            retJson = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd hh:mm:ss");
            return retJson;
        }

        /// <summary>
        /// 获取session中的权限
        /// </summary>
        /// <returns></returns>
        [SkipPermission]
        [HttpGet]
        [Route("api/PermissionManage/GetUserPermissionBySession")]
        public string GetUserPermissionBySession()
        {
            MessageModel retJson = new MessageModel("", true, PermissionManageService.GetCurrentUserPermissionBySession());
            string UserType = ServiceBase.GetInfo(ServiceBase.USERTYPE).ToString().Trim();
            if (UserType == "1")
            {
                retJson.Message = "当前登录用户为超级管理员";
            }
            return JsonUtil.ToJson(retJson, "yyyy-MM-dd hh:mm:ss");
        }

        /// <summary>
        /// 获取用户组权限
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [SkipPermission]
        [HttpGet]
        [Route("api/PermissionManage/GetUserGroupPermission")]
        public string GetUserGroupPermission([FromUri]BootstrapPager pager)
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
        //[HttpPut]
        //[Route("api/PermissionManage/SavePermission")]
        //public string SavePermission(dynamic json)
        //{
        //    string[] arrids = json.Split('&');
        //    string data = arrids[0].Trim();
        //    string userId = arrids[1].Trim();
        //    string type = arrids[2].Trim();
        //    //string data = "", type = "", userId = "", userGroupId = "";
        //    if (type == "user")
        //    {
        //        return SaveUserPermission(data, userId);
        //    }
        //    else
        //    {
        //        string userGroupId = userId;
        //        return SaveUserGroupPermission(data, userGroupId);
        //    }
        //}



        /// <summary>
        /// 保存用户权限
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/PermissionManage/SaveUserPermission")]
        public string SaveUserPermission(dynamic json)
        {
            string wtf = json.ToString();
            //wtf = wtf.Split('&')[0];
            wtf = wtf.Replace("\"{", "{");
            wtf = wtf.Replace("}\"", "}");
            wtf = wtf.Replace("\\", "");
            string[] arrids = wtf.Split('&');
            string data = arrids[0].Trim();
            data =  data.Substring(1);
            string userId = arrids[1].Trim();
            userId = userId.Replace("\"","");
            OperationResult result = PermissionManageService.SaveUserPermission(data, userId);
            MessageModel re = new MessageModel(result);
            return JsonUtil.ToJson(re, "yyyy-MM-dd hh:mm:ss");
        }

        /// <summary>
        /// 保存用户组权限
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/PermissionManage/SaveUserGroupPermission")]
        public string SaveUserGroupPermission(dynamic json)
        {
            string wtf = json;
            //wtf = wtf.Split('&')[0];
            wtf = wtf.Replace("\"{", "{");
            wtf = wtf.Replace("}\"", "}");
            wtf = wtf.Replace("\\", "");
            string[] arrids = wtf.Split('&');
            string data = arrids[0].Trim();
            data = data.Substring(1);
            string userGroupId = arrids[1].Trim();
            userGroupId=userGroupId.Replace("\"", "");

            OperationResult result = UserGroupPermissionService.SaveUserGroupPermission(data, userGroupId);
            MessageModel re = new MessageModel(result);
            return JsonUtil.ToJson(re, "yyyy-MM-dd hh:mm:ss");
        }

        #endregion
       
    }
}