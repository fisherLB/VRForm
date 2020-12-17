using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraphicsEvaluatePlatform.WebPlatform.Controllers.ClientManage
{
    public class ClientManageController : Controller
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
        /// 新增终端
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddClient(string json)
        {
            Dictionary<string, object> pro = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);

            OperationResult ret = ClientManageService.Add(pro);
            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message
            };
            return JsonUtil.ToJson(data);

        }
        /// <summary>
        /// 编辑终端信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public string EditClient(string json)
        {
            Dictionary<string, object> pro = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);

            OperationResult ret = ClientManageService.Update(pro);
            if (ret.ResultType == OperationResultType.Success)
            {
                var data = new
                {
                    Success = true,
                    Message = "修改客户端成功!"
                };
                return JsonUtil.ToJson(data);
            }
            else
            {
                var data = new
                {
                    Success = false,
                    Message = "修改客户端失败!"
                };
                return JsonUtil.ToJson(data);
            }
        }
        /// <summary>
        /// 启禁终端
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public string ActiveClients(FormCollection collection)
        {
            string ids = collection["ids"];
            string values = collection["values"];
            OperationResult ret = new OperationResult(OperationResultType.Success);
            ret = ClientManageService.ActiveClients(ids, values);
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
        /// 删除终端
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public string DelClients(string ids)
        {
            OperationResult ret = ClientManageService.Delete(ids);
            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message,
                Data = ret.AppendData
            };
            return JsonUtil.ToJson(data);
        }
        /// <summary>
        /// 取终端列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public string GetClientsList(BootstrapPager pager)
        {
            if (pager.filter == null)
                pager.filter = "";
            OperationResult ret = ClientManageService.GetList(pager);
            string retJson = "";
            retJson = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
            return retJson;
        }
    }
}
