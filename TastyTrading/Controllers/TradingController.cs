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

        /*************** http://localhost:9905/api/v1/Trading/GetPortfolio ********************/

        public async Task<ActionResult> GetPortfolio()
        {
            var list = await _tradingDb.GetPortfolio();

            if (list != null) return Ok(list);
            _log.LogInformation("Could not get all routes");
            return NotFound("Could not get all routes");
        }
    }
}