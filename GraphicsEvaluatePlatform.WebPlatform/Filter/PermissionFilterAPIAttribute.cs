using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Service;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Script.Serialization;

using System.Web.Http.Controllers;
using System.Web.Helpers;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using System.Collections.Generic;
using GraphicsEvaluatePlatform.Infrastructure.Encrypt;
using System.Data;

namespace GraphicsEvaluatePlatform.WebPlatform.Filter
{
    public class PermissionFilterAPIAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext filterContext)
        {

            if (filterContext.ActionDescriptor.GetCustomAttributes<SkipFilterAttribute>().Any())
            {
                return;
            }
            try
            {
                string url = "/" + filterContext.ControllerContext.RouteData.Route.RouteTemplate;
                if (!CheckUserLogin())
                {//检查是否登录
                    RedirectResult("用户没有登录", false, "-99", filterContext);
                    return;
                }
                //当action中贴有该标签则跳过权限验证
                if (filterContext.ActionDescriptor.GetCustomAttributes<SkipPermissionAttribute>().Any())
                {
                    base.OnActionExecuting(filterContext);
                    return;
                }

                MessageModel msg = CheckActionPermission(url);//权限验证

                if (!msg.Success)
                {
                    //如果用户没有权限的处理方式
                    if (msg.Data.ToString().Trim() == "0")
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

            }
        }



        private void NoPermission(HttpActionContext filterContext)
        {
            var json = new { Success = false, Data = "-88", Message = "用户没有权限" };
            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Unauthorized;
            response.Content = new StringContent(Json.Encode(json), Encoding.UTF8, "application/json");
        }


        /// <summary>
        /// 有权限时处理方式
        /// </summary>
        /// <param name="filterContext">过滤器上下文</param>
        /// <param name="msg">消息</param>
        /// <param name="url">路径</param>
        private void HavePermission(HttpActionContext filterContext, MessageModel msg, string url)
        {
            var filter_pwd = "";
            var filter_username = "";
            var task = filterContext.Request.Content.ReadAsStreamAsync();
            var content = string.Empty;
            using (System.IO.Stream sm = task.Result)
            {
                if (sm != null)
                {
                    sm.Seek(0, SeekOrigin.Begin);
                    int len = (int)sm.Length;
                    byte[] inputByts = new byte[len];
                    sm.Read(inputByts, 0, len);
                    sm.Close();
                    content = Encoding.UTF8.GetString(inputByts);
                    content = content.Replace("\"{", "{");
                    content = content.Replace("}\"", "}");
                    content = content.Replace("\\", "");
                }
            }
            if (msg.Data.ToString().Trim() == "2")
            {//身份验证
                var subcontent = content.Split('&');
                filter_pwd = subcontent[1].Split('=')[1];
                filter_pwd = filter_pwd.Substring(0, filter_pwd.Length - 1);
                var pwd = filter_pwd;
                Users UserInfo = (Users)ServiceBase.GetInfo(ServiceBase.USERINFO);
                string password = UserInfo.Us_Password.Trim();
                if (password != EncryptPasswordFactory.GetEncipher(EncryptType.MD5).EncryptFor(pwd).Trim())
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
                var subcontent = content.Split('&');
                filter_pwd = subcontent[1].Split('=')[1];
                filter_username = subcontent[2].Split('=')[1];
                filter_username = subcontent[2].Split('=')[1].Substring(0, filter_username.Length - 1);
                var username = filter_username;
                var pwd = filter_pwd;
                // var type = filterContext.HttpContext.Request[ServiceBase.CHECK_TYPE].ToString();
                AuthoritiesHigher(filterContext, username, pwd, url);
            }
        }

        ///// <summary>
        ///// 上级授权
        ///// </summary>
        ///// <param name="filterContext">过滤器上下文</param>
        ///// <param name="username">用户名</param>
        ///// <param name="pwd">密码</param>
        ///// <param name="url">全路径</param>
        private void AuthoritiesHigher(HttpActionContext filterContext, string username, string pwd, string url)
        {

            pwd = EncryptPasswordFactory.GetEncipher(EncryptType.MD5).EncryptFor(pwd).Trim();

            DataSet ReUser = UserService.GetUserModel(username);
            if (DataTrim.DataTableTrim(ReUser.Tables[0]).Rows.Count == 1)
            {
                if (DataTrim.DataTableTrim(ReUser.Tables[0]).Rows[0]["Us_Password"].ToString().Trim() != pwd.Trim())
                {//密码不正确
                    RedirectResult("输入密码出错，请重新输入！", false, "-55", filterContext);
                    return;
                }

                //超级管理员
                if (ReUser.Tables[0].Rows[0]["Us_type"].ToString() == "1")
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



                if (up.Tables[0].Rows.Count != 1)
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
        private string GetCheckType(HttpActionContext filterContext)
        {
            try
            {
                return "";//filterContext.HttpContext.Request[ServiceBase.CHECK_TYPE].ToString().Trim();
            }
            catch (Exception)
            {
                return "";
            }
        }


        ///// <summary>
        ///// 判断url是否已经身份验证、上级授权通过
        ///// </summary>
        ///// <param name="filterContext">过滤器上下文</param>
        ///// <returns></returns>
        private string GetCheckURLIsValidated(HttpActionContext filterContext)
        {
            try
            {
                return "";
                //return filterContext.RequestContext.Url.Request.[ServiceBase.URLVALIDATED].ToString().Trim();
            }
            catch (Exception)
            {
                return "";
            }
        }


        ///// <summary>
        ///// 重定向结果
        ///// </summary>
        ///// <param name="msg">消息</param>
        ///// <param name="success">是否成功</param>
        ///// <param name="data">数据</param>
        ///// <param name="filterContext">过滤器上下文</param>
        private void RedirectResult(string msg, bool success, object data, HttpActionContext actionContext)
        {
            var json = new MessageModel(msg, success, data);
            var response = actionContext.Response = actionContext.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StringContent(Json.Encode(json), Encoding.UTF8, "application/json");
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
            try
            {
                if (ServiceBase.GetInfo(ServiceBase.USERINFO).ToString().Trim() != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}