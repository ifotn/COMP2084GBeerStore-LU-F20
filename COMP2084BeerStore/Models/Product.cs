using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COMP2084BeerStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string SKU { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public float AlcoholContent { get; set; }
        [Required]
        public int Volume { get; set; }

        // parent object reference
        public Category Category { get; set; }

        // child ref
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
