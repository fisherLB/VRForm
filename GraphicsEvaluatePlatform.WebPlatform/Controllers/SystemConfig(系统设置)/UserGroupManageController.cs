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
    /// <summary>
    /// 用户组管理
    /// </summary>
    public class UserGroupManageController : Controller
    {
        // GET: UserGroupManage
        public ActionResult Index()
        {
            return View();
        }

        //1、获取用户组信息
        public string GetUserGroupList(BootstrapPager pager)
        {
            if (pager.filter == null)
                pager.filter = "";
            var ret = UserGroupService.GetUserGroupList(pager);
            return JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
        }
        //1、新增用户组
        public string AddUserGroup(string json)
        {

            var group =JsonUtil.ConvertToObject<Dictionary<string, object>>(json);
            OperationResult ret =  UserGroupService.AddUserGroup(group);

            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message
            };
            return JsonUtil.ToJson(data);
        }
        //2、修改用户组
        public string EditUserGroup(string json)
        {
            var userGroup = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);
            OperationResult ret = UserGroupService.EditUserGroup(userGroup);
            return JsonUtil.ToJson(ret.AppendData);
        }

        //3、删除用户组
        public string DeleteUserGroup(string ids)
        {
            OperationResult ret = UserGroupService.DeleteUserGroup(ids);
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
        public string GetUserByUserGroup(string GroupID, string Type, string UnitID)
        {
            OperationResult ret = UserGroupService.GetUserList(GroupID, Type, UnitID);
            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message,
                Data = ret.AppendData
            };
            return JsonUtil.ToJson(data);
        }
        //5、修改组内用户
        public string ChangeGroupUsers(string modelstr)
        {
            ChangeUserGroupViewModel model = JsonUtil.ConvertToObject<ChangeUserGroupViewModel>(modelstr);
            OperationResult ret = UserGroupService.ChangeGroupUsers(model.deleted, model.added, model.groupID);
            return JsonUtil.ToJson(ret.AppendData);
        }

        //6、启用禁用用户组
        public string ChangeUserGroupStatus(string type, string ids)
        {
            return JsonUtil.ToJson(UserGroupService.ChangeUserGroupStatus(type, ids).AppendData, "yyyy-MM-dd HH:mm:ss");
        }
    }
}