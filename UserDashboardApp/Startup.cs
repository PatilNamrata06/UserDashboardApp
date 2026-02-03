using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(UserDashboardApp.Startup))]
namespace UserDashboardApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
