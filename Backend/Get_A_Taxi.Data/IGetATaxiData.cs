namespace Get_A_Taxi.Data
{
    using System;
    using System.Collections.Generic;
    using Get_A_Taxi.Data.Repositories;
    using Get_A_Taxi.Models;

    public interface IGetATaxiData
    {
        IRepository<ApplicationUser> Users { get; }
        IRepository<Taxi> Taxies { get; }
        IRepository<TaxiStand> Stands { get; }
        IRepository<Order> Orders { get; }
        IRepository<Photo> Photos { get; }
        int SaveChanges();
    }
}
