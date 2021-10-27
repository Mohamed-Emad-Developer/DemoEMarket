using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoEMarket.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public decimal Wallet { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public ApplicationUser()
        {
            Products = new List<Product>();
        }
    }
}
