using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Service;
using GraphicsEvaluatePlatform.WebPlatform.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GraphicsEvaluatePlatform.WebPlatform.Controllers.WebAPI
{
    public class DetectionSettingAPIController : ApiController
    {
        /// <summary>
        /// 取检测设置列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [HttpGet]
        [SkipPermission]
        [Route("api/DetectionSetting/GetDetectionSettingsList")]
        public string GetDetectionSettingsList([FromUri]BootstrapPager pager)
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
        [Route("api/DetectionSetting/AddDetectionSetting")]
        public string AddDetectionSetting(dynamic json)
        {
            string wtf = json.ToString();
            wtf = wtf.Split('&')[0];
            Dictionary<string, object> dr = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);          
            OperationResult ret = DetectionSettingService.Add(dr);
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
        [HttpPut]
        [Route("api/DetectionSetting/EditDetectionSetting")]
        public string EditDetectionSetting(dynamic json)
        {
            string wtf = json.ToString();
            wtf = wtf.Split('&')[0];
            Dictionary<string, object> dr = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);
            OperationResult ret = DetectionSettingService.Update(dr);
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
        [HttpDelete]
        [Route("api/DetectionSetting/DelDetectionSettings")]
        public string DelDetectionSettings([FromBody]string ids)
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