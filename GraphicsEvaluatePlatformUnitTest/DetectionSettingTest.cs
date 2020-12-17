using System;
using System.Collections.Generic;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphicsEvaluatePlatformUnitTest
{
    [TestClass]
    public class DetectionSettingTest
    {
        [TestMethod]
        public void TestAdd()
        {
            Random random = new Random(7);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Guid.NewGuid());
            dic.Add("UnitId", Guid.NewGuid());
            dic.Add("ProjectID", "项目ID");
            dic.Add("ProjectName", "项目名称");
            dic.Add("Name", "名称");
            dic.Add("NameRule", "命名规则");
            dic.Add("SizeMin", "图像大小最小值");
            dic.Add("SizeMax", "图像大小最大值");
            dic.Add("ResolutionMin", "分辨率最小值");
            dic.Add("ResolutionMax", "分辨率最大值");
            dic.Add("BrightMin", "亮度最小值");
            dic.Add("BrightMax", "亮度最大值");
            dic.Add("RectifyMin", "纠偏最小值");
            dic.Add("RectifyMax", "纠偏最大值");
            dic.Add("ImplicitWatermarks", "隐式水印");
            dic.Add("DisplayWatermarks", "显示水印");
            dic.Add("PictureType", "图片格式");
            dic.Add("ClarityMin", "清晰度最小值");
            dic.Add("ClarityMax", "清晰度最大值");
            dic.Add("QualifiedPath", "合格存放地址");
            dic.Add("UnqualifiedPath", "不合格存放地址");
            dic.Add("Remarks", "测试检测设置备注·");
            var ret = DetectionSettingService.Add(dic);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
            ret = DetectionSettingService.Add(dic);
            Assert.AreEqual(ret.ResultType, OperationResultType.Error);
            //Assert.AreEqual(ret.Message, "该检测设置已存在");
        }
        [TestMethod]
        public void TestUpdate()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", "5754d863-efa1-4675-8de9-8d035cf52d81");
            dic.Add("UnitId", "1");
            dic.Add("Name", "测试检测设置名称");
            dic.Add("Mac", "00 - 0B - 2F - 7C - 57 - 04");
            dic.Add("IP", "192.168.1.100");
            dic.Add("IsEnable", true);
            dic.Add("Remarks", "测试检测设置备注·");
            var ret = DetectionSettingService.Update(dic);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
            dic.Remove("Id");
            dic.Add("Id", "0ADB43D7-309F-44D0-91D6-D09A6CCFD685");
            ret = DetectionSettingService.Update(dic);
            Assert.AreEqual(ret.ResultType, OperationResultType.Error);
            Assert.AreEqual(ret.Message, "编辑检测设置失败");
        }
        [TestMethod]
        public void TestGetList()
        {
            BootstrapPager pager = new BootstrapPager();
            pager.filter = "测试,UID:-1";
            pager.PageSize = 20;
            pager.PageIndex = 1;
            var ret = DetectionSettingService.GetList(pager);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }
        [TestMethod()]
        public void TestDelete()
        {
            var ids = "";
            var ret = DetectionSettingService.Delete(ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.ParamError);
            ids = "29473e36-39e1-45ff-aee5-f6ac4c6f28e8";
            ret = DetectionSettingService.Delete(ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }
    }
}
