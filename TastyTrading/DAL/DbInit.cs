using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TastyTrading.Models;

namespace TastyTrading.DAL
{
    public static class DbInit
    {
        [ExcludeFromCodeCoverage]
        public static void Initialize(IApplicationBuilder app)
        {
            // Adds a service from the "app" parameter, and imports and uses this, 
            // It allows us to access variables and methods within "app"
            
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                // Using DB, from here we have access to the Database
                var context = serviceScope.ServiceProvider.GetService<TradingDb>();

                // It deletes and creates the database. 
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // Creating stocks 
                var msft = new Stock { Symbol = "msft", Name = "Microsoft", Price = 235.7, Volume = "5.9B" };
                var appl = new Stock { Symbol = "appl", Name = "Apple", Price = 155.5, Volume = "4.93B" };
                var tsla = new Stock { Symbol = "tsla", Name = "Tesla", Price = 229.8, Volume = "7.8B" };
                var sbux = new Stock { Symbol = "sbux", Name = "Starbucks", Price = 145.9, Volume = "3.5B" };
                var nke = new Stock { Symbol = "nke", Name = "Nike", Price = 93.8, Volume = "4.4B" };
                var goog = new Stock { Symbol = "goog", Name = "Google", Price = 96.5, Volume = "8.2B" };


                // Adding the stocks to the database. 
                context.Stocks.Add(msft);
                context.Stocks.Add(appl);
                context.Stocks.Add(tsla);
                context.Stocks.Add(sbux);
                context.Stocks.Add(nke);
                context.Stocks.Add(goog);

                // It creates a new user with the given information. 
                var user1 = new User { FirstName = "Per", LastName = "Hansen", StreetAddress = "Asker Veien 24", 
                PostalCode = "1234", Postallocation = "Asker", Phone = "12345678", Email = "s123456@oslomet.no" };

                // It adds the user to the database. 
                context.Users.Add(user1);

                // It saves the changes made to the database. 
                context.SaveChanges();
            }
        }
    }
}
