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
    /* A way to tell the code coverage tool to ignore this class. */
    [ExcludeFromCodeCoverage]

    public class TradingRepository : ITradingRepository
    {

        /* A private variable that is used to access the database. */
        private readonly TradingDb _tradingDb;

        /* Used to log information to the console. */
        private readonly ILogger<TradingRepository> _log;

        /* This is the constructor for the class. It is used to create an 
         * instance of the class. */
        public TradingRepository(TradingDb tradingDb, ILogger<TradingRepository> log)
        {
            _tradingDb = tradingDb;
            _log = log;
        }

       /* It returns a list of portfolios from the database */
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

        
        /* It takes a customer order, creates a new order, saves it to the 
         * database, and then creates a */
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

                await CreateTransaction("Buy", newOrder.Id, newOrder, customerOrder.Quantity);
                return true;
            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Purchase is not successful!!");
                return false;
            }
        }

        /* It returns a list of all the stocks in the database */ 
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

        // TODO : needs code comments
        public async Task<bool> SellStock(int sellID)
        {
            try
            {
                Console.WriteLine("ID i backend er " + sellID);
                Portfolio order = await _tradingDb.Portfolios.FindAsync(sellID);

                _tradingDb.Portfolios.Remove(order);

                await _tradingDb.SaveChangesAsync();

                await CreateTransaction("Sell", sellID, order, order.Quantity);
                return true;
            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        /* It returns a portfolio object from the database based on the orderID */
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
        
        /* The function takes in a Portfolio object, and updates the quantity 
         * of the stock in the database */
        public async Task<bool> UpdateBuyStock(Portfolio order)
        {
            try
            {
                var myStock = await _tradingDb.Portfolios.FindAsync(order.Id);


                myStock.Quantity += order.Quantity;

                await _tradingDb.SaveChangesAsync();
                await CreateTransaction("Buy", order.Id, myStock, order.Quantity);

            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "The shares have not been updated!");
                return false;
            }
            return true;
        }

        /* It updates the quantity of a stock in the portfolio */
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
                        await CreateTransaction("Sell", order.Id, myStock, order.Quantity);
                        return true;
                    }

                    return false;
                }
                return false;
            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message + "The shares cannot be sold!");
                return false;
            }
        }

        // TODO : needs code comments
        public Task<bool> CreateTransaction(string status, int id, Portfolio order, double quantity)
        {
            try
            {
                DateTime newDateTime = DateTime.Now;

                var newTransaction = new Transaction();
                newTransaction.TotalPrice = order.Price * quantity;
                newTransaction.Status = status;
                newTransaction.Symbol = order.Symbol;
                newTransaction.Name = order.Name;
                newTransaction.CreatedAt = newDateTime;
                newTransaction.OrderID = id;
                newTransaction.Price = order.Price;
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

        /* It returns a list of transactions, each transaction containing the 
         * stock name, price, */
        public async Task<List<Transaction>> GetAllTransactions()
        {
            try
            {
                return await _tradingDb.Transactions.ToListAsync();
            }
            catch
            {
                return null;
            }
        }

        /* It gets a user from the database and returns it */
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