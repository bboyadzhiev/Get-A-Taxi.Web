[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Get_A_Taxi.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Get_A_Taxi.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Get_A_Taxi.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Get_A_Taxi.Data.Migrations;
    using Get_A_Taxi.Data;
    using Get_A_Taxi.Web.Infrastructure.Services;
    using Get_A_Taxi.Web.Infrastructure.Services.Contracts;
    using Get_A_Taxi.Web.Infrastructure.Caching;
    using Get_A_Taxi.Web.Infrastructure.Populators;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            ////var sqlServerPath = @".\SQLEXPRESS";
            //// Config.SetConnectionString("GetATaxi", sqlServerPath);
            // var contextFactory = new MigrationsContextFactory();
            // var db = new GetATaxiData(contextFactory.Create());
            kernel.Bind<IGetATaxiDbContext>().To<GetATaxiDbContext>()
                .WithConstructorArgument("connectionString", Config.ConnectionString);
            kernel.Bind<IGetATaxiData>().To<GetATaxiData>();
            //   .WithConstructorArgument("data", d => db);
            kernel.Bind<IOrdersListService>().To<OrdersListService>();
            kernel.Bind<IAccountService>().To<AccountService>();
            kernel.Bind<ITaxiService>().To<TaxiService>();
            kernel.Bind<IOperatorService>().To<OperatorService>();
            kernel.Bind<ICacheService>().To<InMemoryCache>();
            kernel.Bind<IDropDownListPopulator>().To<DropDownListPopulator>();
        }
    }
}
