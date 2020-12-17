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
    [PermissionFilterAPI]
    public class UnitManageAPIController : ApiController
    {

        /// <summary>
        /// 获取单位树列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SkipPermission]
        [Route("api/UnitManage/GetUnitList")]
        public string GetUnitList()
        {
            OperationResult ret = UnitManageService.GetUnitList();
            var str = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
            str = str.Replace("\"u_ParentId\": 0,","").Replace("\"u_ParentId\":","\"ParentId\":");
            str = str.Replace("\"u_Depth\": 0,","").Replace("\"u_Depth\":","\"Level\":");
            return str;
        }
      
        /// <summary>
        /// 新增单位
        /// </summary>
        /// <param name="json">页面传入json数据</param>
        /// <returns>json数据</returns>
        [HttpPost]
        [Route("api/UnitManage/AddUnit")]
        public string AddUnit(dynamic json)
        {
            string wtf = json.ToString();
            wtf = wtf.Split('&')[0];
            Dictionary<string, object> dr = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);
            OperationResult result = UnitManageService.AddUnit(dr);
            if (result.ResultType == OperationResultType.Success)
            {
                var data = new
                {
                    Success = true,
                    Message = result.Message
                };
                return JsonUtil.ToJson(data, "yyyy-MM-dd");
            }
            else
            {
                var data = new
                {
                    Success = false,
                    Message = result.Message
                };
                return JsonUtil.ToJson(data, "yyyy-MM-dd");
            };
        }

        /// <summary>
        /// 编辑单位
        /// </summary>
        /// <param name="json">页面传入json数据</param>
        /// <returns>json数据</returns>
        [HttpPut]
        [Route("api/UnitManage/EditUnit")]
        public string EditUnit(dynamic json)
        {
            string wtf = json.ToString();
            wtf = wtf.Split('&')[0];
            Dictionary<string, object> dr = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);
            OperationResult result = UnitManageService.EditUnit(dr);
            if (result.ResultType == OperationResultType.Success)
            {
                var data = new
                {
                    Success = true,
                    Message = result.Message
                };
                return JsonUtil.ToJson(data, "yyyy-MM-dd");
            }
            else
            {
                var data = new
                {
                    Success = false,
                    Message = result.Message
                };
                return JsonUtil.ToJson(data, "yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 删除单位
        /// </summary>
        /// <param name="json">页面传入的json数据</param>
        /// <returns>删除结果</returns>
        [HttpDelete]
        [Route("api/UnitManage/DeleteUnit")]
        public string DeleteUnit([FromBody] string ids)
        {

            var sbids = ids.ToString();
            sbids = sbids.Split('&')[0];
            sbids = sbids.Replace("\"", "");
            OperationResult result = UnitManageService.DeleteUnit(sbids);

            if (result.ResultType == OperationResultType.Success)
            {
                var data = new
                {
                    Success = true,
                    Message = result.Message
                };
                return JsonUtil.ToJson(data, "yyyy-MM-dd");
            }
            else
            {
                var data = new
                {
                    Success = false,
                    Message = result.Message
                };
                return JsonUtil.ToJson(data, "yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 启用单位
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/UnitManage/ActiveUnits")]
        public string ActiveUnits(dynamic ids)
        {
            var sbids = ids.ToString();
            sbids = sbids.Split('&')[0];
            sbids = sbids.Replace("\"", "");
            string[] arrids = sbids.Split('|');
            string idss = arrids[0];
            string values = arrids[1];
            OperationResult ret = new OperationResult(OperationResultType.Success);
            ret = UnitManageService.ActiveUnits(idss, values);
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
        /// 禁用单位
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/UnitManage/ForbiddenUnits")]
        public string ForbiddenUnits(dynamic ids)
        {
            var sbids = ids.ToString();
            sbids = sbids.Split('&')[0];
            sbids = sbids.Replace("\"", "");
            string[] arrids = sbids.Split('|');
            string idss = arrids[0];
            string values = arrids[1];
            OperationResult ret = new OperationResult(OperationResultType.Success);
            ret = UnitManageService.ActiveUnits(idss, values);
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
        /// 获取左侧树
        /// </summary>
        /// <returns>json数据</returns>
        [HttpGet]
        [SkipPermission]
        [Route("api/UnitManage/GetUnitCombotree")]
        public string GetUnitCombotree()
        {
            OperationResult ret = UnitManageService.GetUnitComboTree();
            ((List<ComboTree>)ret.AppendData).Insert(0, new ComboTree { id = "-1", text = "全部" });
            return JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
        }
        [SkipPermission]
        [HttpGet]
        [Route("api/UnitManage/GetUnitCombotree2")]
        public string GetUnitCombotree2()
        {
            OperationResult ret = UnitManageService.GetUnitComboTree();
            return JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
        }
    }
}