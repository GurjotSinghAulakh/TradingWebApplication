using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
// using Newtonsoft.Json;

namespace TastyTrading.Models
{
    [ExcludeFromCodeCoverage]
    public class Portfolio
    {
        [Key]
        public int Portfolio_Id { get; set; }
        
        // [RegularExpression(@"^[0-9]{4} ?[0-9]{4} ?[0-9]{4} ?[0-9]{4}$", ErrorMessage = "Invalid CardNumber")]
        public double Stock_Quantity { get; set; }

        // [RegularExpression(@"^\d{3}$", ErrorMessage = "Invalid CSC")]
        public string Stock_Symbol { get; set; }

        // [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{5,40}$", ErrorMessage = "Invalid CardHolderName")]
        public string Stock_Name { get; set; }

        // [RegularExpression(@"^(0[1-9]|1[0-2])$", ErrorMessage = "Invalid ExpirationMonth")]
        public double Stock_Price { get; set; }
        
        //[RegularExpression(@"^(0?[1-9]|[1-9][0-9])$", ErrorMessage = "Invalid ExpirationYear")]
        public double Portfolio_Balance { get; set; }

        private Stock stock;

        // Setting up many to many
        // Ignoring looping Json
        // Source: https://stackoverflow.com/questions/7397207/json-net-error-self-referencing-loop-detected-for-type
        /*
         * [JsonIgnore] 
        [IgnoreDataMember] 
        public virtual ICollection<Customer> Customers { get; set; }

        public Payment()
        {
            this.Customers = new HashSet<Customer>();
        }
        */

        public virtual Stock GetStock()
        {
            return stock;
        }

        // Setting up many to many
        // Ignoring looping Json
        // Source: https://stackoverflow.com/questions/7397207/json-net-error-self-referencing-loop-detected-for-type
        /*
         * [JsonIgnore] 
        [IgnoreDataMember] 
        public virtual ICollection<Customer> Customers { get; set; }

        public Payment()
        {
            this.Customers = new HashSet<Customer>();
        }
        */

        public virtual void SetStock(Stock value)
        {
            stock = value;
        }
    }
}