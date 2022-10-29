using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TastyTrading.Models
{
    [ExcludeFromCodeCoverage]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }

        public string Symbol { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public int OrderID { get; set; }
        public double Quantity { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserID { get; set; }

        public virtual Stock Stock { get; set; }
        public virtual User User { get; set; }
    }
}

