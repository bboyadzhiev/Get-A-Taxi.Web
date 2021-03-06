namespace Get_A_Taxi.Data.Migrations
{
    using Get_A_Taxi.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<GetATaxiDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(GetATaxiDbContext context)
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

                District sofia = new District { Title = "Sofia", CenterLatitude = 42.697652, CenterLongitude = 23.322036, MapZoom = 10 };
                context.Districts.AddOrUpdate(sofia);
                // two taxies near Orlov most, sofia
                context.Taxies.AddOrUpdate(
               new Taxi { Plate = "CA 0001 AA", Seats = 5, Luggage = 2, District = sofia, Status = TaxiStatus.Unassigned, Latitude = 42.690284, Longitude = 23.338264 },
               new Taxi { Plate = "CA 0002 AA", Seats = 5, Luggage = 2, District = sofia, Status = TaxiStatus.Unassigned, Latitude = 42.690456, Longitude = 23.338730 }
                );

                context.Stands.AddOrUpdate(
                    new TaxiStand { Active = true, Alias = "Hotel Pliska", Latitude = 42.675604, Longitude = 23.359076, Address = "boulevard Tsarigradsko shose 83", District = sofia },
                    new TaxiStand { Active = true, Alias = "Terminal 2", Latitude = 42.688151, Longitude = 23.413509, Address = "Sofia Airport Terminal 2", District = sofia },
                    new TaxiStand { Active = true, Alias = "Central Railway Station Sofia", Latitude = 42.711869, Longitude = 23.320702, Address = "bulevard Kniaginya Maria Luiza 102A", District = sofia },
                    new TaxiStand { Active = true, Alias = "Studentski Grad", Latitude = 42.651421, Longitude = 23.342664, Address = "ul. 8-mi dekemvri 9 1700 Sofia", District = sofia }
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

            if (!context.Users.Any(u => u.UserName == "jack@getataxi.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);

                var demoManager = new ApplicationUser
                {
                    UserName = "jack@getataxi.com",
                    FirstName = "Jasper",
                    MiddleName = "Newton",
                    LastName = "Daniel",
                    Email = "jack@getataxi.com",
                    PhoneNumber = "0888333111",
                    District = context.Districts.First(d => d.Title == "Sofia")
                };

                manager.Create(demoManager, "JackDaniels");
                manager.AddToRole(demoManager.Id, UserRoles.Manager.ToString());
            }

            if (!context.Users.Any(u => u.UserName == "shisho@getataxi.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);

                var demoDriver = new ApplicationUser
                {
                    UserName = "shisho@getataxi.com",
                    FirstName = "Shisho",
                    MiddleName = "D.",
                    LastName = "Bakshisho",
                    Email = "shisho@getataxi.com",
                    PhoneNumber = "0888000111",
                    District = context.Districts.First(d => d.Title == "Sofia")
                };

                manager.Create(demoDriver, "shisho");
                manager.AddToRole(demoDriver.Id, UserRoles.Driver.ToString());
            }

            if (!context.Users.Any(u => u.UserName == "maryjane@getataxi.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var demoOperator1 = new ApplicationUser
                {
                    UserName = "maryjane@getataxi.com",
                    FirstName = "Mary",
                    MiddleName = "Jane",
                    LastName = "Watson",
                    Email = "maryjane@getataxi.com",
                    PhoneNumber = "0888222555",
                    District = context.Districts.First(d => d.Title == "Sofia")
                };

                manager.Create(demoOperator1, "maryjane");
                manager.AddToRole(demoOperator1.Id, UserRoles.Operator.ToString());
            }

            if (!context.Users.Any(u => u.UserName == "charlie@getataxi.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);

                var demoOperator2 = new ApplicationUser
                {
                    UserName = "charlie@getataxi.com",
                    FirstName = "Charlie",
                    MiddleName = "Donathan",
                    LastName = "Doe",
                    Email = "charlie@getataxi.com",
                    PhoneNumber = "0888222666",
                    District = context.Districts.First(d => d.Title == "Sofia")
                };

                manager.Create(demoOperator2, "charlie");
                manager.AddToRole(demoOperator2.Id, UserRoles.Operator.ToString());
            }
        }
    }
}
