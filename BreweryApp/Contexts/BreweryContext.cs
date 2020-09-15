using BreweryApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryApp.Contexts
{
    public class BreweryContext : DbContext, IBreweryContext
    {
        public BreweryContext(DbContextOptions<BreweryContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WholesalerStock>().HasKey(ws => new { ws.BeerId, ws.WholesalerId });
        }

        public DbSet<Beer> Beers { get; set; }
        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<Wholesaler> Wholesalers { get; set; }
        public DbSet<WholesalerStock> WholesalerStocks { get; set; }

        public async Task CreateBeer(Beer beer)
        {
            Beers.Add(beer);
            await SaveChangesAsync();
        }

        public async Task DeleteBeer(int beerId)
        {
            var beer = await Beers.FirstAsync(b => b.Id == beerId);
            Beers.Remove(beer);
            await SaveChangesAsync();
        }

        public async Task<List<Beer>> GetBeers()
        {
            return await 
                Beers
                .Include(b => b.Brewery)
                .Include(b => b.WholesalerStocks)
                    .ThenInclude(ws => ws.Wholesaler)
                .ToListAsync();
        }

        public async Task AddBeerToWholeSaler(int wholesalerId, int beerId, int count)
        {
            WholesalerStocks.Add(new WholesalerStock()
            {
                WholesalerId = wholesalerId,
                BeerId = beerId,
                Count = count,
            });
            await SaveChangesAsync();
        }

        public async Task UpdateWholesalerStock(int wholesalerId, int beerId, int count)
        {
            var wsStock = WholesalerStocks.First(ws => ws.WholesalerId == wholesalerId && ws.BeerId == beerId);
            wsStock.Count = count;
            await SaveChangesAsync();
        }

        public async Task DeleteWholesalerStock(int wholesalerId, int beerId)
        {
            var wsStock = WholesalerStocks.First(ws => ws.WholesalerId == wholesalerId && ws.BeerId == beerId);
            WholesalerStocks.Remove(wsStock);
            await SaveChangesAsync();
        }

        public async Task<Wholesaler> GetWholesaler(int wholesalerId)
        {
            return await Wholesalers
                .Include(ws => ws.WholesalerStocks)
                .ThenInclude(ws => ws.Beer)
                .FirstOrDefaultAsync(ws => ws.Id == wholesalerId);
        }
    }
}
