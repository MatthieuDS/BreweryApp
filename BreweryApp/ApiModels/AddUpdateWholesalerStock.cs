using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryApp.ApiModels
{
    public class AddUpdateWholesalerStock
    {
        public int WholesalerId { get; set; }
        public int BeerId { get; set; }
        public int Count { get; set; }
    }
}
