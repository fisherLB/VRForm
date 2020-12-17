using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphicsEvaluatePlatform.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEvaluatePlatform.Infrastructure;

namespace GraphicsEvaluatePlatform.Service.Tests
{
    [TestClass()]
    public class UnitManageServiceTests
    {
        [TestMethod()]
        //新增单位
        public void AddUnitTest()
        {
            Random random = new Random();
            Dictionary<string, object> unit = new Dictionary<string, object>();
            unit.Add("u_Name", "test1" + random.Next(0, 9).ToString());
            unit.Add("u_Remarks", "我是备注");
            unit.Add("u_Contactor", "我是联系人");
            unit.Add("u_Telephone", "15699856695");
            unit.Add("u_EMail", "1156989695@qq.com");
            unit.Add("u_Address", "我是地址");
            unit.Add("u_IsEnable", true);//是否启用
            unit.Add("u_ParentId", "0");
            unit.Add("u_Region", "我是行政地区");
            OperationResult re = UnitManageService.AddUnit(unit);
            Assert.AreEqual(re.ResultType, Infrastructure.OperationResultType.Success);
        }

        [TestMethod()]
        //修改单位
        public void EditUnitTest()
        {
            Dictionary<string, object> unit = new Dictionary<string, object>();
            unit.Add("u_Id", "d079a6f3-5547-4313-a144-f952d524416d");
            unit.Add("childIds", "'ed61b291-eafe-4f20-915c-cae96f6867a9','550250d1-e07f-4b83-ac95-e126d9b9875a'");
            unit.Add("u_Name", "泰坦研发部");
            unit.Add("u_Remarks", "我是备注edit");
            unit.Add("u_Contactor", "我是联系人edit");
            unit.Add("u_Telephone", "15699856695");
            unit.Add("u_EMail", "1156989695@qq.com");
            unit.Add("u_Address", "我是地址edot");
            unit.Add("u_IsEnable", true);
            unit.Add("u_Region", "我是行政地区edit");
            OperationResult re = UnitManageService.EditUnit(unit);
            Assert.AreEqual(re.ResultType, Infrastructure.OperationResultType.Success);
        }

        [TestMethod()]
        //删除单位
        public void DeleteUnitTest()
        {
            string ids = "abaf28b9-c239-4371-bd45-44492f7aa256,17e6baa0-bb74-4bc1-9234-2e34312b1978";
            OperationResult ret = UnitManageService.DeleteUnit(ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }

        [TestMethod()]
        public void GetUnitListTest()
        {
            OperationResult re = UnitManageService.GetUnitList();
            Assert.AreEqual(re.ResultType, Infrastructure.OperationResultType.Success);
        }
    }
}