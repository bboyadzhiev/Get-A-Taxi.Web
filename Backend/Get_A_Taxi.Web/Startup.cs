using Get_A_Taxi.Data.Migrations;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Owin;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(Get_A_Taxi.Web.Startup))]
namespace Get_A_Taxi.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var unityHubActivator = new MvcHubActivator();

            GlobalHost.DependencyResolver.Register(
                typeof(IHubActivator),
                () => unityHubActivator);

            app.MapSignalR();
            
        }

        public class MvcHubActivator : IHubActivator
        {
            public IHub Create(HubDescriptor descriptor)
            {
                return (IHub)DependencyResolver.Current
                    .GetService(descriptor.HubType);
            }
        }
    }
}
