using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Infrastructure.Encrypt;
using GraphicsEvaluatePlatform.Infrastructure.Logging;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraphicsEvaluatePlatform.WebPlatform.Filter
{
    /// <summary>
    /// 权限过滤器
    /// </summary>
    public class PermissionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //当action 中贴有该标签则跳过过滤器
            if (filterContext.ActionDescriptor.IsDefined(typeof(SkipFilterAttribute), false))
            {
                return;
            }
            //url已经身份验证、上级授权通过，跳出过滤器
            if ((GetCheckURLIsValidated(filterContext) == "true"))
            {
                return;
            }

            if (filterContext.ActionParameters.ContainsKey("Token") && filterContext.ActionParameters["Token"] != null)
            {
                if (filterContext.HttpContext.Session["LastQueryToken"] != null)
                {
                    var savedToken = filterContext.HttpContext.Session["LastQueryToken"].ToString();
                    var sendToken = filterContext.ActionParameters["Token"].ToString();
                    if (savedToken == sendToken)
                        return;
                }
            }

            if (filterContext.ActionParameters.ContainsKey("pager"))
            {
                if (((BootstrapPager)filterContext.ActionParameters["pager"]).filter != null && ((BootstrapPager)filterContext.ActionParameters["pager"]).filter.Split('&').Length == 2)
                {
                    if (filterContext.HttpContext.Session["LastQueryToken"] != null)
                    {
                        var savedToken = filterContext.HttpContext.Session["LastQueryToken"].ToString();
                        //2017-12-05 修改
                        if (((BootstrapPager)filterContext.ActionParameters["pager"]).filter.Contains('&'))
                        {
                            var sendTokenfilter = ((BootstrapPager)filterContext.ActionParameters["pager"]).filter.Split('&')[1];
                            if (sendTokenfilter.Contains('='))
                            {
                                var sendToken = ((BootstrapPager)filterContext.ActionParameters["pager"]).filter.Split('&')[1].Split('=')[1];
                                if (savedToken == sendToken)
                                    return;
                            }
                        }

                        //var sendToken = ((GridPager)filterContext.ActionParameters["pager"]).filter.Split('&')[1].Split('=')[1];
                        //if (savedToken == sendToken)
                        //    return;
                    }
                }
            }


            string controller = filterContext.RouteData.Values["controller"].ToString(); //控制器名
            string action = filterContext.RouteData.Values["action"].ToString();  //action 方法名


            try
            {
                if (!CheckUserLogin())
                {//检查是否登录
                    if (action.ToLower() == "index")
                    {//如果action为index 则标识为访问页面的形式
                        filterContext.Result = new RedirectResult("/admin/login");
                        return;
                    }
                    RedirectResult("用户没有登录", false, "-99", filterContext);
                    return;
                }
                //当action中贴有该标签则跳过权限验证
                if (filterContext.ActionDescriptor.IsDefined(typeof(SkipPermissionAttribute), false))
                {
                    base.OnActionExecuting(filterContext);
                    return;
                }


                string url = "/" + controller + "/" + action;   //完整路径

                string checkType = GetCheckType(filterContext);//获取处理方式

                MessageModel msg = CheckActionPermission(url);//权限验证

                if (!msg.Success)
                {
                    //如果用户没有权限的处理方式
                    if (msg.Data.ToString() == "0")
                    {
                        NoPermission(filterContext);
                    }
                    else
                    {
                        HavePermission(filterContext, msg, url);
                    }

                }
            }
            catch (Exception ex)
            {
                RedirectResult("操作异常，请联系管理员！", false, "010101", filterContext);
                Logger.GetLogger("PermissionFilterAttribute").Error("过滤器发生异常", ex);
            }

            base.OnActionExecuting(filterContext);
        }


        /// <summary>
        /// 没有权限时的处理方式
        /// </summary>
        /// <param name="filterContext">过滤器上下文</param>
        private void NoPermission(ActionExecutingContext filterContext)
        {
            var json = new { Success = false, Data = "-88", Message = "用户没有权限" };
            JsonResult result = new JsonResult();
            result.Data = json;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            filterContext.Result = result;
        }


        /// <summary>
        /// 有权限时处理方式
        /// </summary>
        /// <param name="filterContext">过滤器上下文</param>
        /// <param name="msg">消息</param>
        /// <param name="url">路径</param>
        private void HavePermission(ActionExecutingContext filterContext, MessageModel msg, string url)
        {
            if (msg.Data.ToString().Trim() == "2")
            {//身份验证
                var pwd = filterContext.HttpContext.Request[ServiceBase.FILTER_PWD].Trim();

                Users UserInfo = (Users)ServiceBase.GetInfo(ServiceBase.USERINFO);
                string password = UserInfo.Us_Password;
                if (password.Trim() != EncryptPasswordFactory.GetEncipher(EncryptType.MD5).EncryptFor(pwd))
                {//当密码不正确时
                    RedirectResult("输入密码出错，请重新输入！", false, "-55", filterContext);
                    return;
                }
                string checkType = GetCheckType(filterContext);//获取处理方式
                if (checkType == "urltype")
                {//当点击菜单时检查这个
                    RedirectResult("", true, "66", filterContext);
                    return;
                }
            }
            else
            {//上级授权
                var username = filterContext.HttpContext.Request[ServiceBase.FILTER_USERNAME].ToString().Trim();
                var pwd = filterContext.HttpContext.Request[ServiceBase.FILTER_PWD].ToString().Trim();
                // var type = filterContext.HttpContext.Request[ServiceBase.CHECK_TYPE].ToString();
                AuthoritiesHigher(filterContext, username, pwd, url);
            }
        }

        /// <summary>
        /// 上级授权
        /// </summary>
        /// <param name="filterContext">过滤器上下文</param>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="url">全路径</param>
        private void AuthoritiesHigher(ActionExecutingContext filterContext, string username, string pwd, string url)
        {
            
            pwd = EncryptPasswordFactory.GetEncipher(EncryptType.MD5).EncryptFor(pwd);

            DataSet ReUser = UserService.GetUserModel(username);
            
            if (DataTrim.DataTableTrim(ReUser.Tables[0]).Rows.Count == 1)
            {
                if (ReUser.Tables[0].Rows[0]["Us_Password"].ToString().Trim() != pwd)
                {//密码不正确
                    RedirectResult("输入密码出错，请重新输入！", false, "-55", filterContext);
                    return;
                }

                //超级管理员
                if (ReUser.Tables[0].Rows[0]["Us_type"].ToString().Trim() == "1")
                {
                    string checkType = GetCheckType(filterContext);//获取处理方式
                    if (checkType == "urltype")
                    {//当检查这个标识时重定向到发送json格式数据
                        RedirectResult("", true, "66", filterContext);
                        PermissionManageService.SetAuthoritiesHigherRecords(username, url, ReUser);
                        return;
                    }

                    //operationAction,提交数据，上级授权为超级管理员，跳出过滤器，进入对应处理方法
                    return;
                }
                DataSet up = PermissionManageService.GetPermissionMdoel(ReUser.Tables[0].Rows[0]["Us_id"].ToString().Trim(), url);



                if (DataTrim.DataTableTrim(up.Tables[0]).Rows.Count != 1)
                {
                    //上级用户权限表中没有权限，

                    RedirectResult("该上级用户没有该权限！", false, "-55", filterContext);
                    return;
                }

                else
                {
                    if (Convert.ToInt32(up.Tables[0].Rows[0]["Func_grade"].ToString().Trim()) == 3)
                    {//上级用户没有该功能的权限
                        RedirectResult("该上级用户没有该权限！", false, "-55", filterContext);
                        return;
                    }
                    else
                    {//上级用户有权限
                        string checkType = GetCheckType(filterContext);//获取处理方式
                        if (checkType == "urltype")
                        {//当检查这个标识时重定向到放松json格式数据
                            RedirectResult("", true, "66", filterContext);
                            PermissionManageService.SetAuthoritiesHigherRecords(username, url, ReUser);
                            return;
                        }
                    }
                }
            }
            else
            {
                RedirectResult("该上级用户账号不存在！", false, "-55", filterContext);
            }
        }


        /// <summary>
        /// 获取处理方式
        /// </summary>
        /// <param name="filterContext">过滤器上下文</param>
        /// <returns></returns>
        private string GetCheckType(ActionExecutingContext filterContext)
        {
            try
            {
                return filterContext.HttpContext.Request[ServiceBase.CHECK_TYPE].ToString().Trim();
            }
            catch (Exception)
            {
                return "";
            }
        }


        /// <summary>
        /// 判断url是否已经身份验证、上级授权通过
        /// </summary>
        /// <param name="filterContext">过滤器上下文</param>
        /// <returns></returns>
        private string GetCheckURLIsValidated(ActionExecutingContext filterContext)
        {
            try
            {
                return filterContext.HttpContext.Request[ServiceBase.URLVALIDATED].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }


        /// <summary>
        /// 重定向结果
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="success">是否成功</param>
        /// <param name="data">数据</param>
        /// <param name="filterContext">过滤器上下文</param>
        private void RedirectResult(string msg, bool success, object data, ActionExecutingContext filterContext)
        {
            var json = new MessageModel(msg, success, data);
            JsonResult result = new JsonResult();
            result.Data = json;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            filterContext.Result = result;
        }


        /// <summary>
        /// 返回检查权限的结果
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private MessageModel CheckActionPermission(string url)
        {
            return PermissionFilterService.CheckActionPermission(url);
        }

        /// <summary>
        /// 判断用户是否登录
        /// </summary>
        /// <returns></returns>
        private bool CheckUserLogin()
        {
            if (ServiceBase.GetInfo(ServiceBase.USERINFO) != null)
                return true;
            else
                return false;
        }
    }
}