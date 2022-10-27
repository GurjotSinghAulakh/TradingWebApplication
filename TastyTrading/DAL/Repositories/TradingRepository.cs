using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TastyTrading.DAL.Repositories;
using TastyTrading.Models;
using TastyTrading.DAL.Utilities;
using System.Collections;
using System.Security.Cryptography;

namespace TastyTrading.DAL.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TradingRepository : ITradingRepository
    {
        private readonly TradingDB _tradingDb;

        private readonly ILogger<TradingRepository> _log;

        public TradingRepository(TradingDB tradingDb, ILogger<TradingRepository> log)
        {
            _tradingDb = tradingDb;
            _log = log;
        }

        public async Task<bool> BuyStock(Order order)
        {
            try
            {
                var stock = await _tradingDb.Stocks.FindAsync(order.Stock.Id);

                var orderIDGenerator = Utility.CreateUniqueString();
                var newOrder = new Order();
                DateTime now = DateTime.Now;

                newOrder.Quantity = order.Quantity;
                newOrder.Created_At = now;
                newOrder.BuyID = orderIDGenerator;

                if (order.Quantity > 0 && order.Portfolio.Portfolio_Balance > 0)
                {
                    // Updating the balance in the portfolio
                    await UpdateBalance(order.Portfolio);
                    _tradingDb.Orders.Add(newOrder);
                    await _tradingDb.SaveChangesAsync();
                    return true;
                }

                else
                {
                    return false;
                }
            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }


        public async Task<List<Transaction>> GetAllTransactions()
        {
            try
            {
                return await _tradingDb.Transactions.ToListAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        /*-------------- http://localhost:3275/api/v1/Trading/GetOneStock?id=1 --------------*/

        public async Task<Stock> GetOneStock(int id)
        {
            try
            {
                return await _tradingDb.Stocks.FindAsync(id);
            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        /*------------- http://localhost:3275/api/v1/Trading/GetPortfolio?PId=1 ---------------*/


        public async Task<List<Portfolio>> GetPortfolio(int PId)
        {
            try
            {
                List<Portfolio> portfolios = await _tradingDb.Portfolios.Where(S => S.Portfolio_Id == PId).ToListAsync();

                return portfolios;
            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        /*----------- http://localhost:3275/api/v1/Trading/GetStocks -------------*/

        public async Task<List<Stock>> GetStocks()
        {
            try
            {
                List<Stock> allStocks = await _tradingDb.Stocks.Select(S => new Stock
                {
                    Id = S.Id,
                    Symbol = S.Symbol,
                    Product_name = S.Product_name,
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

        public async Task<List<Transaction>> GetTransactions(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var dbTransactions = await _tradingDb.Transactions.Where(
                     d => (fromDate == null || d.FromDate == fromDate)
                     && (toDate == null || d.ToDate == toDate)).ToListAsync();


                if (dbTransactions is null) return null;

                return dbTransactions;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<bool> SaveStock(Portfolio mystock)
        {
            try
            {
                _tradingDb.Portfolios.Add(mystock);

                await _tradingDb.SaveChangesAsync();

                return true;
            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<bool> SellStock(Portfolio sellOrder)
        {
            try {
                Portfolio order = await _tradingDb.Portfolios.FindAsync(sellOrder.GetStock().Id);

                if (order.Stock_Quantity >= sellOrder.Stock_Quantity)
                {
                    _tradingDb.Portfolios.Remove(order);
                    await _tradingDb.SaveChangesAsync();
                    return true;

                }
                else
                {
                    return false;
                }
            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<bool> UpdateBalance(Portfolio portfolio)
        {
            try
            {
                Portfolio newBalance = new Portfolio();
                newBalance.Stock_Quantity = portfolio.Stock_Quantity;
                newBalance.Stock_Symbol = portfolio.Stock_Symbol;
                newBalance.Stock_Name = portfolio.Stock_Name;
                newBalance.Stock_Price = portfolio.Stock_Price;
                newBalance.Portfolio_Balance = portfolio.Portfolio_Balance;
                _tradingDb.Portfolios.Update(newBalance);
                await _tradingDb.SaveChangesAsync();
                return true;
            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }
    }
}



        /**
         * Method that tries to take inn a customer and their proposed ticket, so that it can be saved to the DB
         * Makes a new customer if there is none, and then appends the ticket to their customer list, then saves to DB
         
        public async Task<bool> SaveCustomer(Customer customer)
        {
            try
            {
                // Testing if the customer is already in the DB on id first, and then name if ID fails
                // (assuming things behind OR is never run when first passes, like in java)
                var dbCustomer = await _boatLineDb.Customers.FirstOrDefaultAsync(c =>
                    c.Reference == customer.Reference ||
                    (c.FirstName == customer.FirstName && c.LastName == customer.LastName));

                // Setting customer ticket's sub values to be tied to the DB
                for (var i = 0; i < customer.Tickets.Count; i++)
                {
                    // Setting route
                    customer.Tickets[i].Departure =
                        await _boatLineDb.Departures.FirstOrDefaultAsync(d => d.Id == customer.Tickets[i].Departure.Id);

                    // Setting cabin
                    var newCabinHash = new Collection<Cabin>();

                    foreach (var cabin in customer.Tickets[i].Cabins)
                    {
                        newCabinHash.Add(await _boatLineDb.Cabins.FirstOrDefaultAsync(c => c.Id == cabin.Id));
                    }

                    customer.Tickets[i].Cabins = newCabinHash;
                }


                // If customer does exist in the DB
                if (dbCustomer is not null)
                {
                    // Adds all of the frontend customers tickets onto the dbCustomers list
                    for (var i = 0; i < customer.Tickets.Count; i++)
                    {
                        dbCustomer.Tickets.Add(customer.Tickets[i]);
                    }
                }
                else // If The customer does not exist in the DB, adding the customer with the tickets automatically.
                {
                    // fixing that the postal code info is sett right
                    customer.PostalCode =
                        await _boatLineDb.PostalCodes.FirstOrDefaultAsync(p =>
                            p.Code.Equals(customer.PostalCode.Code));

                    await _boatLineDb.Customers.AddAsync(customer);
                }

                await _boatLineDb.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        /**
         * Method for saving multiple customers
         
        public async Task<bool> SaveCustomers(List<Customer> customers)
        {
            try
            {
                var holder = false;
                foreach (var c in customers)
                {
                    holder = await SaveCustomer(c);
                }

                return holder;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        /**
         * Method for receiving customer by id
         
        public async Task<Customer> GetCustomer(string reference)
        {
            try
            {
                return await _boatLineDb.Customers.FirstOrDefaultAsync(c => c.Reference == reference);
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        /**
         * Method that tries to to get all the customers, so that the customers and their lists of
         * tickets can be formatted into a table of customers and tickets.
         
        public async Task<List<Customer>> GetCustomers()
        {
            try
            {
                return await _boatLineDb.Customers.ToListAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        /**
         * Method for updating customer.
         
        public async Task<bool> UpdateCustomer(Customer customer)
        {
            try
            {
                var updateCustomer =
                    await _boatLineDb.Customers.FirstOrDefaultAsync(c => c.Reference == customer.Reference);

                if (updateCustomer.PostalCode.Code != customer.PostalCode.Code)
                {
                    var testCode =
                        await _boatLineDb.PostalCodes.FirstOrDefaultAsync(p => p.Code == customer.PostalCode.Code);
                    if (testCode == null)
                    {
                        var postalCode = new PostalCode()
                        {
                            Code = customer.PostalCode.Code,
                            Name = customer.PostalCode.Name
                        };
                        updateCustomer.PostalCode = postalCode;
                    }
                    else
                    {
                        updateCustomer.PostalCode.Code = customer.PostalCode.Code;
                    }
                }

                updateCustomer.FirstName = customer.FirstName;
                updateCustomer.LastName = customer.LastName;
                updateCustomer.StreetAddress = customer.StreetAddress;
                updateCustomer.Email = customer.Email;
                updateCustomer.Phone = customer.Phone;

                await _boatLineDb.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }

            return true;
        }

        /**
         * Method that deletes a specific customer from id in database
         
        public async Task<bool> DeleteCustomer(string reference)
        {
            try
            {
                var customer = await _boatLineDb.Customers.FirstOrDefaultAsync(c => c.Reference == reference);
                _boatLineDb.Customers.Remove(customer);
                await _boatLineDb.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /**
         * Method that gets a specific cabin from id in database
         
        public async Task<Cabin> GetCabin(int id)
        {
            try
            {
                return await _boatLineDb.Cabins.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        /**
         * Method that gets all cabins in database
         
        public async Task<List<Cabin>> GetCabins()
        {
            try
            {
                return await _boatLineDb.Cabins.ToListAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        /**
         * Method that gets a specific route from id in database
         
        public async Task<List<Cabin>> GetCabinUnoccupied()
        {
            try
            {
                return await _boatLineDb.Cabins.Where(c => c.Tickets.Count == 0).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /**
         * Method that gets a specific route from id in database
         
        public async Task<Route> GetRoute(int id)
        {
            try
            {
                return await _boatLineDb.Routes.FirstOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        /**
         * Method that gets all routes in database
         
        public async Task<List<Route>> GetRoutes()
        {
            try
            {
                return await _boatLineDb.Routes.ToListAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        /**
         * Method that gets all tickets in database
         
        public async Task<List<Ticket>> GetTickets()
        {
            try
            {
                return await _boatLineDb.Tickets.ToListAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        /**
         * Method that returns postalcode object whit given code as parameter
         
        public async Task<PostalCode> GetPostalCode(string code)
        {
            try
            {
                return await _boatLineDb.PostalCodes.FirstOrDefaultAsync(p => p.Code.Equals(code));
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        /**
         * Method that returns list of customers matching reference numbers.
         * Reference number consists of 8 digits and the first 4 identify customer
         * Using substring to get first 4 digits
         
        public async Task<List<Customer>> GetCustomersByReferences(IEnumerable<string> references)
        {
            try
            {
                var list = new List<Customer>();
                foreach (var reference in references)
                {
                    var customer = await _boatLineDb.Customers.FirstOrDefaultAsync(t =>
                        t.Reference == reference.Substring(0, 4));

                    if (customer is not null) list.Add(customer);
                }

                if (list.Count > 0) return list;
                _log.LogInformation("Did not find anny tickets by reference");
                return null;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        /**
         * Method for generating reference numbers for customer and ticket
         * Customer and Ticket reference number consist of 4 digits each
         
        public async Task<string> GenerateReference(string firstname, string lastname)
        {
            try
            {
                var dbCustomer = await _boatLineDb.Customers.FirstOrDefaultAsync(c =>
                    c.FirstName == firstname && c.LastName == lastname);

                if (dbCustomer is null) return Utility.GetRandomHexNumber(8);
                dbCustomer.Reference += Utility.GetRandomHexNumber(4);

                return dbCustomer.Reference;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<List<Departure>> GetDeparturesByDateAndRoute(string date, int routeId)
        {
            try
            {
                return await _boatLineDb.Departures.Where(d => (date == null || d.Date == date) && (routeId == 0 || d.Route.Id == routeId)).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        
        public async Task<List<Departure>> GetDepartures()
        {
            try
            {
                return await _boatLineDb.Departures.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        
        public async Task<Departure> GetDeparture(int id)
        {
            try
            {
                var dbDeparture = await _boatLineDb.Departures.FirstOrDefaultAsync(d => d.Id == id);
                if (dbDeparture is null) return null;
                else return dbDeparture;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /**
         * Method that generates price from cabin price
         
        public double GeneratePrice(IEnumerable<Cabin> cabins)
        {
            return cabins.Sum(cabin => cabin.Price);
        }

        /**
         * Just a pseudo method to act as actual payment verification
        
        public bool PaymentCheck(Payment payment)
        {
            return payment != null;
        }
    }
}
        */