using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryApp.Models
{
    public class Brewery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<Beer> Beers { get; set; }
    }
}
