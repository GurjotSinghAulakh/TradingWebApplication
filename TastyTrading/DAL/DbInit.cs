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
                var context = serviceScope.ServiceProvider.GetService<TradingDB>();

                //Sletter og opretter databasen
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // Adding stocks

                var msft = new Stock { Symbol = "msft", Product_name = "Microsoft", Price = 145.9, Volume = 5.9 };
                var appl = new Stock { Symbol = "appl", Product_name = "Apple", Price = 145.9, Volume = 5.9 };
                var tsla = new Stock { Symbol = "tsla", Product_name = "Tesla", Price = 145.9, Volume = 5.9 };
                var sbux = new Stock { Symbol = "sbux", Product_name = "Starbucks", Price = 145.9, Volume = 5.9 };
                var nke = new Stock { Symbol = "nke", Product_name = "Nike", Price = 145.9, Volume = 5.9 };
                var goog = new Stock { Symbol = "goog", Product_name = "Google", Price = 145.9, Volume = 5.9 };

                var myportfolio = new Portfolio { Stock_Quantity = 200, Stock_Symbol = "msft", Stock_Name = "Microsoft", Stock_Price = 112.5, Portfolio_Balance = 120.0 };

                context.Stocks.Add(msft);
                context.Stocks.Add(appl);
                context.Stocks.Add(tsla);
                context.Stocks.Add(sbux);
                context.Stocks.Add(nke);
                context.Stocks.Add(goog);

                context.Portfolios.Add(myportfolio);

                context.SaveChanges();
            }
        }
    }
}