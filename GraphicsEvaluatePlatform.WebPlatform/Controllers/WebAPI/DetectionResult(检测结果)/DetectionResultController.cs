using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Service;
using GraphicsEvaluatePlatform.WebPlatform.Filter;
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
    public class DetectionResultController : ApiController
    {
        /// <summary>
        /// 获取检测结果列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [HttpGet]
        [SkipPermission]
        [Route("api/DetectionResult/GetList")]
        public string GetList([FromUri]BootstrapPager pager)
        {
            if (pager.filter == null)
                pager.filter = "";
            OperationResult ret = DetectionResultService.GetList(pager);
            string retJson = "";
            retJson = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
            return retJson;
        }


        /// <summary>
        /// 导出检测结果列表
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/DetectionResult/ExportDetectionResult")]
        public HttpResponseMessage ExportDetectionResult([FromUri]string value, string ids, string unitId,string projectId)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            string unitid = string.Empty;//单位id；
            string projectid = string.Empty;//项目id
            string keyWord = string.Empty;//查询关键字
            if (!string.IsNullOrEmpty(value))
            {
                keyWord = value;
            }
            if (!string.IsNullOrEmpty(projectId))
            {
                projectid = projectId;
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
                ret = DetectionResultService.Export(filepath, unitid,projectid, keyWord, ids, 10000);

                sFileName = string.Format("项目信息-{0}.csv", DateTime.Now.ToString("yyyyMMdd"));
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

    }
}