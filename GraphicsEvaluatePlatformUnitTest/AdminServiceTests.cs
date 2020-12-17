using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphicsEvaluatePlatform.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEvaluatePlatform.Model;
using GraphicsEvaluatePlatform.Infrastructure;

namespace GraphicsEvaluatePlatform.Service.Tests
{
    [TestClass()]
    public class AdminServiceTests
    {
        [TestMethod()]
        //用户登录
        public void UserLoginTest()
        {

            #region //账号不存在
            LoginModel model = new LoginModel();
            model.LoginName = "qly";
            model.Password = "123";
            model.ValidateCode = "";
            model.ValidateCode = "false";
            OperationResult result = AdminService.UserLogin(model);
            Assert.AreEqual(result.ResultType, OperationResultType.QueryNull);
            #endregion

            #region //账号正确，密码不正确
            LoginModel model1 = new LoginModel();
            model.LoginName = "UnitTest202";
            model.Password = "1234";
            model.ValidateCode = "";
            model.ValidateCode = "false";
            OperationResult result1 = AdminService.UserLogin(model1);
            Assert.AreEqual(result.ResultType, OperationResultType.Error);
            #endregion

            #region //账号正确，密码正确（包括账号被冻结，账号被管理员禁用）
            LoginModel model2 = new LoginModel();
            model.LoginName = "UnitTest202";
            model.Password = "123";
            model.ValidateCode = "";
            model.ValidateCode = "false";
            OperationResult result2 = AdminService.UserLogin(model2);
            Assert.AreEqual(result.ResultType, OperationResultType.Error);
            #endregion
        }

        [TestMethod()]
        public void ChangePasswordTest()
        {
            var id = @"CEC304E9-D83D-4D95-9A70-76B0D5EFE60E";
            var ret = AdminService.ChangePassword("", "123", "1234");
            Assert.AreEqual(ret.ResultType, OperationResultType.ParamError);

            ret = AdminService.ChangePassword(id, "", "123");
            Assert.AreEqual(ret.ResultType, OperationResultType.Error);

            ret = AdminService.ChangePassword(id, "123", "");
            Assert.AreEqual(ret.ResultType, OperationResultType.Error);

            ret = AdminService.ChangePassword(id, "123", "123");
            Assert.AreEqual(ret.ResultType, OperationResultType.Success);
        }


    }
}