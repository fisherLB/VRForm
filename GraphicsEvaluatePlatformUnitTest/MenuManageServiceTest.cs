using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GraphicsEvaluatePlatform.Service;
using GraphicsEvaluatePlatform.Infrastructure;

namespace GraphicsEvaluatePlatformUnitTest
{
    /// <summary>
    /// 功能菜单
    /// </summary>
    [TestClass]
    public class MenuManageServiceTest
    {
        //添加菜单
        [TestMethod]
        public void AddMenuInfoTest()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("Func_name", "测试菜单");
            dic.Add("Func_code", " HomePage");
            dic.Add("Func_parent_id", "643a7a93-b095-429b-8aa7-05ea77730b1f");
            dic.Add("Func_id", "");
            dic.Add("Func_urlPath", "/HomePage/TestIndex");
            dic.Add("Func_full_name", "首页-测试菜单");
            dic.Add("Func_sequence", 1);
            dic.Add("Func_type", 1);
            var re = MenuManageService.AddMenuInfo(dic);
            Assert.AreEqual(re.ResultType, OperationResultType.Success);
        }
        //修改菜单
        [TestMethod]
        public void EditMenuInfoTest()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Func_name", "测试菜单");
            dic.Add("Func_code", " HomePage");
            dic.Add("Func_parent_id", "643a7a93-b095-429b-8aa7-05ea77730b1f");
            dic.Add("Func_id", "7E9B3338-E011-4161-8B80-998D5503B832");
            dic.Add("Func_urlPath", "/HomePage/TestIndex");
            dic.Add("Func_full_name", "首页-测试菜单");
            dic.Add("Func_sequence", 1);
            dic.Add("Func_type", 1);
            var re = MenuManageService.EditMenuInfo(dic);
            Assert.AreEqual(re.ResultType, OperationResultType.Error);
        }
        //删除菜单
        [TestMethod]
        public void DelMenuInfoTest()
        {
            Dictionary<string, object> dicw = new Dictionary<string, object>();
            dicw.Add("Func_id", "7E9B3338-E011-4161-8B80-998D5503B823");
            var re = MenuManageService.DelMenuInfo(dicw);
            Assert.AreEqual(re.ResultType, OperationResultType.Error);
        }
        //子菜单列表(功能管理列表)
        [TestMethod]
        public void GetMenuList()
        {
            var re = MenuManageService.GetMenuList();
            Assert.AreEqual(re.ResultType, OperationResultType.Success);
        }
    }
}
