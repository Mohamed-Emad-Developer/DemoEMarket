using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoEMarket.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Please enter Product Name")]
        [MinLength(2,ErrorMessage ="please enter 2 characters at least")]
        [DisplayName("Name")]
        public string Name { get; set; }

        //[DisplayName("Piece")]
        //[Range(0, int.MaxValue, ErrorMessage = "Enter number of packs")]
        //public int Pieces { get; set; }

        [Required(ErrorMessage ="please enter price of product")]
        [Range(0,double.PositiveInfinity,ErrorMessage ="please enter number bigger than zero")]
        [DisplayName("Price")]
        public double Price { get; set; }
        public int AvailableProducts { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
        public string ImageName { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        //public IEnumerable<Item> Items { get; set; }
        //public Product()
        //{
        //    Items = new List<Item>();
        //}
        public string VendorId { get; set; }
        public ApplicationUser Vendor { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Product()
        {
            ModifiedDate = DateTime.Now;
        }
        [Required]
        public string Description { get; set; }

    }
}
