using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryApp.Models
{
    public class Beer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal AlcoolPercentage { get; set; }
        public Brewery Brewery { get; set; }
        public List<WholesalerStock> WholesalerStocks { get; set; }
    }
}
