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
    /// <summary>
    /// 功能管理
    /// </summary>
    [PermissionFilter]
    public class MenuManageController : Controller
    {
        // GET: MenuManage
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取session中的菜单列表
        /// </summary>
        /// <returns></returns>
        [SkipPermission]
        public string GetAllMenus()
        {

            var ret = (object)ServiceBase.GetInfo(ServiceBase.MENULIST);//.GetParentMenuList();
            return JsonUtil.ToJson(ret, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [SkipPermission]
        public string AddMenu(string json)
        {

            PublicHelper.CheckArgument(json, "json");

            Dictionary<string, object> model = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);
            OperationResult ret = MenuManageService.AddMenuInfo(model);
            var retJson = new MessageModel(ret);
            return JsonUtil.ToJson(retJson);
        }
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string EditMenu(string json)
        {

            PublicHelper.CheckArgument(json, "json");

            Dictionary<string, object> model = JsonUtil.ConvertToObject<Dictionary<string, object>>(json);
            OperationResult ret = MenuManageService.EditMenuInfo(model);
            var retJson = new MessageModel(ret);

            return JsonUtil.ToJson(retJson);
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="Func_id"></param>
        /// <returns></returns>
        public string DelMenu(string Func_id)
        {
            PublicHelper.CheckArgument(Func_id, "string");
            Dictionary<string, object> dicw = new Dictionary<string, object>();
            dicw.Add("Func_id", Func_id);
            OperationResult ret = MenuManageService.DelMenuInfo(dicw);
            var retJson = new MessageModel(ret);
            return JsonUtil.ToJson(retJson);

        }

        [SkipPermission]
        /// <summary>
        /// 子菜单列表(功能管理列表)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetMenuList()
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);

            ret = MenuManageService.GetMenuList();

            return JsonUtil.ToJson(ret.AppendData);

        }
    }
}