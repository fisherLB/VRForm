using GraphicsEvaluatePlatform.Client.Basics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GraphicsEvaluatePlatform.Client
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);
            //根据客户端使用版本信息设定
            BaseService.CLIENTTYPE = "1";
        }
    }
}
