using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Threading;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(GraphicsEvaluatePlatform.WebPlatform.Startup))]

namespace GraphicsEvaluatePlatform.WebPlatform
{
    public class Startup
    {
        Thread thread = null;
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>//路由
            {
                map.UseCors(CorsOptions.AllowAll);//允许跨域
                var hubConfiguration = new HubConfiguration
                {
                    EnableJSONP = true//跨域的关键语句
                };
                map.RunSignalR(hubConfiguration);
            });
            app.MapSignalR();//

            //启动线程发送消息通知  
            thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(2000);
                    //new EchoHub().Broadcast("后台自动发送消息");  
                    try
                    {
                        //发送失败：Using a Hub instance not created by the HubPipeline is unsupported.  
                        //new EchoHub().Clients.All.Message("后台自动发送消息");  
                        //发送失败：Using a Hub instance not created by the HubPipeline is unsupported.  
                        //DefaultHubManager manager = new DefaultHubManager(GlobalHost.DependencyResolver);  
                        //var hub = manager.ResolveHub("EchoHub") as EchoHub;  
                        //hub.Clients.All.Message("asdfasdf");  
                        //发送消息成功  
                        var hub = GlobalHost.ConnectionManager.GetHubContext<LetsChat>();
                        //hub.Clients.All.Message("asdfasdf");
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }
            });
            thread.Start();

        }
    }
}
