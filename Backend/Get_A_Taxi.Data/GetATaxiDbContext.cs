namespace Get_A_Taxi.Data
{
    using System.Data.Entity;
    using Get_A_Taxi.Data.Migrations;
    using Get_A_Taxi.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

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

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public new int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}