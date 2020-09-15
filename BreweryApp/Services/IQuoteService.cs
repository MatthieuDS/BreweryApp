using BreweryApp.ApiModels;
using BreweryApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryApp.Services
{
    public interface IQuoteService
    {
        Task<QuoteResult> GetQuote(RequestQuote requestQuote);
    }
}
