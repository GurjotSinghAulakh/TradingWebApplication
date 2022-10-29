using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TastyTrading.DAL.Repositories;
using TastyTrading.Models;

namespace TastyTrading.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/v1/[controller]/[action]")]
    public class TradingController : ControllerBase
    {
        private readonly ITradingRepository _tradingDb;
        private readonly ILogger<TradingController> _log;

        public TradingController(ITradingRepository tradingDb, ILogger<TradingController> log)
        {
            _tradingDb = tradingDb;
            _log = log;
        }

        public async Task<ActionResult> GetPortfolio()
        {
            var list = await _tradingDb.GetPortfolio();

            if (list != null) return Ok(list);
            _log.LogInformation("Could not get all routes");
            return NotFound("Could not get all routes");
        }

        public async Task<ActionResult> BuyStock(Portfolio customerOrder)
        {
            var product = await _tradingDb.BuyStock(customerOrder);

            if (product != null) return Ok(product);
            _log.LogInformation("Product was not found");
            return NotFound("Product was not found");
        }

        public async Task<ActionResult> GetStocks()
        {
            var list = await _tradingDb.GetStocks();

            if (list != null) return Ok(list);
            _log.LogInformation("Could not get all stocks");
            return NotFound("Could not get all stocks");
        }

        public async Task<bool> SellStock(int sellID)
        {

            var stock = await _tradingDb.SellStock(sellID);

            if (stock) return Ok(stock);
            _log.LogInformation("Stock was not deleted");
            return NotFound("Stock was not deleted");

        }

        public async Task<ActionResult> GetOneOrder(int orderID)
        {
            var order = await _tradingDb.GetOneOrder(orderID);

            if (order != null) return Ok(order);
            _log.LogInformation("Could not get all routes");
            return NotFound("Could not get all routes");
        }

        public async Task<ActionResult> UpdateBuyStock(Portfolio order, int orderID)
        {

            var checkStock = await _tradingDb.UpdateBuyStock(order, orderID);

            if (checkStock != null) return Ok(checkStock);
            _log.LogInformation("Stock was not updated");
            return NotFound("Stock was not updated");

        }

        public async Task<ActionResult> UpdateSellStock(Portfolio order, int orderID)
        {

            var checkStock = await _tradingDb.UpdateBuyStock(order, orderID);

            if (checkStock != null) return Ok(checkStock);
            _log.LogInformation("Stock was not deleted");
            return NotFound("Stock was not deleted");

        }

        public async Task<ActionResult> GetAllTransactions()
        {
            var transactions = await _tradingDb.GetAllTransactions();

            if (transactions != null) return Ok(transactions);
            _log.LogInformation("Could not get all transactions");
            return NotFound("Could not get all transactions");
        }

        // Usikker om vi trenger den i controlleren:

        /*public async Task<ActionResult> CreateTransaction(string status, int id, Portfolio portfolio, double quantity)
        {
            var list = await _tradingDb.CreateTransaction(status, id, portfolio, quantity);

            if (list != null) return Ok(list);
            _log.LogInformation("Could not get all transactions");
            return NotFound("Could not get all transactions");

        }*/
    }
}