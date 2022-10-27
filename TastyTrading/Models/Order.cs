using System;
using System.ComponentModel.DataAnnotations;

namespace TastyTrading.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int Quantity { get; set; }
        public DateTime Created_At { get; set; }
        public String BuyID { get; set; }
        // public int SellID { get; set; }

        public virtual Stock Stock { get; set; }

        public virtual Portfolio Portfolio { get; set; }
    }
}

