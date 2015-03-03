namespace Get_A_Taxi.Data
{
    using System.Data.Entity;
    using Get_A_Taxi.Data.Migrations;
    using Get_A_Taxi.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Validation;

    public class GetATaxiDbContext : IdentityDbContext<ApplicationUser>, IGetATaxiDbContext
    {
        //public GetATaxiDbContext()
        //{
        //}

        public GetATaxiDbContext(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<GetATaxiDbContext, Configuration>());
            this.Database.Connection.ConnectionString = connectionString;
        }

        public static GetATaxiDbContext Create()
        {
            return new GetATaxiDbContext(Config.ConnectionString);
        }

        public IDbSet<Taxi> Taxies { get; set; }

        public IDbSet<TaxiStand> Stands { get; set; }

        public IDbSet<Order> Orders { get; set; }

        public IDbSet<Photo> Photos { get; set; }

        public IDbSet<District> Districts { get; set; }

        public IDbSet<Location> Locations { get; set; }

        public IDbSet<OperatorOrder> OperatorsOrders { get; set; }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public new int SaveChanges()
        {
            int code = 0;
            try
            {
                code = base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        int i = 5;
                    }
                }
                throw e;
            }
            catch (Exception e)
            {

                throw e;
            }

            return code;
        }
    }
}