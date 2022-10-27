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
        public double Total { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        
        public virtual Stock stock { get; set; }

        public virtual Order order { get; set; }
    }
}

