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
    public class ProjectManagementController : Controller
    {
       /// <summary>
       /// 显示主页
       /// </summary>
       /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 新增项目
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string AddProject(string json)
        {
            Dictionary<string, object> pro = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);

            OperationResult ret = ProjectManagementService.Add(pro);
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
        public string Edit(string json)
        {
            Dictionary<string, object> pro = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);

            OperationResult ret = ProjectManagementService.Update(pro);
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
        public string Delete(string ids)
        {
            OperationResult ret = ProjectManagementService.Delete(ids);
            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message,
                Data = ret.AppendData
            };
            return JsonUtil.ToJson(data);
        }
        [SkipPermission]
        /// <summary>
        /// 获取项目数据列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public string GetProjectsList(BootstrapPager pager)
        {
            if (pager.filter == null)
                pager.filter = "";
            OperationResult ret = ProjectManagementService.GetList(pager);
            string retJson = "";
            retJson = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
            return retJson;
        }
        /// <summary>
        /// 启禁项目
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public string ActiveProjects(FormCollection collection)
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
        /// 导出项目列表
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public FileResult ExportProjects(string keyWord, string ids, string unitId)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);

            //string value = string.Empty;// 查询条件
            //string ids = string.Empty;//选中的行记录主键 
            //string unitId = string.Empty;//单位id；
            if (!string.IsNullOrEmpty(Request["keyWord"]))
            {
                keyWord = Request["keyWord"];
            }
            if (!string.IsNullOrEmpty(Request["ids"]))
            {
                ids = Request["ids"];
            }
            if (!string.IsNullOrEmpty(Request["unitId"]))
            {
                unitId = Request["unitId"];
            }
            string path = Server.MapPath("~/temps/");
            if (System.IO.Directory.Exists(path) == false)//如果不存在就创建file文件夹
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string filepath = path + "projects.csv";
            string sFileName = string.Empty;
            ret = ProjectManagementService.Export(filepath, unitId, keyWord, ids, 10000);
            if (ret.ResultType == 0)
            {
                sFileName = string.Format("项目信息-{0}.csv", DateTime.Now.ToString("yyyyMMdd"));
                if (Request.ServerVariables["http_user_agent"].ToString().IndexOf("Firefox") != -1)
                    sFileName = "=?UTF-8?B?" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sFileName)) + "?=";
                else
                    sFileName = System.Web.HttpUtility.UrlEncode(sFileName, System.Text.Encoding.UTF8);
                return File(filepath, "text/csv", sFileName);
            }
            else
            {
                return File(ret.Message, "text/plain", "项目信息表导出失败.txt");
            }
        }

    }
}