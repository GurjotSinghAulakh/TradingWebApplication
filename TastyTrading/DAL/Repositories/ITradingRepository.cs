using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using TastyTrading.Models;

namespace TastyTrading.DAL.Repositories
{
    /* An interface. */
    public interface ITradingRepository
    {
        Task<List<Portfolio>> GetPortfolio();
        Task<bool> BuyStock(Portfolio customerOrder);
        Task<List<Stock>> GetStocks();
        Task<bool> SellStock(int sellID);
        Task<Portfolio> GetOneOrder(int orderID);
        Task<bool> UpdateBuyStock(Portfolio order);
        Task<bool> UpdateSellStock(Portfolio order);
        Task<List<Transaction>> GetAllTransactions();
        Task<User> GetUser();
    }
}