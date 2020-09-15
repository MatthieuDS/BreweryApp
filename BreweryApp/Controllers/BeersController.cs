using BreweryApp.Contexts;
using BreweryApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeersController
    {
        private IBreweryContext _breweryContext;
        public BeersController(IBreweryContext breweryContext)
        {
            _breweryContext = breweryContext;
        }

        [HttpGet]
        public async Task<List<Beer>> Get()
        {
            return await _breweryContext.GetBeers();
        }

        [HttpPost]
        public async Task CreateBeer(Beer beer)
        {
            await _breweryContext.CreateBeer(beer);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteBeer(int beerId)
        {
            await _breweryContext.DeleteBeer(beerId);
        }
    }
}
