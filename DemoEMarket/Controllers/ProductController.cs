using DemoEMarket.Data;
using DemoEMarket.Models;
using DemoEMarket.Models.viewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DemoEMarket.Controllers
{
    
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hosting;

        public ProductController(ApplicationDbContext db, UserManager<IdentityUser> userManager, IWebHostEnvironment hosting)
        {
            _db = db;
            _userManager = userManager;
            _hosting = hosting;
        }
        [Authorize(Roles = "Vendor")]
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var products = _db.Products.Include(p=>p.Category).Where(p=> p.VendorId == userId).OrderBy(p=> p.Name).ToList();
           
            return View(products);
        }
        [Authorize(Roles = "Vendor")]
        public IActionResult Create()
        {
            var viewModel = new ProductCategoryVM
            {
                Product = new Product(),
                Categories = _db.Categories.OrderBy(c=> c.Title).Select(
                        i => new SelectListItem
                        {
                            Text = i.Title,
                            Value = i.Id.ToString()
                        }
                    )
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Vendor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductCategoryVM model)
        {
           
                
            if (ModelState.IsValid )
            {
                string fileName = String.Empty;
                if(model.Product.Image != null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, @"images/uploads/products");
                    fileName = model.Product.Image.FileName;
                    string fullPath = Path.Combine(uploads, fileName);
                    model.Product.Image.CopyTo(new FileStream(fullPath, FileMode.Create));
                }
                var userId = _userManager.GetUserId(User);
                model.Product.VendorId = userId;
                model.Product.ImageName = fileName; 
                _db.Products.Add(model.Product);
                _db.SaveChanges();
                return RedirectToAction("Details", new { id = model.Product.Id});
            }
            var viewModel = new ProductCategoryVM
            {
                Product = new Product(),
                Categories = _db.Categories.OrderBy(c => c.Title).Select(
                        i => new SelectListItem
                        {
                            Text = i.Title,
                            Value = i.Id.ToString()
                        }
                    )
            };
            return View(viewModel);
        }
       
        [Authorize(Roles = "Vendor")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var product = _db.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            string uploads = Path.Combine(_hosting.WebRootPath, @"images/uploads/products");
            string fullPath = Path.Combine(uploads, product.ImageName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            _db.Products.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Vendor , Customer")]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();
            var product = _db.Products.Include(p=> p.Category).SingleOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
             
            return View(product);
        }
      
        [Authorize(Roles = "Vendor")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var product = _db.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            var viewModel = new ProductCategoryVM
            {
                Product = product,
                Categories = _db.Categories.Select(
                       i => new SelectListItem
                       {
                           Text = i.Title,
                           Value = i.Id.ToString()
                       }
                   )
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Vendor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductCategoryVM model)
        {
            var userId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                string fileName = String.Empty;
                if (model.Product.Image != null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, @"images/uploads/products");
                    fileName = model.Product.Image.FileName;
                    string fullPath = Path.Combine(uploads, fileName);
                    //delete old file
                    var productInDb = _db.Products.SingleOrDefault(p => p.Id == model.Product.Id);
                    string oldFileName = productInDb.ImageName;
                    string FullOldPath = Path.Combine(uploads,oldFileName);
                    if (fullPath != FullOldPath)
                    {
                        using(var fileStream = new FileStream(fullPath, FileMode.Create))
                        {

                             model.Product.Image.CopyTo(fileStream);
                        }
                        

                        System.IO.File.Delete(FullOldPath);
                        
                    }
                }
                model.Product.ImageName = fileName;
                model.Product.VendorId = userId;
                _db.Products.Update(model.Product);
                _db.SaveChanges();
                return RedirectToAction("Index", "Product");
            }
            return View(model);
        }
    }
}
