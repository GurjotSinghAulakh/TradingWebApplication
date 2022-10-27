using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
// using Newtonsoft.Json;

namespace TastyTrading.Models
{
    [ExcludeFromCodeCoverage]
    public class Stock
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Product_name { get; set; }
        public double Price { get; set; }
        public double Volume { get; set; }


        /*
        // Setting up many to many
        // Ignoring looping Json
        // Source: https://stackoverflow.com/questions/7397207/json-net-error-self-referencing-loop-detected-for-type
        [JsonIgnore] 
        [IgnoreDataMember] 
        public virtual ICollection<Ticket> Tickets { get; set; }

        public Cabin()
        {
            this.Tickets = new HashSet<Ticket>();
        }
        */

    }
}
