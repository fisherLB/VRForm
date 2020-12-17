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
    public class UserManageAPIController : ApiController
    {
        /// <summary>
        /// 获取用户数据列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/UserManage/GetUserList")]
        [SkipPermission]
        public string GetUserList([FromUri] BootstrapPager pager)
        {
            if (string.IsNullOrEmpty(pager.filter))
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
        [HttpPost]
        [Route("api/UserManage/AddUser")]
        public string AddUser(dynamic json)
        {
            string wtf = json.ToString();
            wtf = wtf.Split('&')[0];
            Dictionary<string, object> user = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);
            
            OperationResult ret = UserService.AddUser(user);
            if (ret.ResultType == OperationResultType.Success)
            {
               var  data = new
                {
                    Success = true,
                    Message = ret.Message
                };
                return JsonUtil.ToJson(data);
            }
            else
            {
               var  data = new
                {
                    Success = false,
                    Message = ret.Message
                };
                return JsonUtil.ToJson(data);
            }
            
        }

        //3、修改用户
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/UserManage/EditUser")]
        public string EditUser(dynamic json)
        {
            string wtf = json.ToString();
            wtf = wtf.Split('&')[0];
            Dictionary<string, object> user = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);

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
                    Message = ret.Message
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
        [HttpDelete]
        [Route("api/UserManage/DeleteUser")]
        public string DeleteUser([FromBody]string ids)
        {
           var sbids = ids.ToString();
            sbids = sbids.Split('&')[0];
            sbids= sbids.Replace("\"", "");
            OperationResult ret = UserService.DeleteUser(sbids);
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
        [HttpPut]
        [Route("api/UserManage/ChangePassword")]
        public string ChangePassword([FromBody] string kid, string pwd)
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
        [HttpPut]
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

        //7、启用用户 
        /// <summary>
        /// 启用用户 或 禁用用户
        /// </summary>
        /// <param name="type">判断是启用还是禁用 true 启用  false 禁用</param>
        /// <param name="ids">条目 ID</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/UserManage/ChangeUserStatus")]
        public string ChangeUserStatus(dynamic ids)
        {
            var sbids = ids.ToString();
            sbids = sbids.Split('&')[0];
            sbids = sbids.Replace("\"", "");
            string[] arrids = sbids.Split('|');
            var idsTr = arrids[0];
            var type = arrids[1];
             return JsonUtil.ToJson(UserService.ChangeUserStatus(type, idsTr).AppendData, "yyyy-MM-dd HH:mm:ss");
        }


        //8、 禁用用户
        /// <summary>
        /// 启用用户 或 禁用用户
        /// </summary>
        /// <param name="type">判断是启用还是禁用 true 启用  false 禁用</param>
        /// <param name="ids">条目 ID</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/UserManage/ForbiddenUserStatus")]
        public string ForbiddenUserStatus(dynamic ids)
        {
            var sbids = ids.ToString();
            sbids = sbids.Split('&')[0];
            sbids = sbids.Replace("\"", "");
            string[] arrids = sbids.Split('|');
            var idsTr = arrids[0];
            var type = arrids[1];
            return JsonUtil.ToJson(UserService.ChangeUserStatus(type, idsTr).AppendData, "yyyy-MM-dd HH:mm:ss");
        }


    }
}