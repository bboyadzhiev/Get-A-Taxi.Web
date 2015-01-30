using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Data.Migrations
{
    public class MigrationsContextFactory : IDbContextFactory<GetATaxiDbContext>
    {
        public MigrationsContextFactory()
        {
        }
        public GetATaxiDbContext Create()
        {
            //if (String.IsNullOrEmpty(Config.ConnectionString))
            //{
            //    var sqlServerPath = @".\SQLEXPRESS";
            //    Config.SetConnectionString("GetATaxi", sqlServerPath);
            //}
            return new GetATaxiDbContext(Config.ConnectionString);
        }
    }
}
