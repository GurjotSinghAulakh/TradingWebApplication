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
    }
}