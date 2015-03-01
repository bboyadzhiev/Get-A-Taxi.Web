using Get_A_Taxi.Data.Migrations;
using Get_A_Taxi.Web.App_Start;
using Get_A_Taxi.Web.Hubs;
using Get_A_Taxi.Web.Infrastructure.Services.Hubs;
using Get_A_Taxi.Web.Infrastructure.Services.Hubs.HubServices;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Ninject;
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

            //var unityHubActivator = new MvcHubActivator();
            //GlobalHost.DependencyResolver.Register(
            //    typeof(IHubActivator),
            //    () => unityHubActivator);
            // app.MapSignalR();

            var kernel = NinjectWebCommon.bootstrapper.Kernel;
            var resolver = new NinjectSignalRDependencyResolver(kernel);

            kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
                    resolver.Resolve<IConnectionManager>().GetHubContext<OrdersHub>().Clients
                        ).WhenInjectedInto<IOrderBridge>();

            var config = new HubConfiguration();
            config.Resolver = resolver;
            ConfigureSignalR(app, config);
            
        }

        public static void ConfigureSignalR(IAppBuilder app, HubConfiguration config)
        {
            app.MapSignalR(config);
        }

        //public class MvcHubActivator : IHubActivator
        //{
        //    public IHub Create(HubDescriptor descriptor)
        //    {
        //        return (IHub)DependencyResolver.Current
        //            .GetService(descriptor.HubType);
        //    }
        //}
    }
}
