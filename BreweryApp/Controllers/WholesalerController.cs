using BreweryApp.ApiModels;
using BreweryApp.Contexts;
using BreweryApp.DTO;
using BreweryApp.Exceptions;
using BreweryApp.Exceptions.RequestQuote;
using BreweryApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BreweryApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WholesalerController : ControllerBase
    {
        private IBreweryContext _breweryContext;
        private IQuoteService _quoteService;
        public WholesalerController(IBreweryContext breweryContext, IQuoteService quoteService)
        {
            _breweryContext = breweryContext;
            _quoteService = quoteService;
        }

        [HttpPost("stock")]
        public async Task AddBeerToWholesaler(AddUpdateWholesalerStock stock)
        {
            await _breweryContext.AddBeerToWholeSaler(stock.WholesalerId, stock.BeerId, stock.Count);
        }

        [HttpPut("stock")]
        public async Task UpdateWholesalerStock(AddUpdateWholesalerStock stock)
        {
            await _breweryContext.UpdateWholesalerStock(stock.WholesalerId, stock.BeerId, stock.Count);
        }

        [HttpDelete("stock/{wholesalerId}/{beerId}")]
        public async Task DeleteBeerFromWholesaler(int wholesalerId, int beerId)
        {
            await _breweryContext.DeleteWholesalerStock(wholesalerId, beerId);
        }

        [HttpGet("quote")]
        public async Task<ActionResult> GetQuote(RequestQuote requestQuote)
        {
            try
            {
                return Ok(await _quoteService.GetQuote(requestQuote));
            }
            catch (EmptyQuoteRequestException)
            {
                return BadRequest("Cannot request a quote without specifying any product.");
            }
            catch (BeerNotSoldByWholesalerException ex)
            {
                return BadRequest($"Beer {ex.BeerId} is not sold by the wholesaler.");
            }
            catch (DoubleBeerRequestedException ex)
            {
                return BadRequest($"Beer(s) {string.Join(",", ex.DoubleBeerIds)} specified twice.");
            }
            catch (NotEnoughStockException ex)
            {
                return BadRequest($"Beer {string.Join(",", ex.BeerId)} has not enough stock.");
            }
            catch (WholesalerNotFoundException)
            {
                return BadRequest($"Wholesaler {requestQuote.WholesalerId} not found");
            }
        }
    }
}
