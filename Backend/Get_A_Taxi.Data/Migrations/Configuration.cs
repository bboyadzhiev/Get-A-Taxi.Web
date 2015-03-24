namespace Get_A_Taxi.Data.Migrations
{
    using Get_A_Taxi.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Get_A_Taxi.Data.GetATaxiDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Get_A_Taxi.Data.GetATaxiDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            foreach (UserRoles role in Enum.GetValues(typeof(UserRoles)))
            {
                if (!context.Roles.Any(r => r.Name == role.ToString()))
                {
                    var store = new RoleStore<IdentityRole>(context);
                    var manager = new RoleManager<IdentityRole>(store);
                    var newRole = new IdentityRole { Name = role.ToString() };
                    manager.Create(newRole);
                }
            }

           

            //42.697652, 23.322036
            if (!context.Districts.Any(d=>d.Title == "Sofia"))
            {

                District sofia = new District { Title = "Sofia", CenterLattitude = 42.697652, CenterLongitude = 23.322036, MapZoom = 10 };
                context.Districts.AddOrUpdate(sofia);
                context.Taxies.AddOrUpdate(
               new Taxi { Plate = "CA 0001 AA", Seats = 5, Luggage = 2, District = sofia, Status = TaxiStatus.OffDuty },
               new Taxi { Plate = "CA 0002 AA", Seats = 5, Luggage = 2, District = sofia, Status = TaxiStatus.OffDuty }
                );
                context.SaveChanges();
            }

            if (!context.Users.Any(u => u.UserName == "bboyadzhiev@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = "bboyadzhiev@gmail.com",
                    FirstName = "Borislav",
                    MiddleName = "Vladimirov",
                    LastName = "Boyadzhiev",
                    Email = "bboyadzhiev@gmail.com",
                    PhoneNumber = "0886176803",
                    District = context.Districts.First(d=>d.Title == "Sofia")
                };

                manager.Create(user, "passW0RD");
                manager.AddToRole(user.Id, UserRoles.Administrator.ToString());
                manager.AddToRole(user.Id, UserRoles.Manager.ToString());
                manager.AddToRole(user.Id, UserRoles.Operator.ToString());
            }

            if (!context.Users.Any(u => u.UserName == "shisho@getataxi.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    UserName = "shisho@getataxi.com",
                    FirstName = "Shisho",
                    MiddleName = "D.",
                    LastName = "Bakshisho",
                    Email = "shisho@getataxi.com",
                    PhoneNumber = "0111222333",
                    District = context.Districts.First(d => d.Title == "Sofia")
                };

                manager.Create(user, "shisho");
                manager.AddToRole(user.Id, UserRoles.Driver.ToString());
            }
        }
    }
}
