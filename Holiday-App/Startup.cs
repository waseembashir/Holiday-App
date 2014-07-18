using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Holiday_App.Startup))]
namespace Holiday_App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
