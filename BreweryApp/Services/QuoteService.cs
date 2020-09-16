using BreweryApp.ApiModels;
using BreweryApp.Contexts;
using BreweryApp.DTO;
using BreweryApp.Exceptions;
using BreweryApp.Exceptions.RequestQuote;
using BreweryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryApp.Services
{
    public class QuoteService : IQuoteService
    {
        private IBreweryContext _breweryContext;
        public QuoteService(IBreweryContext breweryContext)
        {
            _breweryContext = breweryContext;
        }


        public async Task<QuoteResult> GetQuote(RequestQuote requestQuote)
        {
            if (requestQuote.Beers == null || requestQuote.Beers.Count == 0 || requestQuote.Beers.Sum(b => b.Count) == 0)
            {
                throw new EmptyQuoteRequestException();
            }

            Wholesaler wholesaler = await _breweryContext.GetWholesaler(requestQuote.WholesalerId);
            if (wholesaler == null)
            {
                throw new WholesalerNotFoundException();
            }

            var doubleBeers =
                requestQuote.Beers
                .GroupBy(b => b.BeerId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (doubleBeers.Any())
            {
                throw new DoubleBeerRequestedException(doubleBeers);
            }

            int totalRequestCountBeer = requestQuote.Beers.Sum(b => b.Count);
            var discountPercentage = 1.0M;
            var totalPrice = 0.0M;
            var requestedBeers = new List<QuoteResultBeer>();
            if (totalRequestCountBeer > 20)
            {
                discountPercentage = 0.8M;
            }
            else if (totalRequestCountBeer > 10)
            {
                discountPercentage = 0.9M;
            }

            foreach (var requestedBeer in requestQuote.Beers)
            {
                var wholeSalerStock = wholesaler.WholesalerStocks.FirstOrDefault(ws => ws.BeerId == requestedBeer.BeerId);
                if (wholeSalerStock == null)
                {
                    throw new BeerNotSoldByWholesalerException(requestedBeer.BeerId);
                }

                if (wholeSalerStock.Count < requestedBeer.Count)
                {
                    throw new NotEnoughStockException(requestedBeer.BeerId, wholeSalerStock.Count);
                }

                requestedBeers.Add(new QuoteResultBeer()
                {
                    BeerId = requestedBeer.BeerId,
                    BeerName = wholeSalerStock.Beer.Name,
                    RequestedCount = requestedBeer.Count
                });

                totalPrice += requestedBeer.Count * wholeSalerStock.Beer.Price;
            }

            totalPrice = totalPrice * discountPercentage;

            return new QuoteResult()
            {
                Price = Math.Round(totalPrice, 2),
                Beers = requestedBeers,
                WholesalerId = wholesaler.Id,
                WholesalerName = wholesaler.Name
            };
        }
    }
}
