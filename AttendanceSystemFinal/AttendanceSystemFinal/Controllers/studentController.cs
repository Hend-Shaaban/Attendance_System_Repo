using AttendanceSystemFinal.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace AttendanceSystemFinal.Controllers
{
    [Authorize(Roles ="Student")]
    public class studentController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult permission()
        {
            var userId = User.Identity.GetUserId();
            string Email = db.Users.FirstOrDefault(u => u.Id == userId).Email;
            Student std = db.Students.ToList().FirstOrDefault(a => a.Email == Email);
            ViewBag.stdname = std.stdName;
            ViewBag.phone = db.Users.FirstOrDefault(u => u.Id == userId).PhoneNumber;
            ViewBag.deptname = db.Departments.ToList().FirstOrDefault(a => a.DeptNo == std.DeptNo).DeptName;
            return View();
        }

        [HttpPost]
        public ActionResult permission(permission model)
        {
            permission newp = new permission();
            var userId = User.Identity.GetUserId();
            string Email = db.Users.FirstOrDefault(u => u.Id == userId).Email;
            Student std = db.Students.ToList().FirstOrDefault(a => a.Email == Email);
            newp.stdId= std.stdId;
            newp.date = model.date;
            newp.note = model.note;
            db.permissions.Add(newp);
            db.SaveChanges();


            return RedirectToAction("Index", "Home");
        }
      
      
        public ActionResult profile()
        {
            var userId = User.Identity.GetUserId();

            string Email = db.Users.FirstOrDefault(u => u.Id == userId).Email;
            var student = db.Students.ToList().Find(a => a.Email == Email);
            var dept = db.Departments.ToList().Find(a => a.DeptNo == student.DeptNo);
            ViewBag.deptname = dept.DeptName;
            return View(student);
        }
        public ActionResult MyAttendence()
        {

            return View();
        }


        public ActionResult MyAttendences(string EnterDate)
        {
            DateTime d = DateTime.Parse(EnterDate);
            TimeSpan time = new TimeSpan(9, 0, 0);
            var userId = User.Identity.GetUserId();
            string Email = db.Users.FirstOrDefault(u => u.Id == userId).Email;
            var student = db.Students.ToList().Find(a => a.Email == Email);
            Attendance data = db.attendances.FirstOrDefault(a => a.stdId == student.stdId && a.date.Day == d.Day && a.date.Month == d.Month && a.date.Year == d.Year);
           permission per = db.permissions.FirstOrDefault(a => a.stdId == student.stdId &&a.status=="accepted"&& a.date.Day == d.Day && a.date.Month == d.Month && a.date.Year == d.Year);
            if (data==null&&per==null)
            {
                ViewBag.status = "absent";
            }
            else if(per!=null&&data==null)
            {
                ViewBag.status = "permission";
            }
            else if(data!=null)
            {
                if(data.attTime>time)
                {
                    ViewBag.status = "late";
                }
                if (data.attTime <= time)
                {
                    ViewBag.status = "ontime";
                }
            }
            ViewBag.stdname = student.stdName;
            if(data==null)
            {
                data = new Attendance();
                data.attTime = TimeSpan.Parse("0");
                data.stdId = student.stdId;
                data.date = DateTime.Parse(EnterDate);
                data.id = 0;
            }
            return PartialView(data);
        }
    }
}
   