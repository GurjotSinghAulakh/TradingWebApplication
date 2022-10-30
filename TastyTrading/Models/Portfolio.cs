using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using TastyTrading.DAL;

namespace TastyTrading.Models
{
    [ExcludeFromCodeCoverage]
    public class Portfolio
    {
        [Key]
        public int Id { get; set; }

        // These will come from the lazy loading.
        public string Symbol { get; set; }
        public double Quantity { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int PersonId { get; set; }

        public virtual User User { get; set; }
        public virtual Stock Stock { get; set; }
    }

}
