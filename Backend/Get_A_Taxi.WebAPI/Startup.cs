using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Get_A_Taxi.Data;
using Get_A_Taxi.Data.Migrations;
using Microsoft.Owin;
using Ninject;
using Owin;

[assembly: OwinStartup(typeof(Get_A_Taxi.WebAPI.Startup))]

namespace Get_A_Taxi.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var sqlServerPath = @".\SQLEXPRESS";
            Config.SetConnectionString("GetATaxi", sqlServerPath);
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            RegisterMappings(kernel);
            return kernel;
        }

        private static void RegisterMappings(StandardKernel kernel)
        {
            var contextFactory = new MigrationsContextFactory();
            var db = new GetATaxiData(contextFactory.Create());
            kernel.Bind<IGetATaxiData>().To<GetATaxiData>()
                .WithConstructorArgument("data", d => db);
        }
    }
}
