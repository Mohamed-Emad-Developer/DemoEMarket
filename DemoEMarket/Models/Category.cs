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
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Category")]
        public string Title { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public Category()
        {
            Products = new List<Product>();
        }
        public string ImageName { get; set; }
        [NotMapped]
        //[Required]
        public IFormFile Image { get; set; }
        public string VendorId { get; set; }
        public ApplicationUser Vendor { get; set; }
    }
}
