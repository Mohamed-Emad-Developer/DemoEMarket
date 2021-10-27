using DemoEMarket.Data;
using DemoEMarket.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoEMarket.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public List<Product> Get()
        {
            var products = _db.Products.ToList();
            foreach(var p in products)
            {
                p.Category = _db.Categories.SingleOrDefault(c => c.Id == p.CategoryId);
            }
            return products ;
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (product != null)
            {
                _db.Products.Add(product);
                _db.SaveChanges();
            }
            else
            {
                return NotFound();
            }
                
            return Ok(product);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetProduct(int? id)
        {
            if (id == null)
                return NotFound();
            
            var product = _db.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            

            return Ok(product);
        }
    }
}
