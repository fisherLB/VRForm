using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Service;
using GraphicsEvaluatePlatform.WebPlatform.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraphicsEvaluatePlatform.WebPlatform.Controllers
{
    public class UnitManageController : Controller
    {
        [PermissionFilter]
        // GET: UnitManage
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取单位树列表
        /// </summary>
        /// <returns></returns>
        public string GetUnitList()
        {
            OperationResult ret = UnitManageService.GetUnitList();
            var str = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
            var ss = str.Replace("\"ParentID\": 0,", " ").Replace("\"ParentID\":", "\"_parentId\":");
            return str.Replace("\"ParentID\": 0,", " ").Replace("\"ParentID\":", "\"_parentId\":");
        }

        /// <summary>
        /// 新增单位
        /// </summary>
        /// <param name="json">页面传入json数据</param>
        /// <returns>json数据</returns>
        public string AddUnit(string json)
        {
            Dictionary<string, object> dr = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);
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
        public string EditUnit(string json)
        {
            Dictionary<string, object> dr = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);
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
        public string DeleteUnit(string ids)
        {
            OperationResult result = UnitManageService.DeleteUnit(ids);

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
    }
}