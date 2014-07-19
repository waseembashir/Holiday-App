using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HolidayApp.Startup))]
namespace HolidayApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
