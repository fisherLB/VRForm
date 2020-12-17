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
    [PermissionFilterAPI]
    public class ProjectManagementAPIController : ApiController
    {
        /// <summary>
        /// 获取项目数据列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [HttpGet]
        [SkipPermission]
        [Route("api/ProjectManagement/GetProjectsList")]
        public string GetProjectsList([FromUri]BootstrapPager pager)
        {
            if (pager.filter == null)
                pager.filter = "";
            OperationResult ret = ProjectManagementService.GetList(pager);
            string retJson = "";
            retJson = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
            return retJson;
        }


        /// <summary>
        /// 新增项目
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ProjectManagement/AddProject")]
        public string AddProject(dynamic json)
        {
            string wtf = json.ToString();
            wtf = wtf.Split('&')[0];
            Dictionary<string, object> dr = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);

            OperationResult ret = ProjectManagementService.Add(dr);
            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message
            };
            return JsonUtil.ToJson(data);
        }

        /// <summary>
        /// 编辑项目信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/ProjectManagement/Edit")]
        public string Edit(dynamic json)
        {
            string wtf = json.ToString();
            wtf = wtf.Split('&')[0];
            Dictionary<string, object> dr = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);

            OperationResult ret = ProjectManagementService.Update(dr);
            if (ret.ResultType == OperationResultType.Success)
            {
                var data = new
                {
                    Success = true,
                    Message = "修改项目成功!"
                };
                return JsonUtil.ToJson(data);
            }
            else
            {
                var data = new
                {
                    Success = false,
                    Message = "修改项目失败!"
                };
                return JsonUtil.ToJson(data);
            }
        }

        /// <summary>
        /// 删除项目信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/ProjectManagement/Delete")]
        public string Delete([FromBody]string ids)
        {
            var sbids = ids.ToString();
            sbids = sbids.Split('&')[0];
            sbids = sbids.Replace("\"", "");
            OperationResult ret = ProjectManagementService.Delete(sbids);
            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message,
                Data = ret.AppendData
            };
            return JsonUtil.ToJson(data);
        }
       
        /// <summary>
        /// 启禁项目
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/ProjectManagement/ActiveProjects")]
        public string ActiveProjects(Microsoft.Owin.FormCollection collection)
        {
            string ids = collection["ids"];
            string values = collection["values"];
            OperationResult ret = new OperationResult(OperationResultType.Success);
            ret = ProjectManagementService.ActiveProjects(ids, values);
            if (ret.ResultType == 0)
            {
                return JsonUtil.ToJson(new { Success = true, Message = "操作成功" }, "yyyy-MM-dd");
            }
            else
            {
                return JsonUtil.ToJson(new { Success = false, Message = ret.Message }, "yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 获取机构下的项目
        /// </summary>
        /// <param name="unitId">单位id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ProjectManagement/getProgectOfUnit")]
        public string getProgectOfUnit(dynamic unitId)
        {
            string wtf = unitId.ToString();
            wtf = wtf.Split('&')[0];
            Dictionary<string, object> dr = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);
            OperationResult ret = ProjectManagementService.getProgectOfUnit(dr["unitId"].ToString().Trim());
            string result = JsonUtil.ToJson(ret.AppendData, "yyyy - MM - dd HH: mm:ss");
            return result;
        }

        /// <summary>
        /// 导出项目列表
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ProjectManagement/ExportProjects")]
        public  HttpResponseMessage ExportProjects([FromUri]string value, string ids, string unitId)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            //string ids = string.Empty;//选中的行记录主键 
            string unitid = string.Empty;//单位id；
            string keyWord = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                keyWord = value;
            }
            //if (!string.IsNullOrEmpty(ids))
            //{
            //    ids = ids;
            //}
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
                ret = ProjectManagementService.Export(filepath, unitid, keyWord, ids, 10000);

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

        /// <summary>
        /// 导出项目列表
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        ///  [Route("api/ProjectManagement/ExportProjects")]
        //public FileResult ExportProjects(string keyWord, string ids, string unitId)
        //{
        //    OperationResult ret = new OperationResult(OperationResultType.Success);

        //    //string value = string.Empty;// 查询条件
        //    //string ids = string.Empty;//选中的行记录主键 
        //    //string unitId = string.Empty;//单位id；
        //    if (!string.IsNullOrEmpty(Request["keyWord"]))
        //    {
        //        keyWord = Request["keyWord"];
        //    }
        //    if (!string.IsNullOrEmpty(Request["ids"]))
        //    {
        //        ids = Request["ids"];
        //    }
        //    if (!string.IsNullOrEmpty(Request["unitId"]))
        //    {
        //        unitId = Request["unitId"];
        //    }
        //    string path = Server.MapPath("~/temps/");
        //    if (System.IO.Directory.Exists(path) == false)//如果不存在就创建file文件夹
        //    {
        //        System.IO.Directory.CreateDirectory(path);
        //    }
        //    string filepath = path + "projects.csv";
        //    string sFileName = string.Empty;
        //    ret = ProjectManagementService.Export(filepath, unitId, keyWord, ids, 10000);
        //    if (ret.ResultType == 0)
        //    {
        //        sFileName = string.Format("项目信息-{0}.csv", DateTime.Now.ToString("yyyyMMdd"));
        //        if (Request.ServerVariables["http_user_agent"].ToString().IndexOf("Firefox") != -1)
        //            sFileName = "=?UTF-8?B?" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sFileName)) + "?=";
        //        else
        //            sFileName = System.Web.HttpUtility.UrlEncode(sFileName, System.Text.Encoding.UTF8);
        //        return File(filepath, "text/csv", sFileName);
        //    }
        //    else
        //    {
        //        return File(ret.Message, "text/plain", "项目信息表导出失败.txt");
        //    }
        //}

    }
}