using Get_A_Taxi.Data.Migrations;
using Get_A_Taxi.Web.App_Start;
using Get_A_Taxi.Web.Hubs;
using Get_A_Taxi.Web.Infrastructure.Bridges;
using Get_A_Taxi.Web.Infrastructure.Services.HubServices;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.WebApi;
using Ninject.Web.WebApi.OwinHost;
using Ninject.Web.Common.OwinHost;
using Owin;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(Get_A_Taxi.Web.Startup))]
namespace Get_A_Taxi.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var kernel = NinjectWebCommon.bootstrapper.Kernel;
            var resolver = new NinjectSignalRDependencyResolver(kernel);

            // Binding Orders Hub to Orders Bridge
            kernel.Bind(typeof(IHubConnectionContext<dynamic>))
                .ToMethod(context => resolver
                    .Resolve<IConnectionManager>()
                    .GetHubContext<OrdersHub>()
                    .Clients)
                    .WhenInjectedInto<IOrderBridge>();

            // Binding Taxies Hub to Taxies Bridge
            kernel.Bind(typeof(IHubConnectionContext<dynamic>))
                .ToMethod(context => resolver
                    .Resolve<IConnectionManager>()
                    .GetHubContext<TaxiesHub>()
                    .Clients)
                    .WhenInjectedInto<ITaxiesBridge>();

            var config = new HubConfiguration();
            config.Resolver = resolver;
            ConfigureSignalR(app, config);

        }

        public static void ConfigureSignalR(IAppBuilder app, HubConfiguration config)
        {
            app.MapSignalR(config);
        }

    }
}
