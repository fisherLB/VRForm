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
    public class DetectionSettingController : Controller
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
        /// 取检测设置列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public string GetDetectionSettingsList(BootstrapPager pager)
        {
            if (pager.filter == null)
                pager.filter = "";
            OperationResult ret = DetectionSettingService.GetList(pager);
            string retJson = "";
            retJson = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
            return retJson;
        }

        /// <summary>
        /// 新增检测设置
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDetectionSetting(string json)
        {
            Dictionary<string, object> pro = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);

            OperationResult ret = DetectionSettingService.Add(pro);
            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message
            };
            return JsonUtil.ToJson(data);

        }
        /// <summary>
        /// 编辑检测设置
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public string EditDetectionSetting(string json)
        {
            Dictionary<string, object> pro = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);

            OperationResult ret = DetectionSettingService.Update(pro);
            if (ret.ResultType == OperationResultType.Success)
            {
                var data = new
                {
                    Success = true,
                    Message = "修改检测设置成功!"
                };
                return JsonUtil.ToJson(data);
            }
            else
            {
                var data = new
                {
                    Success = false,
                    Message = "修改检测设置失败!"
                };
                return JsonUtil.ToJson(data);
            }
        }
        /// <summary>
        /// 删除检测设置
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public string DelDetectionSettings(string ids)
        {
            OperationResult ret = DetectionSettingService.Delete(ids);
            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message,
                Data = ret.AppendData
            };
            return JsonUtil.ToJson(data);
        }
      
    }
}