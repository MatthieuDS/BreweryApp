using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryApp.Exceptions
{
    public class NotEnoughStockException : Exception
    {
        public int BeerId { get; set; }
        public int CurrentStock { get; set; }
        public NotEnoughStockException(int beerId, int currentStock)
        {
            BeerId = beerId;
            CurrentStock = currentStock;
        }
    }
}
