using AttendanceSystemFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttendanceSystemFinal.Controllers
{
    [Authorize(Roles = "Secureity")]
    public class securityController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            List<Department> department = db.Departments.ToList<Department>();
            ViewBag.department = new SelectList(department, "DeptNo", "DeptName");
            return View();
        }

        public ActionResult dropdownone()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            SelectListItem item1 = new SelectListItem() { Text = "choose", Value = "1" };
            SelectListItem item2 = new SelectListItem() { Text = "Attande", Value = "2" };
            SelectListItem item3 = new SelectListItem() { Text = "Exit", Value = "3" };
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);
            ViewBag.items = items;
            return View();
        }
        public ActionResult getstudent(int id)
        {

            List<Student> students = db.Students.ToList().FindAll(a => a.DeptNo == id && a.IsAttend == false);
            List<Student> std = new List<Student>();

           
            foreach (var item in students)
            {

                std.Add(item);
            }

            return PartialView(std);
        }

        public ActionResult copy(int id)
        {

           
            Attendance student = new Attendance();


            Student std = db.Students.FirstOrDefault(a => a.stdId == id);
            std.IsAttend = true;
            student.stdId = std.stdId;
         
            student.date = DateTime.Now;
           
            student.attTime = DateTimeOffset.Now.TimeOfDay;

            db.attendances.Add(student);
            db.SaveChanges();

            return RedirectToAction("index");
        }
    }







}
