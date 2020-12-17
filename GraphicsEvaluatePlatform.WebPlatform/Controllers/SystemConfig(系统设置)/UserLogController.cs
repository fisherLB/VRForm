using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraphicsEvaluatePlatform.WebPlatform.Controllers
{
    public class UserLogController : Controller
    {
        /// <summary>
        /// 用户操作日志
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取用户日志列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public string GetUserLogList(BootstrapPager pager)
        {
            OperationResult ret = UserLogService.GetUserLogList(pager);
            return JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
        }

        #region 导出模块
        public FileResult ExportUserLog(FormCollection collection)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);

            string value = string.Empty;
            string ids = string.Empty;
            string unitId = string.Empty;
            if (!string.IsNullOrEmpty(Request["value"]))
            {
                value = Request["value"];
            }
            if (!string.IsNullOrEmpty(Request["unitId"]))
            {
                unitId = Request["unitId"];
            }
            if (!string.IsNullOrEmpty(Request["ids"]))
            {
                ids = Request["ids"];
            }
            string path = Server.MapPath("~/temps/");
            if (System.IO.Directory.Exists(path) == false)//如果不存在就创建file文件夹
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string filepath = path + "UserLog.csv";
            string sFileName = string.Empty;
            ret = UserLogService.WriteFileStream(filepath, unitId, value, ids, 10000);
            if (ret.ResultType == 0)
            {
                sFileName = string.Format("用户日志-{0}.csv", DateTime.Now.ToString("yyyyMMdd"));
                if (Request.ServerVariables["http_user_agent"].ToString().IndexOf("Firefox") != -1)
                    sFileName = "=?UTF-8?B?" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sFileName)) + "?=";
                else
                    sFileName = System.Web.HttpUtility.UrlEncode(sFileName, System.Text.Encoding.UTF8);
                return File(filepath, "text/csv", sFileName);
            }
            else
            {
                return File(ret.Message, "text/plain", "用户日志导出失败.txt");
            }
        }

        #endregion
    }
}