using GraphicsEvaluatePlatform.Client.Models;
using GraphicsEvaluatePlatform.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Client.Repository
{
    interface ILoginService
    {
        OperationResult Login(Models.LoginModel model);

        LoginModel LoadLoginUser();

        /// <summary>
        /// 同步数据
        /// </summary>
        OperationResult SynchronousData();

        OperationResult CheckVersion();
    }
}
