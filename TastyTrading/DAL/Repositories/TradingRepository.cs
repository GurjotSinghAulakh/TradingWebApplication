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

                _tradingDb.Portfolios.Add(newOrder);
                await _tradingDb.SaveChangesAsync();

                Console.WriteLine(newOrder.Id);
                Console.WriteLine(newOrder);
                Console.WriteLine(customerOrder.Quantity);

                // TODO : Lage en transaksjon
                await CreateTransaction("Buy", newOrder.Id, newOrder, customerOrder.Quantity);
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

        public async Task<bool> UpdateBuyStock(Portfolio order)
        {
            try
            {
                var myStock = await _tradingDb.Portfolios.FindAsync(order.Id);


                myStock.Quantity += order.Quantity;

                await _tradingDb.SaveChangesAsync();
            }


            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Aksjene er ikke oppdatert ");
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateSellStock(Portfolio order)
        {
            try
            {
                var myStock = await _tradingDb.Portfolios.FindAsync(order.Id);

                if (order.Quantity > 0)
                {
                    if (myStock.Quantity == order.Quantity)
                    {
                        return await SellStock(order.Id);
                    }

                    if (myStock.Quantity > order.Quantity)
                    {
                        myStock.Quantity -= order.Quantity;
                        await _tradingDb.SaveChangesAsync();
                        return true;
                    }

                    return false;
                }
                return false;
            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Aksjene kan ikke selges ");
                return false;
            }
        }

        public Task<bool> CreateTransaction(string status, int id, Portfolio order, double quantity)
        {
            try
            {
                DateTime newDateTime = new DateTime();

                var newTransaction = new Transaction();
                // ewTransaction.TotalPrice = portfolio.Stock.Price * portfolio.Quantity;
                newTransaction.Status = status;
                newTransaction.Symbol = order.Symbol;
                newTransaction.Name = order.Name;
                newTransaction.CreatedAt = newDateTime;
                newTransaction.OrderID = id;
                newTransaction.Stock = order.Stock;
                // newTransaction.User = order.User;
                newTransaction.Quantity = quantity;

                _tradingDb.Transactions.Add(newTransaction);

                _tradingDb.SaveChangesAsync();
                return Task.FromResult(true);

            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Kan ikke lage en transaksjon!!");
                return Task.FromResult(false);
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
                   // UserID = t.User.Id
                }).ToListAsync();
                return transactions;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User> GetUser()
        {
            try
            {
                User newUser = await _tradingDb.Users.FindAsync(1);
                var getNewUser = new User()
                {
                    Id = newUser.Id,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    StreetAddress = newUser.StreetAddress,
                    PostalCode = newUser.PostalCode,
                    Postallocation = newUser.Postallocation,
                    Phone = newUser.Phone,
                    Email = newUser.Email
                };

                return getNewUser;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }
    }
}