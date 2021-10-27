using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DemoEMarket.Utility
{
    public static class Helper
    {
        public static string Admin = "Admin";
        public static string Customer = "Customer";
        public static string Vendor = "Vendor";
         public static List<SelectListItem> GetRolesForDropDown(){

            return new List<SelectListItem>
            {
                new SelectListItem{ Value = Helper.Admin, Text = Helper.Admin},
                new SelectListItem{ Value = Helper.Customer, Text = Helper.Customer},
                new SelectListItem{ Value = Helper.Vendor, Text = Helper.Vendor}
            };

        }
    }
}
