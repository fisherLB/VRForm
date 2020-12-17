using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphicsEvaluatePlatform.Service;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Model;
namespace GraphicsEvaluatePlatformUnitTest
{
    /// <summary>
    /// ClientManageServiceTest 的摘要说明
    /// </summary>
    [TestClass]
    public class ClientManageServiceTest
    {
        [TestMethod]
        public void TestAdd()
        {
            Random random = new Random(7);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("c_Id", Guid.NewGuid());
            dic.Add("c_UnitId", Guid.NewGuid());
            dic.Add("c_Code", random.Next(1, 9).ToString());
            dic.Add("c_Name", "测试客户端名称" + random.Next(1,9).ToString());
            dic.Add("c_Mac", "00 - 0B - 2F - 7C - 57 - 04");
            dic.Add("c_IP", "192.168.1.100");
            dic.Add("c_IsEnable", "ture");   
            dic.Add("c_Remarks", "测试客户端备注·");
            var ret = ClientManageService.Add(dic);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
            ret = ClientManageService.Add(dic);
            Assert.AreEqual(ret.ResultType, OperationResultType.Error);
            //Assert.AreEqual(ret.Message, "该客户端已存在");
        }
        [TestMethod]
        public void TestUpdate()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("c_Id", "5754d863-efa1-4675-8de9-8d035cf52d81");
            dic.Add("c_UnitId", "1");
            dic.Add("c_Name", "测试客户端名称");
            dic.Add("c_Mac", "00 - 0B - 2F - 7C - 57 - 04");
            dic.Add("c_IP", "192.168.1.100");
            dic.Add("c_IsEnable", true);
            dic.Add("c_Remarks", "测试客户端备注·");
            var ret = ClientManageService.Update(dic);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
            dic.Remove("c_Id");
            dic.Add("c_Id", "0ADB43D7-309F-44D0-91D6-D09A6CCFD685");
            ret = ClientManageService.Update(dic);
            Assert.AreEqual(ret.ResultType, OperationResultType.Error);
            Assert.AreEqual(ret.Message, "编辑客户端失败");
        }
        [TestMethod]
        public void TestGetList()
        {
            BootstrapPager pager = new BootstrapPager();
            pager.filter = "测试,UID:-1";
            pager.PageSize = 20;
            pager.PageIndex = 1;
            var ret = ClientManageService.GetList(pager);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }
        [TestMethod()]
        public void TestDelete()
        {
            var ids = "";
            var ret = ClientManageService.Delete(ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.ParamError);
            ids = "29473e36-39e1-45ff-aee5-f6ac4c6f28e8";
            ret = ClientManageService.Delete(ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }
        [TestMethod()]
        public void TestActive()
        {
            var ids = "29473e36-39e1-45ff-aee5-f6ac4c6f28e8,0d88a621-5e0c-45f5-a9d2-d6e891952e1b,797cf9a0-f358-4eb9-8e67-e1f1af23fee9";        
            string values = "1,0,1";
            var ret = ClientManageService.ActiveClients(ids, values);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }
    }
}
