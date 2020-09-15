using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryApp.Exceptions.RequestQuote
{
    public class DoubleBeerRequestedException : Exception
    {
        public List<int> DoubleBeerIds { get; set; }
        public DoubleBeerRequestedException(List<int> doubleBeerIds)
        {
            DoubleBeerIds = doubleBeerIds;
        }
    }
}
