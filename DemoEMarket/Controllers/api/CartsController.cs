using DemoEMarket.Data;
using DemoEMarket.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class CartsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public UserManager<IdentityUser> _userManager;
        public CartsController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [HttpGet]
        [Route("ListOfCarts")]
        public List<Cart> ListOfCarts()
        {
            var userId = _userManager.GetUserId(User);
            return _db.Carts.Include(c=> c.Product).Where(c => c.CustomerId == userId).ToList();
        }
    }
}
