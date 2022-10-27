using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TastyTrading.Models;

namespace TastyTrading.DAL.Repositories
{
    public interface ITradingRepository
    {
        Task<List<Stock>> GetStocks();
        Task<Stock> GetOneStock(int id);
        Task<bool> SaveStock(Portfolio mystock);
        //Task<bool> UpdateStock(Buy mystock);
        Task<bool> UpdateBalance(Portfolio portfolio);
        // Task<bool> DeleteStock(int id);
        Task<List<Portfolio>> GetPortfolio(int PId);
        Task<bool> BuyStock(Order order);
        Task<bool> SellStock(Portfolio sellOrder);
        Task<List<Transaction>> GetAllTransactions();
        Task<List<Transaction>> GetTransactions(DateTime fromDate, DateTime toDate);


        /*
        Task<bool> SaveCustomer(Customer frontCustomer);
        Task<bool> SaveCustomers(List<Customer> frontCustomers); // trenger ikke 
        Task<Customer> GetCustomer(string reference);
        Task<List<Customer>> GetCustomers();
        Task<bool> UpdateCustomer(Customer customer);
        Task<bool> DeleteCustomer(string reference);
        Task<Cabin> GetCabin(int id);  // trenger ikke 
        Task<List<Cabin>> GetCabins();
        Task<List<Cabin>> GetCabinUnoccupied(); // trenger ikke
        Task<Route> GetRoute(int id);
        Task<List<Route>> GetRoutes();
        Task<List<Ticket>> GetTickets();
        Task<PostalCode> GetPostalCode(string code);
        Task<List<Customer>> GetCustomersByReferences(IEnumerable<string> references);
        Task<string> GenerateReference(string firstname, string lastname);
        Task<List<Departure>> GetDeparturesByDateAndRoute(string date, int routeId); // trenger ikke
        Task<List<Departure>> GetDepartures(); // trenger ikke
        Task<Departure> GetDeparture(int id);  // trenger ikke
        double GeneratePrice(IEnumerable<Cabin> cabins); // trenger ikke
        bool PaymentCheck(Payment payment); // trenger ikke
        */
    }
}