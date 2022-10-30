using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TastyTrading.DAL.Repositories;
using TastyTrading.Models;

namespace TastyTrading.Controllers
{
    /* Used to exclude a class or method from code coverage results. */
    [ExcludeFromCodeCoverage]

    /* A route template that is used to define the URL pattern that is used to 
     * reach the controller. */
    [Route("api/v1/[controller]/[action]")]
    public class TradingController : ControllerBase
    {
        /* A private variable that is used to access the database. */
        private readonly ITradingRepository _tradingDb;

        /* Used to log information to the console. */
        private readonly ILogger<TradingController> _log;

        /* A constructor that takes in two parameters. */
        public TradingController(ITradingRepository tradingDb, ILogger<TradingController> log)
        {
            _tradingDb = tradingDb;
            _log = log;
        }

        /* It gets the portfolio from the database and returns it */
        public async Task<ActionResult> GetPortfolio()
        {
            var list = await _tradingDb.GetPortfolio();

            if (list != null) return Ok(list);
            _log.LogInformation("Could not get portfolio");
            return NotFound("Could not get portfolio");
        }

        /* The function takes in a customer order, and returns the product that 
         * was bought */ 
        public async Task<ActionResult> BuyStock(Portfolio customerOrder)
        {
            var product = await _tradingDb.BuyStock(customerOrder);

            if (product == true) return Ok(product);
            _log.LogInformation("Product was not found");
            return NotFound("Product was not found");
        }

        /* It gets all the stocks from the database and returns them to the user */
        public async Task<ActionResult> GetStocks()
        {
            var list = await _tradingDb.GetStocks();

            if (list != null) return Ok(list);
            _log.LogInformation("Could not get all stocks");
            return NotFound("Could not get all stocks");
        }

        /* This function is used to sell a stock, and returning a boolean value. */
        public async Task<ActionResult> SellStock(int sellID)
        {

            var stock = await _tradingDb.SellStock(sellID);

            if (stock == true) return Ok(stock);
            _log.LogInformation("Stock was not deleted");
            return NotFound("Stock was not deleted");

        }

        /*  It gets one order from the database, and returns it */
        public async Task<ActionResult> GetOneOrder(int orderID)
        {
            var order = await _tradingDb.GetOneOrder(orderID);

            if (order != null) return Ok(order);
            _log.LogInformation("Could not get stock");
            return NotFound("Could not get stock");
        }

        /* This function updates the stock in the database */
        public async Task<ActionResult> UpdateBuyStock(Portfolio order)
        {

            var checkStock = await _tradingDb.UpdateBuyStock(order);

            if (checkStock == true) return Ok(checkStock);
            _log.LogInformation("Stock was not updated");
            return NotFound("Stock was not updated");

        }

        /* This function is used to update the stock quantity in the database 
         * when a user sells a stock */
        public async Task<ActionResult> UpdateSellStock(Portfolio order)
        {

            var checkStock = await _tradingDb.UpdateSellStock(order);

            if (checkStock == true) return Ok(checkStock);
            _log.LogInformation("Stock was not deleted");
            return NotFound("Stock was not deleted");

        }

        /* It gets all the transactions from the database and returns them to 
         * the user */
        public async Task<ActionResult> GetAllTransactions()
        {
            var transactions = await _tradingDb.GetAllTransactions();

            if (transactions != null) return Ok(transactions);
            _log.LogInformation("Could not get all transactions");
            return NotFound("Could not get all transactions");
        }

        /* It gets the user from the database and returns it if it exists, 
         * otherwise it logs an error and returns a 404 */ 
        public async Task<ActionResult> GetUser()
        {
            var person = await _tradingDb.GetUser();

            if (person != null) return Ok(person);
            _log.LogInformation("Could not find the user");
            return NotFound("Could not find the user");
        }
    }
}