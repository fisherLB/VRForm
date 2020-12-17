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
    public class MenuManageAPIController : ApiController
    {

        /// <summary>
        /// 获取session中的菜单列表
        /// </summary>
        /// <returns></returns>
        [SkipPermission]
        [HttpGet]
        [Route("api/MenuManage/GetAllMenus")]
        public string GetAllMenus()
        {

            var ret = (object)ServiceBase.GetInfo(ServiceBase.MENULIST);//.GetParentMenuList();
            return JsonUtil.ToJson(ret, "yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 子菜单列表(功能管理列表)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SkipPermission]
        [HttpGet]
        [Route("api/MenuManage/GetMenuList")]
        public string GetMenuList()
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);

            ret = MenuManageService.GetMenuList();
            var str = JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
            str = str.Replace("\"Func_parent_id\": 0,", "").Replace("\"Func_parent_id\":", "\"ParentId\":");
            str = str.Replace("\"Depth\": 0,", "").Replace("\"Depth\":", "\"Level\":");
            // return JsonUtil.ToJson(ret.AppendData);
            return str;

        }

        /// <summary>
        /// 获取内存中的菜单列表
        /// </summary>
        /// <returns></returns>
        [SkipPermission]
        [HttpGet]
        [Route("api/MenuManage/GetMemoryMenuList")]
        public string GetMemoryMenuList()
        {
            var ret = (object)ServiceBase.GetInfo(ServiceBase.MENULIST);//.GetParentMenuList();
            return JsonUtil.ToJson(ret, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        [SkipPermission]
        [HttpGet]
        [Route("api/MenuManage/GetMenus")]
        public string GetMenus()
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            ret = MenuManageService.GetMenus();
            return JsonUtil.ToJson(ret.AppendData, "yyyy-MM-dd HH:mm:ss");
        }


        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/MenuManage/AddMenu")]
        public string AddMenu(dynamic json)
        {
            string wtf = json.ToString();
            wtf = wtf.Split('&')[0];
            PublicHelper.CheckArgument(wtf, "json");
            wtf = wtf.Replace("\"{", "{");
            wtf = wtf.Replace("}\"", "}");
            wtf = wtf.Replace("\\", "");
            Dictionary<string, object> model = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);
            OperationResult ret = MenuManageService.AddMenuInfo(model);
            var retJson = new MessageModel(ret);
            return JsonUtil.ToJson(retJson);
        }
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/MenuManage/EditMenu")]
        public string EditMenu(dynamic json)
        {
            string wtf = json.ToString();
            wtf = wtf.Split('&')[0];
            PublicHelper.CheckArgument(wtf, "json");

            Dictionary<string, object> model = JsonUtil.ConvertToObject<Dictionary<string, object>>(wtf);
            OperationResult ret = MenuManageService.EditMenuInfo(model);
            var retJson = new MessageModel(ret);

            return JsonUtil.ToJson(retJson);
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="Func_id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/MenuManage/DelMenu")]
        public string DelMenu([FromBody]string Func_id)
        {
            var sbids = Func_id.ToString();
            sbids = sbids.Split('&')[0];
            sbids = sbids.Replace("\"", "");
            PublicHelper.CheckArgument(sbids, "string");
            Dictionary<string, object> dicw = new Dictionary<string, object>();
            dicw.Add("Func_id", sbids);
            OperationResult ret = MenuManageService.DelMenuInfo(dicw);
            var retJson = new MessageModel(ret);
            return JsonUtil.ToJson(retJson);

        }
    }
}