using Get_A_Taxi.Data.Migrations;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Get_A_Taxi.Web.Startup))]
namespace Get_A_Taxi.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            
        }
    }
}
