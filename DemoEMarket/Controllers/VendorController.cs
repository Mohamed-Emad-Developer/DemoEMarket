using DemoEMarket.Data;
using DemoEMarket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoEMarket.Controllers
{
    public class VendorController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserManager<IdentityUser> _userManager;
        public VendorController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }
        public IActionResult VendorProfile()
        {
            var userId = _userManager.GetUserId(User);
            var user = (ApplicationUser)_db.Users.SingleOrDefault(u=> u.Id == userId);
            user.Products = _db.Products.Where(p => p.VendorId == userId).ToList();
            return View(user);
        }
    }
}
