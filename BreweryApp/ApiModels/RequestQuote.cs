using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryApp.ApiModels
{
    public class RequestQuote
    {
        public int WholesalerId { get; set; } 
        public List<RequestQuoteBeers> Beers { get; set; }
    }

    public class RequestQuoteBeers
    {
        public int BeerId { get; set; }
        public int Count { get; set; }
    }
}
