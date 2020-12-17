using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Service;
using GraphicsEvaluatePlatform.WebPlatform.Filter;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace GraphicsEvaluatePlatform.WebPlatform.Controllers.WebAPI
{
    public class UserLogAPIController : ApiController
    {
        /// <summary>
        /// 用户操作日志
        /// </summary>
        /// <summary>
        /// 获取用户日志列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionFilterAPI]
        [Route("api/UserLog/GetUserLogList")]

        public string GetUserLogList([FromUri]BootstrapPager pager)
        {
            OperationResult ret = UserLogService.GetUserLogList(pager);
            return JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
        }

        #region 导出模块
        //[HttpGet]
        //public HttpResponseMessage ExportUserLog(FormCollection collection)
        //{
        //    OperationResult ret = new OperationResult(OperationResultType.Success);
        //    string value = string.Empty;
        //    string ids = string.Empty;
        //    string unitId = string.Empty;
        //    if (!string.IsNullOrEmpty(Request["value"]))
        //    {
        //        value = Request["value"];
        //    }
        //    if (!string.IsNullOrEmpty(Request["unitId"]))
        //    {
        //        unitId = Request["unitId"];
        //    }
        //    if (!string.IsNullOrEmpty(Request["ids"]))
        //    {
        //        ids = Request["ids"];
        //    }
        //    // string path = Server.MapPath("~/temps/");
        //    string path = HttpContext.Current.Server.MapPath("~/temps/");
        //    if (System.IO.Directory.Exists(path) == false)//如果不存在就创建file文件夹
        //    {
        //        System.IO.Directory.CreateDirectory(path);
        //    }
        //    string filepath = path + "UserLog.csv";
        //    string sFileName = string.Empty;
        //    ret = UserLogService.WriteFileStream(filepath, unitId, value, ids, 10000);
        //    if (ret.ResultType == 0)
        //    {
        //        sFileName = string.Format("用户日志-{0}.csv", DateTime.Now.ToString("yyyyMMdd"));
        //        if (Request.ServerVariables["http_user_agent"].ToString().IndexOf("Firefox") != -1)
        //            sFileName = "=?UTF-8?B?" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sFileName)) + "?=";
        //        else
        //            sFileName = System.Web.HttpUtility.UrlEncode(sFileName, System.Text.Encoding.UTF8);
        //        return File(filepath, "text/csv", sFileName);
        //    }
        //    else
        //    {
        //        return File(ret.Message, "text/plain", "用户日志导出失败.txt");
        //    }
        //}

        /// <summary>
        /// 导出用户日志列表
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/UserLog/ExportUserLog")]
        public HttpResponseMessage ExportUserLog([FromUri]string value, string ids, string unitId)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
           // string ids = string.Empty;//选中的行记录主键 
            string unitid = string.Empty;//单位id；
            string keyWord = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                keyWord = value;
            }
            if (!string.IsNullOrEmpty(ids))
            {
                ids = ids;
            }
            if (!string.IsNullOrEmpty(unitId))
            {
                unitid = unitId;
            }
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/temps/");
            if (System.IO.Directory.Exists(path) == false)//如果不存在就创建file文件夹
            {
                System.IO.Directory.CreateDirectory(path);
            }
            try
            {
                string filepath = path + "projects.csv";
                string sFileName = string.Empty;
                ret = UserLogService.WriteFileStream(filepath, unitId, value, ids, 10000);;

                sFileName = string.Format("用户日志-{0}.csv", DateTime.Now.ToString("yyyyMMdd"));
                if (HttpContext.Current.Request.ServerVariables["http_user_agent"].ToString().IndexOf("Firefox") != -1)
                    sFileName = "=?UTF-8?B?" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sFileName)) + "?=";
                else
                    sFileName = System.Web.HttpUtility.UrlEncode(sFileName, System.Text.Encoding.UTF8);
                if (File.Exists(filepath))
                {
                    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                    var stream = new FileStream(filepath, FileMode.Open);
                    result.Content = new StreamContent(stream);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = sFileName;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
        #endregion
    }
}