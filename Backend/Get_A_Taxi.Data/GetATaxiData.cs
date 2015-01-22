using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Get_A_Taxi.Data.Repositories;
using Get_A_Taxi.Models;

namespace Get_A_Taxi.Data
{
    public class GetATaxiData : IGetATaxiData
    {
        private readonly IGetATaxiDbContext context;
        private readonly IDictionary<Type, object> repositories;

        //public GetATaxiData()
        //    :this(new GetATaxiDbContext())
        //{

        //}
        //public GetATaxiData(string connectionString)
        //    :this(new GetATaxiDbContext(connectionString))
        //{

        //}

        public GetATaxiData(IGetATaxiDbContext ctx)
        {
            this.context = ctx;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<ApplicationUser> Users
        {
            get
            {
                return this.GetRepository<ApplicationUser>();
            }
        }

        public IRepository<Taxi> Taxies
        {
            get
            {
                return this.GetRepository<Taxi>();
            }
        }

        public IRepository<TaxiStand> Stands
        {
            get
            {
                return this.GetRepository<TaxiStand>();
            }
        }

        public IRepository<Order> Orders
        {
            get
            {
                return this.GetRepository<Order>();
            }
        }

        public IRepository<Photo> Photos
        {
            get
            {
                return this.GetRepository<Photo>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }
        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(Repository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }
            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}
