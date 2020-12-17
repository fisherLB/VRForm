using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GraphicsEvaluatePlatform.Service;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Model;

namespace GraphicsEvaluatePlatformUnitTest
{
    [TestClass]
    public class PermissionManageServiceTest
    {
        
        [TestMethod]
        public void GetUserListTest()
        {
            BootstrapPager pager = new BootstrapPager();
            pager.PageSize = 10;
            pager.PageIndex = 1;
            pager.filter = ",UID:1";
            var re = PermissionManageService.GetUserList(pager);
            Assert.AreEqual(re.ResultType, OperationResultType.Success);
        }

        [TestMethod]
        public void GetUserPermissionTest()
        {
            string userId = "bfbffe01-9508-4643-af20-3cce48110bc4";
            var re = PermissionManageService.GetUserPermission(userId);
            Assert.AreEqual(re.ResultType, OperationResultType.Success);
        }

        [TestMethod]
        public void SaveUserPermissionTest()
        {
            string data = "[{\"Func_id\":\"ef18d989-de4b-4802-b720-8ba5161bfa99\",\"Us_id\":\"744bd8bb-126c-45a4-91ca-79b2ce940bd0\",\"Full_url\":\"/HomePage/Index\",\"Func_grade\":1}]";
            string userId = "744bd8bb-126c-45a4-91ca-79b2ce940bd0";
            var re = PermissionManageService.SaveUserPermission(data, userId);
            Assert.AreEqual(re.ResultType, OperationResultType.Success);
        }
    
    }
}
