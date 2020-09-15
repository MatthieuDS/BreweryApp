using BreweryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryApp.Contexts
{
    public interface IBreweryContext
    {
        Task<List<Beer>> GetBeers();
        Task CreateBeer(Beer beer);
        Task DeleteBeer(int beerId);
        Task AddBeerToWholeSaler(int wholesalerId, int beerId, int count);
        Task UpdateWholesalerStock(int wholeSalerId, int beerId, int count);
        Task DeleteWholesalerStock(int wholeSalerId, int beerId);
        Task<Wholesaler> GetWholesaler(int wholesalerId);
    }
}
