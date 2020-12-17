using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GraphicsEvaluatePlatform.WebPlatform.Controllers.WebAPI
{
    public class AdminAPIController : ApiController
    {

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string LoadUserInfo()
        {
            Users us;
            if (ServiceBase.GetInfo(ServiceBase.USERINFO) != null)
                us = (Users)ServiceBase.GetInfo(ServiceBase.USERINFO);
            else
                us = null;

            var ret = new
            {
                Success = us != null,
                Message = "获取" + (us != null ? "成功" : "失败"),
                Data = us != null ? us : new Users()
            };
            return JsonUtil.ToJson(ret, "yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public string UserLogin(LoginModel model)
        {
            try
            {
                PublicHelper.CheckArgument(model, "model");
            }
            catch
            {
                var data = new
                {
                    Success = false,
                    Message = "输入参数错误",
                    Address = ""
                };
                return JsonUtil.ToJson(data);
            }

            try
            {
                if (HttpContext.Current.Session["ValidateCode"] == null || model.ValidateCode == null)
                {
                    var data = new
                    {
                        Success = false,
                        Message = "验证码错误",
                        Address = ""
                    };
                    return JsonUtil.ToJson(data);
                }
                string validatecode = HttpContext.Current.Session["ValidateCode"].ToString();
#if SELFDEBUG
                //if (model.ValidateCode.Trim() != validatecode && model.ValidateCode != "1111")
                if (model.ValidateCode.Trim() != validatecode)
                {
                    var data = new
                    {
                        Success = false,
                        Message = "验证码错误",
                        Address = ""
                    };
                    return JsonUtil.ToJson(data);
                }
#else
                if (model.ValidateCode.Trim() != validatecode)
                {
                    var data = new
                    {
                        Success = false,
                        Message = "验证码错误",
                        Address = ""
                    };
                    return JsonUtil.ToJson(data);
                }
#endif

                OperationResult result = AdminService.UserLogin(model);
                string msg = result.Message ?? result.ResultType.ToDescription();
                if (result.ResultType == OperationResultType.Success)
                {
                    var data = new
                    {
                        Success = true,
                        Message = msg,
                        Address = "/Admin/Index"
                    };

                    HttpCookie aCookie = new HttpCookie("LoginName");
                    aCookie.Value = model.LoginName;
                    aCookie.Expires = DateTime.Now.AddDays(1);
                    HttpContext.Current.Request.Cookies.Add(aCookie);

                    return JsonUtil.ToJson(data);
                }
                else
                {
                    var data = new
                    {
                        Success = false,
                        Message = msg,
                        Address = ""
                    };
                    return JsonUtil.ToJson(data);
                }
            }
            catch (Exception e)
            {
                var data = new
                {
                    Success = false,
                    Message = e.Message,
                    Address = ""
                };
                return JsonUtil.ToJson(data);
            }
        }
        [HttpGet]
        /// <summary>
        /// 用户登出
        /// </summary>
        /// <returns></returns>
        public string UserLogout()
        {
            try
            {
                HttpContext.Current.Session.Clear();
                var data = new
                {
                    Success = true,
                    Message = "登出成功"
                };
                return JsonUtil.ToJson(data);
            }
            catch (Exception ex)
            {
                Logger.GetLogger("AdminController").Error("UserLogout 发生异常", ex);
                var data = new
                {
                    Success = false,
                    Message = "登出失败"
                };
                return JsonUtil.ToJson(data);
            }
        }

        /// <summary>
        /// 超时的登录
        /// </summary>
        /// <param name="model">前台传回来的值</param>
        /// <returns></returns>
        //public JsonResult TimeOutLogin(LoginModel model)
        //{
        //    PublicHelper.CheckArgument(model, "model");
        //    string loginname = Request.Cookies["LoginName"].Value;
        //    model.LoginName = loginname;
        //    OperationResult result = AdminService.UserLogin(model);
        //    var json = new MessageModel(result.Message, result, "");
        //    return Json(json, JsonRequestBehavior.AllowGet);

        //}

        public string UpdateUserInfo(Users us)
        {
            var str = JsonUtil.ToJson(us);
            Dictionary<string, object> user = JsonUtil.ConvertToObject<Dictionary<string, object>>(str);
            var ret = UserService.UpdateUser(user);
            var retdata = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message
            };
            return JsonUtil.ToJson(retdata, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="kid"></param>
        /// <param name="OriPwd"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPut]
        public string ChangePassword(dynamic json)
        {            
            Dictionary<string, object> dr = JsonUtil.ConvertToObject<Dictionary<string, object>>(JsonUtil.ToJson(json));
            string kid = ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim();
            string OriPwd = dr["old_password"].ToString().Trim();
            string pwd = dr["new_password"].ToString().Trim();            
            OperationResult ret = AdminService.ChangePassword(kid, OriPwd, pwd);

            if (ret.ResultType == OperationResultType.Success)
            {
                var data = new
                {
                    Success = true,
                    Message = "修改密码成功!"
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
    }
}