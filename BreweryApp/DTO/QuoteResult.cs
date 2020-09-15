using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryApp.DTO
{
    public class QuoteResult
    {
        public decimal Price { get; set; }
        public int WholesalerId { get; set; }
        public string WholesalerName { get; set; }
        public List<QuoteResultBeer> Beers { get; set; }
    }


    public class QuoteResultBeer
    {
        public int BeerId { get; set; }
        public string BeerName { get; set; }
        public int RequestedCount { get; set; }
    }
}
