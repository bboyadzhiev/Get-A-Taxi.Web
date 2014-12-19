namespace Get_A_Taxi.Data
{
    using System.Data.Entity;
    using Get_A_Taxi.Data.Migrations;
    using Get_A_Taxi.Models;

    public class GetATaxiDbContext : DbContext, IGetATaxiDbContext
    {
        public GetATaxiDbContext()
        {
        }

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

        public IDbSet<ApplicationUser> Users { get; set; }

        public IDbSet<Taxi> Taxies { get; set; }

        public IDbSet<TaxiStand> Stands { get; set; }

        public IDbSet<Order> Orders { get; set; }

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