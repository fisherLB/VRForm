using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
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
    //[PermissionFilter]
    public class AdminController : Controller
    {
        // GET: Admin
        [SkipPermission]
        public ActionResult Index()
        {
            return View();
        }
        [SkipPermission]
        [SkipFilter]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        [SkipPermission]
        public string LoadUserInfo()
        {
            Users us;
            if (ServiceBase.GetInfo(ServiceBase.USERINFO).ToString().Trim() != null)
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
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        /// 
        [SkipPermission]
        [SkipFilter]
        public FileResult GetCodeImage()
        {            
            ValidateCode validateCode = new ValidateCode();
            string code = validateCode.CreateValidateCode(4);
            Session["ValidateCode"] = code;
            byte[] bytes = validateCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SkipPermission]
        [SkipFilter]
        [HttpPost]
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
                if (Session["ValidateCode"] == null || model.ValidateCode == null)
                {
                    var data = new
                    {
                        Success = false,
                        Message = "验证码错误",
                        Address = ""
                    };
                    return JsonUtil.ToJson(data);
                }
                string validatecode = Session["ValidateCode"].ToString();
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
                    Response.Cookies.Add(aCookie);

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

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <returns></returns>
        [SkipPermission]
        [SkipFilter]
        public string UserLogout()
        {
            try
            {
                HttpContext.Session.Clear();
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
        [SkipFilter]
        public JsonResult TimeOutLogin(LoginModel model)
        {
            PublicHelper.CheckArgument(model, "model");
            string loginname = Request.Cookies["LoginName"].Value;
            model.LoginName = loginname;
            OperationResult result = AdminService.UserLogin(model);
            var json = new MessageModel(result.Message, result, "");
            return Json(json, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="kid"></param>
        /// <param name="OriPwd"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [SkipPermission]
        public string ChangePassword(string kid, string OriPwd, string pwd)
        {
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
        [SkipPermission]
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
        [SkipPermission]
        [SkipFilter]
        public string GetUserID()
        {
            return ServiceBase.GetInfo(ServiceBase.USERID).ToString().Trim();
        }

    }
}