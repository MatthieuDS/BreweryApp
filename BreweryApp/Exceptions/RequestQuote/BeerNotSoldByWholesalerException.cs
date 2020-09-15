using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryApp.Exceptions
{
    public class BeerNotSoldByWholesalerException : Exception 
    {
        public int BeerId { get; set; }
        public BeerNotSoldByWholesalerException(int beerId)
        {
            BeerId = beerId;
        }
    }
}
