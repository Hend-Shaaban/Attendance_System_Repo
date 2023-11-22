using AttendanceSystemFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace AttendanceSystemFinal.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                string id = User.Identity.GetUserId();
                string Email = db.Users.FirstOrDefault(u => u.Id == id).Email;
                string phone = db.Users.FirstOrDefault(u => u.Id == id).PhoneNumber;
                if(Email!=null)
                {
                    ViewBag.Email = Email;
                }
                if (phone != null)
                {
                    ViewBag.phone = phone;
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}