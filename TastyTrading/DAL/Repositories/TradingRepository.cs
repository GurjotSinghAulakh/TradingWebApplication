using System;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using TastyTrading.Controllers;
using TastyTrading.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq;

namespace TastyTrading.DAL.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TradingRepository : ITradingRepository
    {

        private readonly TradingDb _tradingDb;

        private readonly ILogger<TradingRepository> _log;

        public TradingRepository(TradingDb tradingDb, ILogger<TradingRepository> log)
        {
            _tradingDb = tradingDb;
            _log = log;
        }

        public async Task<List<Portfolio>> GetPortfolio()
        {
            try
            {
                return await _tradingDb.Portfolios.ToListAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<bool> BuyStock(Portfolio customerOrder)
        {
            try
            {

                var newOrder = new Portfolio();
                newOrder.Symbol = customerOrder.Stock.Symbol;
                newOrder.Name = customerOrder.Stock.Name;
                newOrder.Price = customerOrder.Stock.Price;
                newOrder.Quantity = customerOrder.Quantity;
                newOrder.PersonId = 1;

                // TODO : Lage en transaksjon
                // await CreateTransaction("Buy", customerOrder.Id, newOrder, customerOrder.Quantity);

                _tradingDb.Portfolios.Add(newOrder);
                await _tradingDb.SaveChangesAsync();
                return true;
            }


            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Kjøp er ikke vellykket!! ");
                return false;
            }
        }
    }
}