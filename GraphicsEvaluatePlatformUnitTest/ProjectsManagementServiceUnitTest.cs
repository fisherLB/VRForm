
using GraphicsEvaluatePlatform.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphicsEvaluatePlatform.Service;
using System.Collections.Generic;
using System;
using GraphicsEvaluatePlatform.Model;

namespace GraphicsEvaluatePlatformUnitTest
{
    [TestClass]
    public class ProjectsManagementServiceUnitTest
    {
        [TestMethod]
        public void TestAdd()
        {
            Random random = new Random(7);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("p_Id", Guid.NewGuid());
            dic.Add("p_UnitId", Guid.NewGuid());
            dic.Add("p_Name", "测试项目名称"+random.Next().ToString());
            dic.Add("p_Region", "测试项目地区");
            dic.Add("p_Contactor", "测试项目负责人");
            dic.Add("p_DataSize", "测试项目数据大小");
            dic.Add("p_Remarks", "测试项目备注·");
            var ret = ProjectManagementService.Add(dic);         
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
            ret = ProjectManagementService.Add(dic);
            Assert.AreEqual(ret.ResultType, OperationResultType.Error);
            //Assert.AreEqual(ret.Message, "该项目已存在");
        }
        [TestMethod]
        public void TestUpdate()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("p_Id", "32615fd6-30ba-4069-bc3b-2cf9f1030f75");
            dic.Add("p_UnitId", "4765dd01-7ea1-4c16-a4e9-03623adae2b0");
            dic.Add("p_UnitName", "测试项目单位名称");
            dic.Add("p_Name", "测试项目更改名称");
            dic.Add("p_Region", "测试项目地区");
            dic.Add("p_Contactor", "测试项目负责人");
            dic.Add("p_DataSize", "测试项目数据大小");
            dic.Add("p_Remarks", "测试项目备注·");
            var ret = ProjectManagementService.Update(dic);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
            dic.Remove("p_Id");
            dic.Add("p_Id", "0ADB43D7-309F-44D0-91D6-D09A6CCFD685");
            ret = ProjectManagementService.Update(dic);
            Assert.AreEqual(ret.ResultType, OperationResultType.Error);
            Assert.AreEqual(ret.Message, "编辑项目失败");
        }
        [TestMethod]
        public void TestGetList()
        {
            BootstrapPager pager = new BootstrapPager();
            pager.filter = "测试,UID:-1";
            pager.PageSize = 20;
            pager.PageIndex= 1;
            var ret = ProjectManagementService.GetList(pager);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }
        [TestMethod()]
        public void TestDelete()
        {
            var ids = "";
            var ret = ProjectManagementService.Delete(ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.ParamError);
            ids = "32615fd6-30ba-4069-bc3b-2cf9f1030f75";
            ret = ProjectManagementService.Delete(ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }
    }
}
