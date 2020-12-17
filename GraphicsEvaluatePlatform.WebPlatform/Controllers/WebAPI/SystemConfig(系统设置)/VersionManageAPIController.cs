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
    public class VersionManageAPIController : ApiController
    {
        /// <summary>
        /// 获取版本数据列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [HttpGet]
        [SkipPermission]
        [Route("api/VersionManage/GetVersionsList")]
        public string GetVersionsList([FromUri]BootstrapPager pager)
        {
            if (pager.filter == null)
                pager.filter = "";
            OperationResult ret = VersionManageService.getList(pager);
            string retJson = "";
            retJson = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
            return retJson;
        }


        /// <summary>
        /// 新增版本
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/VersionManage/AddVersion")]
        public string AddVersion(dynamic json)
        {
            string wtf = json.ToString();
            wtf = wtf.Split('&')[0];
            Dictionary<string, object> dr = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);

            OperationResult ret = VersionManageService.addVersion(dr);
            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message
            };
            return JsonUtil.ToJson(data);
        }

        /// <summary>
        /// 编辑版本信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/VersionManage/Edit")]
        public string Edit(dynamic json)
        {
            string wtf = json.ToString();
            wtf = wtf.Split('&')[0];
            Dictionary<string, object> dr = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);

            OperationResult ret = VersionManageService.Update(dr);
            if (ret.ResultType == OperationResultType.Success)
            {
                var data = new
                {
                    Success = true,
                    Message = "修改版本成功!"
                };
                return JsonUtil.ToJson(data);
            }
            else
            {
                var data = new
                {
                    Success = false,
                    Message = "修改版本失败!"
                };
                return JsonUtil.ToJson(data);
            }
        }

        /// <summary>
        /// 删除版本信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/VersionManage/Delete")]
        public string Delete([FromBody]string ids)
        {
            var sbids = ids.ToString();
            sbids = sbids.Split('&')[0];
            sbids = sbids.Replace("\"", "");
            OperationResult ret = VersionManageService.Delete(sbids);
            var data = new
            {
                Success = ret.ResultType == OperationResultType.Success,
                Message = ret.Message,
                Data = ret.AppendData
            };
            return JsonUtil.ToJson(data);
        }

        /// <summary>
        /// 启用版本
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/VersionManage/ActiveVersion")]
        public string ActiveVersion(dynamic ids)
        {
            var sbids = ids.ToString();
            sbids = sbids.Split('&')[0];
            sbids = sbids.Replace("\"", "");
            string[] arrids = sbids.Split('|');
            string idss = arrids[0];
            string values = arrids[1];
            OperationResult ret = new OperationResult(OperationResultType.Success);
            ret = VersionManageService.ActiveVersion(idss, values);
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
        /// 禁用版本
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/VersionManage/ForbiddenVersion")]
        public string ForbiddenVersion(dynamic ids)
        {
            var sbids = ids.ToString();
            sbids = sbids.Split('&')[0];
            sbids = sbids.Replace("\"", "");
            string[] arrids = sbids.Split('|');
            string idss = arrids[0];
            string values = arrids[1];
            OperationResult ret = new OperationResult(OperationResultType.Success);
            ret = VersionManageService.ActiveVersion(idss, values);
            if (ret.ResultType == 0)
            {
                return JsonUtil.ToJson(new { Success = true, Message = "操作成功" }, "yyyy-MM-dd");
            }
            else
            {
                return JsonUtil.ToJson(new { Success = false, Message = ret.Message }, "yyyy-MM-dd");
            }
        }
    }
}