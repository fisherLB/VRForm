/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatformUnitTest
 * 项目描述: 
 * 类 名 称: UserGroupServiceTests
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: Administrator
 * 命名空间: GraphicsEvaluatePlatformUnitTest
 * 文件名称: UserGroupServiceTests
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/3 8:49:31
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.WebPlatform.Controllers.Tests
{
    [TestClass()]
    public class UserGroupServiceTests
    {
        string ugid = "";
        [TestMethod()]
        public void GetUserGroupComboboxTest()
        {
            var ret = UserGroupService.GetUserGroupCombobox();
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }

        [TestMethod()]
        public void GetUserGroupListTest()
        {
            BootstrapPager pager = new BootstrapPager();
            pager.filter = ",UID:-1";
            //pager.rows = 20;
            //pager.page = 1;
            pager.PageSize = 20;
            pager.PageIndex = 1;
            var ret = UserGroupService.GetUserGroupList(pager);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }
        //添加用户组
        [TestMethod()]
        public void AddUserGroupTest()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Ug_Name", "单元测试组Q");
            dic.Add("Ug_type", 1);
            dic.Add("UnitID", "d079a6f3-5547-4313-a144-f952d524416d");
            var ret = UserGroupService.AddUserGroup(dic);
            //ugid = ((DataRow)ret.AppendData)["Ug_id"].ToString().Trim();
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }

        //修改用户组
        [TestMethod()]
        public void EditUserGroupTest()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Ug_id", "9f76052b-e18b-4c53-85c6-c053e2a76151");
            dic.Add("Ug_Name", "单元测试组X");
            dic.Add("Ug_type", 1);
            dic.Add("UnitID", "d079a6f3-5547-4313-a144-f952d524416d");
            var ret = UserGroupService.EditUserGroup(dic);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }
        //删除用户组
        [TestMethod()]
        public void DeleteUserGroupTest()
        {
            var ids = "";
            var ret = UserGroupService.DeleteUserGroup(ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.ParamError);
            ids = "9f76052b-e18b-4c53-85c6-c053e2a76151";
            ret = UserGroupService.DeleteUserGroup(ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }
        //启禁用户组
        [TestMethod()]
        public void ChangeUserGroupStatusTest()
        {
            var ids = "8404af3e-8155-4562-afd1-e07e0cebefd8";
            var type = "false";
            var ret = UserGroupService.ChangeUserGroupStatus(type, ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);

            //type = "false";
            //ret = UserGroupService.ChangeUserGroupStatus(type, ids);
            //Assert.AreEqual(ret.ResultType, OperationResultType.ParamError);
        }
    }
}
