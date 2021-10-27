using DemoEMarket.Data;
using DemoEMarket.Models;
using DemoEMarket.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoEMarket.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserManager<IdentityUser> _userManager;
        public CartController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult ListOfCarts()
        {
            var userId = _userManager.GetUserId(User);
            var carts = _db.Carts.Where(c=> c.CustomerId == userId).Include(c=> c.Product).ToList();
            return View(carts);
        }
        
        public IActionResult AddToCart(int? id)
        {
            if (id == null)
                return NotFound();

            var product = _db.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            var userId = _userManager.GetUserId(User);
            Cart cart = new Cart();
            cart.CustomerId = userId;
            if (product.AvailableProducts > 0)
            {
                cart.Product = product;
                cart.Quantity++;
                cart.Cost = (decimal)product.Price * cart.Quantity;
                _db.Carts.Add(cart);

            }
            _db.SaveChanges();
            return RedirectToAction("ListOfCarts"); ;
        }
        public IActionResult IncreaseQuantity(int? id) 
        {
            if (id == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            var cart = _db.Carts.SingleOrDefault(c => c.Id == id && c.CustomerId == userId);
            var product = _db.Products.SingleOrDefault(p => p.Id == cart.ProductId);
            if (cart == null || product == null)
                return NotFound();

            cart.Quantity++;
            cart.Cost = (decimal)product.Price * cart.Quantity;
            _db.SaveChanges();
            return RedirectToAction("ListOfCarts");

        }

        public IActionResult DecreaseQuantity(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            var cart = _db.Carts.SingleOrDefault(c => c.Id == id && c.CustomerId == userId);
            var product = _db.Products.SingleOrDefault(p => p.Id == cart.ProductId);
            if (cart == null || product ==null)
                return NotFound();

            if(cart.Quantity != 0)
            {

                cart.Quantity--;
                cart.Cost = (decimal)product.Price * cart.Quantity;
            }
            else
            {
                ViewData["no"] = "Noooo..";
                return RedirectToAction("ListOfCarts");
            }
            _db.SaveChanges();
            return RedirectToAction("ListOfCarts");

        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var cart = _db.Carts.SingleOrDefault(c => c.Id == id);

            if (cart == null)
                return NotFound();

            _db.Carts.Remove(cart);
            _db.SaveChanges();
            return RedirectToAction("ListOfCarts");
        }
        public IActionResult CheckOut()
        {
            var userId = _userManager.GetUserId(User);
            var carts = _db.Carts.Include(c=> c.Product).Include(c => c.Product.Vendor).Where(c => c.CustomerId == userId).ToList();
            if (carts == null)
                return NotFound();

            foreach(var cart in carts)
            {
                cart.Product.AvailableProducts = cart.Product.AvailableProducts - cart.Quantity;
                cart.Product.Vendor.Wallet = cart.Product.Vendor.Wallet + cart.Cost;

            }
            _db.Carts.RemoveRange(carts);

            _db.SaveChanges();
            return RedirectToAction("Index","Home");
        }
    }
}
