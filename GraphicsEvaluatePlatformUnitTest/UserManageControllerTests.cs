using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphicsEvaluatePlatform.WebPlatform.Controllers;
/***********************************************************************
* Copyright @ Taitan Soft Corporation 2018. All rights reserved.
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEvaluatePlatform.Service;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Model;

namespace GraphicsEvaluatePlatform.WebPlatform.Controllers.Tests
{
    [TestClass()]
    public class UserManageControllerTests
    {
        //添加用户
        [TestMethod()]
        public void AddUserTest()
        {
            Random r = new Random();
            Dictionary<string, object> user = new Dictionary<string, object>();
            user.Add("UnitID", "d079a6f3-5547-4313-a144-f952d524416d");
            user.Add("Us_id", Guid.NewGuid().ToString());
            user.Add("Us_Account", "UnitTest"+r.Next(0,9).ToString());
            user.Add("Us_Password", "123");
            user.Add("Us_name", "单元测试22");
            user.Add("Us_status", 1);
            user.Add("Us_type", 1);
            user.Add("Us_remark", "");
            var ret = UserService.AddUser(user);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
            ret = UserService.AddUser(user);
            Assert.AreEqual(ret.ResultType, OperationResultType.ParamError);
            Assert.AreEqual(ret.Message, "新增用户失败");
        }
        //更新用户
        [TestMethod()]
        public void UpdateUserTest()
        {
            Dictionary<string, object> user = new Dictionary<string, object>();
            user.Add("Us_id", "0ADB43D7-309F-44D0-91D6-D09A6CCFD68D");
            user.Add("Us_Password", "321");
            user.Add("Us_name", "测试");
            user.Add("UnitID", "00000000-0000-0000-0000-000000000000");
            var ret = UserService.UpdateUser(user);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
            user.Remove("Us_id");
            user.Add("Us_id", "0ADB43D7-309F-44D0-91D6-D09A6CCFD685");
            ret = UserService.UpdateUser(user);
            Assert.AreEqual(ret.ResultType, OperationResultType.ParamError);
            Assert.AreEqual(ret.Message, "无法找到指定用户");
        }
        //删除用户
        [TestMethod()]
        public void DeleteUserTest()
        {
            var ids = "";
            var ret = UserService.DeleteUser(ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.ParamError);
            ids = "5FD923DE-2A7A-4233-8579-C75DEDD7C028,0ADB43D7-309F-44D0-91D6-D09A6CCFD68D";
            ret = UserService.DeleteUser(ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }
        //初始化密码
        [TestMethod()]
        public void InitPasswordTest()
        {
            var ids = "";
            var ret = UserService.InitPassword(ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.ParamError);
            ids = "0ADB43D7-309F-44D0-91D6-D09A6CCFD68D";
            ret = UserService.InitPassword(ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }
        //启禁用户
        [TestMethod()]
        public void ChangeUserStatusTest()
        {
            var ids = "0ADB43D7-309F-44D0-91D6-D09A6CCFD68D";
            var type = "true";
            var ret = UserService.ChangeUserStatus(type, ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);

            type = "false";
            ret = UserService.ChangeUserStatus(type, ids);
            Assert.AreEqual(ret.ResultType, OperationResultType.ParamError);
        }
        //修改密码
        [TestMethod()]
        public void ChangePasswordTest()
        {
            var ids = "0ADB43D7-309F-44D0-91D6-D09A6CCFD68D";
            var newpass = "321";
            var ret = UserService.ChangePassword(ids, newpass);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);

            ids = "0ADB43D7-309F-44D0-91D6-D09A6CCFD68D";
            ret = UserService.ChangePassword(ids, "123");
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }
        //获取用户信息
        [TestMethod()]
        public void GetUserListTest()
        {
            BootstrapPager pager = new BootstrapPager();
            pager.filter = ",UID:d079a6f3-5547-4313-a144-f952d524416d";
            pager.PageSize = 20;
            pager.PageIndex = 1;
            var ret = UserService.GetUserList(pager);
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }
    }
}