using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BreweryApp.Models
{
    public class WholesalerStock
    {
        [JsonIgnore]
        public int BeerId { get; set; }
        [JsonIgnore]
        public Beer Beer { get; set; }
        [JsonIgnore]
        public int WholesalerId { get; set; }
        public Wholesaler Wholesaler { get; set; }
        public int Count { get; set; }
    }
}
