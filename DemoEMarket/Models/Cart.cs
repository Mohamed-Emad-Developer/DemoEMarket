using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoEMarket.Models
{
    public class Cart
    {
        public int Id { get; set; }
        //public ICollection<Product> Products { get; set; }

        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }

    }
}
