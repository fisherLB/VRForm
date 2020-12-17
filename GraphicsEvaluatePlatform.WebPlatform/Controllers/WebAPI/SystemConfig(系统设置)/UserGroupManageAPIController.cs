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

namespace GraphicsEvaluatePlatform.WebPlatform.Controllers.WebAPI
{
    [PermissionFilterAPI]
    public class UserGroupManageAPIController : ApiController
    {
        [HttpGet]
        [Route("api/UserGroupManage/GetUserGroupList")]
        [SkipPermission]
        //1、获取用户组信息
        public string GetUserGroupList([FromUri] BootstrapPager pager)
        {
            if (pager.filter == null)
                pager.filter = "";
            var ret = UserGroupService.GetUserGroupList(pager);
            return JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
        }
        [HttpPost]
        [Route("api/UserGroupManage/AddUserGroup")]
        //1、新增用户组
        public string AddUserGroup(dynamic json)
        {
            string wtf = json.ToString();
            wtf = wtf.Split('&')[0];
            var group = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);
            OperationResult ret = UserGroupService.AddUserGroup(group);

            if (ret.ResultType == OperationResultType.Success)
            {
                var data = new
                {
                    Success = true,
                    Message = ret.Message
                };
                return JsonUtil.ToJson(data);
            }
            else
            {
                var data = new
                {
                    Success = false,
                    Message = ret.Message
                };
                return JsonUtil.ToJson(data);
            }
        }
        [HttpPut]
        [Route("api/UserGroupManage/EditUserGroup")]
        //2、修改用户组
        public string EditUserGroup(dynamic json)
        {
            string wtf = json.ToString();
            wtf = wtf.Split('&')[0];
            var userGroup = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);
            OperationResult ret = UserGroupService.EditUserGroup(userGroup);
            return JsonUtil.ToJson(ret.AppendData);
        }
        [HttpDelete]
        [Route("api/UserGroupManage/DeleteUserGroup")]
        //3、删除用户组
        public string DeleteUserGroup([FromBody] string ids)
        {
            var sbids = ids.ToString();
            sbids = sbids.Split('&')[0];
            sbids = sbids.Replace("\"", "");
            OperationResult ret = UserGroupService.DeleteUserGroup(sbids);
            var returnmessage = "";
            if (ret.ResultType == OperationResultType.IllegalOperation)
            {
                string retstr = ret.AppendData.ToString();
                var retarr = retstr.Split(',');
                foreach (var item in retarr)
                {
                    returnmessage += "用户组:" + item.Split(':')[0] + "内有" + item.Split(':')[1] + "个用户无法删除组,";
                }
                if (returnmessage.Length > 1)
                    returnmessage = returnmessage.Substring(0, returnmessage.Length - 1);

                var data = new
                {
                    Success = ret.ResultType == OperationResultType.Success,
                    Message = returnmessage
                };
                return JsonUtil.ToJson(data);
            }
            return JsonUtil.ToJson(ret.AppendData);
        }

        //4、获取不在用户组内的用户
        [HttpGet]
        [Route("api/UserGroupManage/GetUserByUserGroup")]
        [SkipPermission]
        public string GetUserByUserGroup([FromUri] string GroupID, string Type, string UnitID)
        {
            OperationResult ret = UserGroupService.GetUserList(GroupID, Type, UnitID);
            var data = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
            return data;
        }

        [HttpPut]
        [Route("api/UserGroupManage/ChangeGroupUsers")]
        //5、修改组内用户（为用户组添加用户）
        public string ChangeGroupUsers(dynamic modelstr)
        {
            //string str = modelstr.ToString();
            //modelstr = modelstr.Split('&')[0];
            //modelstr = modelstr.Replace("\\", "");
            //modelstr = modelstr.Submodelstring(1);
            //modelstr = modelstr.Submodelstring(0, modelstr.Length - 1);
            ChangeUserGroupViewModel model = JsonUtil.ConvertToObject<ChangeUserGroupViewModel>(modelstr);
            OperationResult ret = UserGroupService.ChangeGroupUsers(model.deleted, model.added, model.groupID);
            return JsonUtil.ToJson(ret.AppendData);
        }
        [HttpPut]
        //6、启用禁用用户组
        public string ChangeUserGroupStatus(string type, string ids)
        {
            return JsonUtil.ToJson(UserGroupService.ChangeUserGroupStatus(type, ids).AppendData, "yyyy-MM-dd HH:mm:ss");
        }
    }
}