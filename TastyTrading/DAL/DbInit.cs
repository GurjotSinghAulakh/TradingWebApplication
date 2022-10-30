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
            // legger til en service fra parameteren "app", og importerer og bruker dette
            // det gjør at vi har tilgang til variabler og metoder innenfor "app"
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                //Bruker DB, fra her har vi tilgang til Database og 
                var context = serviceScope.ServiceProvider.GetService<TradingDb>();

                //Sletter og opretter databasen
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // Adding stocks

                var msft = new Stock { Symbol = "msft", Name = "Microsoft", Price = 235.7, Volume = "5.9B" };
                var appl = new Stock { Symbol = "appl", Name = "Apple", Price = 155.5, Volume = "4.93B" };
                var tsla = new Stock { Symbol = "tsla", Name = "Tesla", Price = 229.8, Volume = "7.8B" };
                var sbux = new Stock { Symbol = "sbux", Name = "Starbucks", Price = 145.9, Volume = "3.5B" };
                var nke = new Stock { Symbol = "nke", Name = "Nike", Price = 93.8, Volume = "4.4B" };
                var goog = new Stock { Symbol = "goog", Name = "Google", Price = 96.5, Volume = "8.2B" };

                // var myportfolio = new Portfolio { Stock_Quantity = 200, Stock_Symbol = "msft", Stock_Name = "Microsoft", Stock_Price = 112.5, Portfolio_Balance = 120.0 };

                context.Stocks.Add(msft);
                context.Stocks.Add(appl);
                context.Stocks.Add(tsla);
                context.Stocks.Add(sbux);
                context.Stocks.Add(nke);
                context.Stocks.Add(goog);

                var user1 = new User { FirstName = "Per", LastName = "Hansen", StreetAddress = "Asker Veien 24", PostalCode = "1234", Postallocation = "Asker", Phone = "12345678", Email = "s123456@oslomet.no" };
                var user2 = new User { FirstName = "Per", LastName = "Hansen", StreetAddress = "Asker Veien 24", PostalCode = "1234", Postallocation = "Asker", Phone = "12345678", Email = "s123456@oslomet.no" };

                context.Users.Add(user1);
                context.Users.Add(user2);

                //var newPortfolio = new Portfolio { Id = 1, Name = "Gurjot", PersonId = 1, Price = 123.0, Stock = msft, Symbol = "msft" };

                //context.Portfolios.Add(newPortfolio);

                //context.Portfolios.Add(myportfolio);

                context.SaveChanges();
            }
        }
    }
}
