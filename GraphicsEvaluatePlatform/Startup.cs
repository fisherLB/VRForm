using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GraphicsEvaluatePlatform.Startup))]
namespace GraphicsEvaluatePlatform
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
