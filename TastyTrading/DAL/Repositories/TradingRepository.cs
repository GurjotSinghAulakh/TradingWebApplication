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

        public async Task<List<Stock>> GetStocks()
        {
            try
            {
                List<Stock> allStocks = await _tradingDb.Stocks.Select(S => new Stock
                {
                    Id = S.Id,
                    Symbol = S.Symbol,
                    Name = S.Name,
                    Price = S.Price,
                    Volume = S.Volume

                }).ToListAsync();

                return allStocks;
            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<bool> SellStock(int sellID)
        {
            try
            {
                Console.WriteLine("ID i backend er " + sellID);
                Portfolio order = await _tradingDb.Portfolios.FindAsync(sellID);

                _tradingDb.Portfolios.Remove(order);

                // TODO : Lage en transaksjon
                // await CreateTransaction("Sell", sellID, order, order.Quantity);

                await _tradingDb.SaveChangesAsync();
                return true;
            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<Portfolio> GetOneOrder(int orderID)
        {
            try
            {
                return await _tradingDb.Portfolios.FindAsync(orderID);
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<bool> UpdateBuyStock(Portfolio order, int orderID)
        {
            try
            {
                var myStock = await _tradingDb.Portfolios.FindAsync(orderID);


                //myStock.Quantity += order.Quantity;

                var newOrder = new Portfolio();
                newOrder.Symbol = order.Stock.Symbol;
                newOrder.Name = order.Stock.Name;
                newOrder.Price = order.Stock.Price;
                newOrder.Quantity += order.Quantity;
                newOrder.PersonId = 1;

                _tradingDb.Portfolios.Update(newOrder);

                await _tradingDb.SaveChangesAsync();
                return true;
            }


            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Aksjene er ikke oppdatert ");
                return false;
            }
        }

        public async Task<bool> UpdateSellStock(Portfolio order, int orderID)
        {
            try
            {
                Portfolio[] checkStock = _tradingDb.Portfolios.Where(p => p.Stock.Id == orderID).ToArray();

                if (checkStock[0].Quantity > order.Quantity && order.Quantity != 0)
                {
                    checkStock[0].Quantity -= order.Quantity;
                    await _tradingDb.SaveChangesAsync();
                    return true;
                }

                if (checkStock[0].Quantity == order.Quantity && order.Quantity != 0)
                {
                    _tradingDb.Portfolios.Remove(checkStock[0]);
                    await _tradingDb.SaveChangesAsync();
                    return true;
                }

                return false;
            }


            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Aksjene kan ikke selges ");
                return false;
            }
        }

        public async Task<bool> CreateTransaction(string status, int id, Portfolio portfolio, double quantity)
        {
            try
            {

                DateTime newDateTime = new DateTime();

                var newTransaction = new Transaction();
                //newTransaction.TotalPrice = portfolio.Stock.Price * portfolio.Quantity;
                newTransaction.Status = status;
                newTransaction.Symbol = portfolio.Symbol;
                newTransaction.Name = portfolio.Name;


                /*
                newTransaction.CreatedAt = newDateTime;
                newTransaction.OrderID = portfolio.Id;
                newTransaction.Stock = portfolio.Stock;
                newTransaction.User = portfolio.User;
                newTransaction.Quantity = quantity;*/

                _tradingDb.Transactions.Add(newTransaction);

                await _tradingDb.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Kan ikke lage en transaksjon!!");
                return false;
            }
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            try
            {
                List<Transaction> transactions = await _tradingDb.Transactions.Select(t => new Transaction
                {
                    Id = t.Id,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt,
                    Quantity = t.Quantity,
                    OrderID = t.OrderID,
                    Name = t.Stock.Name,
                    Price = t.Stock.Price,
                    UserID = t.User.Id
                }).ToListAsync();
                return transactions;
            }
            catch
            {
                return null;
            }
        }
    }
}