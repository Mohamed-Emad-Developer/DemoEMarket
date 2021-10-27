using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
 

namespace DemoEMarket.Models.viewModel
{
    public class ProductCategoryVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
