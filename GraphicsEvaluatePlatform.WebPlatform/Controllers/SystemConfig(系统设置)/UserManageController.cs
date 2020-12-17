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
    /// 用户管理
    /// </summary>
    public class UserManageController : Controller
    {
        // GET: UserManage
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取用户数据列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public string GetUserList(BootstrapPager pager)
        {
            if (pager.filter == null)
                pager.filter = "";
            OperationResult ret = UserService.GetUserList(pager);
            string retJson = "";
            retJson = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
            return retJson;
        }

        //2、新增用户
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string AddUser(string json)
        {
            Dictionary<string, object> user = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);

            OperationResult ret = UserService.AddUser(user);

            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message
            };
            return JsonUtil.ToJson(data);
        }

        //3、修改用户
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string EditUser(string json)
        {
            Dictionary<string, object> user = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);

            OperationResult ret = UserService.UpdateUser(user);
            if (ret.ResultType == OperationResultType.Success)
            {
                var data = new
                {
                    Success = true,
                    Message = "修改用户成功!"
                };
                return JsonUtil.ToJson(data);
            }
            else
            {
                var data = new
                {
                    Success = false,
                    Message = "修改用户失败!"
                };
                return JsonUtil.ToJson(data);
            }
        }

        //4、删除用户
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids">条目ID</param>
        /// <returns></returns>
        public string DeleteUser(string ids)
        {
            OperationResult ret = UserService.DeleteUser(ids);
            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message,
                Data = ret.AppendData
            };
            return JsonUtil.ToJson(data);
        }

        //5、修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="kid">单位 ID</param>
        /// <param name="pwd">新密码</param>
        /// <returns></returns>
        public string ChangePassword(string kid, string pwd)
        {
            OperationResult ret = UserService.ChangePassword(kid, pwd);

            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = "修改密码" + (ret.ResultType == OperationResultType.Success ? "成功" : "失败," + ret.Message)
            };

            return JsonUtil.ToJson(data);
        }

        //6、初始化密码
        /// <summary>
        /// 初始化密码
        /// </summary>
        /// <param name="ids">条目 ID</param>
        /// <returns></returns>
        public string InitPassword(string ids)
        {
            var ret = UserService.InitPassword(ids);

            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = "初始化用户密码" + (ret.ResultType == OperationResultType.Success ? "成功" : "失败," + ret.Message)
            };

            return JsonUtil.ToJson(data);
        }

        //7、启用用户 或 禁用用户
        /// <summary>
        /// 启用用户 或 禁用用户
        /// </summary>
        /// <param name="type">判断是启用还是禁用 true 启用  false 禁用</param>
        /// <param name="ids">条目 ID</param>
        /// <returns></returns>
        public string ChangeUserStatus(string type, string ids)
        {
            return JsonUtil.ToJson(UserService.ChangeUserStatus(type, ids).AppendData, "yyyy-MM-dd HH:mm:ss");
        }
    }
}