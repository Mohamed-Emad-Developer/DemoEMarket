using DemoEMarket.Data;
using DemoEMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DemoEMarket.Controllers
{
    [Authorize(Roles = "Vendor")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hosting;
        private readonly UserManager<IdentityUser> _userManager;
        
        public CategoryController(ApplicationDbContext db, IWebHostEnvironment hosting, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _hosting = hosting;
            _userManager = userManager;
           
        }
        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(User);
            var categories = _db.Categories.Where(c=> c.VendorId == userId).Include(c=>c.Products).OrderBy(c => c.Title).ToList();
         
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            string userId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                string fileName = String.Empty;
                if (category.Image != null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, @"images/uploads/categories");
                    fileName = category.Image.FileName;
                    string fullPath = Path.Combine(uploads, fileName);
                    category.Image.CopyTo(new FileStream(fullPath, FileMode.Create));
                }
                category.ImageName = fileName;
                category.VendorId = userId;
                _db.Categories.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index", "Category");
            }

            return View(category);
        }
        
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var category = _db.Categories.SingleOrDefault(c => c.Id == id);
            if (category == null)
                return NotFound();
            string uploads = Path.Combine(_hosting.WebRootPath, @"images/uploads/categories");
            string fullPath = Path.Combine(uploads, category.ImageName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
       
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var category = _db.Categories.SingleOrDefault(c => c.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                return RedirectToAction("Index", "Category");
            }
            return View(category);
        }

    }
}
