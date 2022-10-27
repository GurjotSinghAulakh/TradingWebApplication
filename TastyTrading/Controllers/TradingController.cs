using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TastyTrading.DAL.Repositories;
using System.Collections;
using System.Security.Cryptography;
using TastyTrading.Models;

namespace TastyTrading.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/v1/[controller]/[action]")]
    public class TradingController : ControllerBase
    {
        private readonly ITradingRepository _db;

        private readonly ILogger<TradingController> _log;

        public TradingController(ITradingRepository db, ILogger<TradingController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> GetStocks() 
        {
            var list = await _db.GetStocks();

            if (list != null) return Ok(list);
            _log.LogInformation("Could not get all stocks");
            return NotFound("Could not get all stocks");
        }

        public async Task<ActionResult> GetOneStock(int id)
        {
            var product = await _db.GetOneStock(id);

            if (id > 0) return Ok(product);
            _log.LogInformation("Stock was not found");
            return NotFound("Stock was not found");
        }

        public async Task<ActionResult> SaveStock(Portfolio mystock)
        {
            if (ModelState.IsValid)
            {
                var res = await _db.SaveStock(mystock);

                if (res) return Ok("Stock saved in the portfolio");
                _log.LogInformation("Stock was not saved");
                return BadRequest("Stock was not saved");
            }


            _log.LogInformation("Stock was not found");
            return BadRequest("Stock was not found");
        }

        /*
        public async Task<ActionResult> UpdateStock(Buy mystock)
        {
            return await _db.UpdateStock(mystock);
        }*/


        // update balance in the portfolio
        public async Task<ActionResult> UpdateBalance(Portfolio portfolio)
        {
            if (ModelState.IsValid)
            {
                var ret = await _db.UpdateBalance(portfolio);

                if (ret) return Ok("Balance updated");
                _log.LogInformation("Balance was not updated");
                return NotFound("Balance was not updated");
            }

            _log.LogInformation("Input validation failed:");
            return BadRequest("Input validation failed ");
        }
        
        // read stocks from portfolio
        public async Task<ActionResult> GetPortfolio(int PId)
        {
            var myStocks = await _db.GetPortfolio(PId);

            if (myStocks != null) return Ok(myStocks);
            _log.LogInformation("Could not find the stocks");
            return NotFound("Could not find the stocks");
        }

        //  create to portfolio
        public async Task<ActionResult> BuyStock(Order order)
        {

            var product = await _db.BuyStock(order);

            if (product != null) return Ok(product);
            _log.LogInformation("Product was not found");
            return NotFound("Product was not found");
            //ta med metoden AddToPortofolio(buy), den legger nye kjøpet vårt i portofolio.
        }

        // Delete from portfolio
        public async Task<ActionResult> SellStock(Portfolio sellOrder)
        {
            var product = await _db.SellStock(sellOrder);

            if (product) return Ok("Stock deleted");
            _log.LogInformation("Stock was not deleted");
            return NotFound("Stock was not deleted");

        }

        public async Task<ActionResult> GetAllTransactions()
        {
            var list = await _db.GetAllTransactions();

            if (list != null) return Ok(list);
            _log.LogInformation("Could not get all transactions");
            return NotFound("Could not get all transactions");

        }

        public async Task<ActionResult> GetTransactions(DateTime fromDate, DateTime toDate)
        {
            var list = await _db.GetTransactions(fromDate, toDate);

            if (list != null) return Ok(list);
            _log.LogInformation("Could not get transaction");
            return NotFound("Could not get transaction");

        }









        /* public async Task<ActionResult> SaveCustomer(Customer customer)
         {
             if (ModelState.IsValid)
             {
                 var res = await _db.SaveCustomer(customer);

                 if (res) return Ok("Ticket saved");
                 _log.LogInformation("Customer ticket was not saved");
                 return BadRequest("Customer ticket was not saved");
             }

             var message = GetModelStateMessage();

             _log.LogInformation("Customer was not saved: " + message);
             return BadRequest("Customer was not saved: " + message);
         }
         public async Task<ActionResult> SaveCustomers(List<Customer> customers)
         {
             if (ModelState.IsValid)
             {
                 var res = await _db.SaveCustomers(customers);

                 if (res) return Ok("Ticket saved");
                 _log.LogInformation("Customer ticket was not saved");
                 return BadRequest("Customer ticket was not saved");
             }

             var message = GetModelStateMessage();

             _log.LogInformation("Input validation failed: " + message);
             return BadRequest("Input validation failed: " + message);
         }

         public async Task<ActionResult> GetCustomer(string reference)
         {
             var customer = await _db.GetCustomer(reference);

             if (customer != null) return Ok(customer);
             _log.LogInformation("Customer was not found");
             return NotFound("Customer was not found");
         }
         public async Task<ActionResult> GetCustomers()
         {
             var list = await _db.GetCustomers();

             if (list != null) return Ok(list);
             _log.LogInformation("Could not get all customers");
             return NotFound("Could not get all customers");
         }

         public async Task<ActionResult> UpdateCustomer(Customer customer)
         {
             if (ModelState.IsValid)
             {
                 var ret = await _db.UpdateCustomer(customer);

                 if (ret) return Ok("Customer updated");
                 _log.LogInformation("Customer was not found");
                 return NotFound("Customer was not found");
             }

             var message = GetModelStateMessage();

             _log.LogInformation("Input validation failed: " + message);
             return BadRequest("Input validation failed " + message);
         }

         public async Task<ActionResult> DeleteCustomer(string reference)
         {
             var ret = await _db.DeleteCustomer(reference);

             if (ret) return Ok("Customer deleted");
             _log.LogInformation("Customer was not deleted");
             return NotFound("Customer was not deleted");
         }

         public async Task<ActionResult> GetCabin(int id)
         {
             var cabin = await _db.GetCabin(id);

             if (cabin != null) return Ok(cabin);
             _log.LogInformation("Could not get cabin");
             return BadRequest("Could not get cabin");
         }

         public async Task<ActionResult> GetCabins()
         {
             var list = await _db.GetCabins();

             if (list != null) return Ok(list);
             _log.LogInformation("Could not get all cabins");
             return NotFound("Could not get all cabins");
         }

         public async Task<ActionResult> GetCabinUnoccupied()
         {
             var list = await _db.GetCabinUnoccupied();
             if (list != null) return Ok(list);
             _log.LogInformation("Not able to get unoccupied cabins");
             return NotFound("Not able to get unoccupied cabins");
         }

         public async Task<ActionResult> GetRute(int id)
         {
             var rute = await _db.GetRoute(id);
             if (rute != null) return Ok(rute);
             _log.LogInformation("Not able to get rute");
             return NotFound("Not able to get rute");
         }

         public async Task<ActionResult> GetRoutes()
         {
             var list = await _db.GetRoutes();

             if (list != null) return Ok(list);
             _log.LogInformation("Could not get all routes");
             return NotFound("Could not get all routes");
         }

         public async Task<ActionResult> GetTickets()
         {
             var list = await _db.GetTickets();

             if (list != null) return Ok(list);
             _log.LogInformation("Could not get all tickets");
             return NotFound("Could not get all tickets");
         }

         public async Task<ActionResult> GetPostalCode(string code)
         {
             var postalcode = await _db.GetPostalCode(code);
             if (postalcode != null) return Ok(postalcode);
             _log.LogInformation("Could not get post name");
             return NotFound("Could not get post name");
         }

         public ActionResult GetReference(string firstname, string lastname)
         {
             var reference = _db.GenerateReference(firstname, lastname);
             if (reference != null) return Ok(reference);
             _log.LogInformation("Could not generate reference code");
             return NotFound("Could not generate reference code");
         }

         public async Task<ActionResult> GetCustomersByReferences(string reference)
         {
             string[] references = reference.Split("-");

             var customers = await _db.GetCustomersByReferences(references);
             if (customers != null) return Ok(customers);
             _log.LogInformation("Did not find tickets by reference");
             return NotFound("Did not find tickets by reference");
         }

         public ActionResult GetPrice(List<Cabin> cabins)
         {
             var price = _db.GeneratePrice(cabins);
             if (double.IsNaN(price)) return Ok(price);
             _log.LogInformation("Could not generate price");
             return NotFound("Could not generate price");
         }

         public ActionResult ValidatePayment(Payment payment)
         {
             if (ModelState.IsValid)
             {
                 var res = _db.PaymentCheck(payment);
                 if (res) return Ok("Payment validation succeeded");
                 _log.LogInformation("Payment validation failed");
                 return BadRequest("Payment validation failed");
             }

             var message = GetModelStateMessage();

             _log.LogInformation("Input validation failed: " + message);
             return BadRequest("Input validation failed: " + message);
         }

         public async Task<ActionResult> GetDeparturesByDateAndRoute(string date, int routeId)
         {
             var res = await _db.GetDeparturesByDateAndRoute(date, routeId);
             return Ok(res);
         }

         public async Task<ActionResult> GetDepartures()
         {
             var res = await _db.GetDepartures();
             return Ok(res);
         }

         public async Task<ActionResult> GetDeparture(int id)
         {
             Console.WriteLine(id);
             var res = await _db.GetDeparture(id);
             return Ok(res);
         }

         /**
          * Formatting multiple model state messages for better logging

         private string GetModelStateMessage()
         {
             return string.Join(", ", ModelState.Values
                 .SelectMany(v => v.Errors)
                 .Select(e => e.ErrorMessage));
         }*/
    }
}