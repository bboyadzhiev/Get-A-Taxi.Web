using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Get_A_Taxi.Models;

namespace Get_A_Taxi.Data
{
    public interface IGetATaxiDbContext
    {
        IDbSet<ApplicationUser> Users { get; set; }
        IDbSet<Taxi> Taxies { get; set; }
        IDbSet<Order> Orders { get; set; }
        IDbSet<TaxiStand> Stands { get; set; }
        IDbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
    }
}
