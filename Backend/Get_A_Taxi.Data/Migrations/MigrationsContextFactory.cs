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
            //return new GetATaxiDbContext("GetATaxi");
            //return new GetATaxiDbContext();
            return GetATaxiDbContext.GetInstance();
        }
    }
}
