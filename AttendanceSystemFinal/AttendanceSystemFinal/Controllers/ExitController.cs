using AttendanceSystemFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttendanceSystemFinal.Controllers
{
    [Authorize(Roles = "Secureity")]
    public class ExitController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Exite
        public ActionResult Index()
        {
            List<Department> department = db.Departments.ToList<Department>();
            ViewBag.department = new SelectList(department, "DeptNo", "DeptName");

            return View();
        }


        public ActionResult getstudent(int id)
        {
            List<Student> std = db.Students.ToList().FindAll(a => a.DeptNo == id);
            List<Attendance> students = new List<Attendance>();
            List<Student> sss = new List<Student>();

            foreach (var i in std)
            {
                if (db.attendances.FirstOrDefault(a => a.stdId == i.stdId && i.IsAttend == true && a.date.Day == DateTimeOffset.Now.Day && a.date.Month == DateTimeOffset.Now.Month && a.date.Year == DateTimeOffset.Now.Year) != null)
                {
                    students.Add(db.attendances.FirstOrDefault(a => a.stdId == i.stdId && i.IsAttend == true && a.date.Day == DateTimeOffset.Now.Day && a.date.Month == DateTimeOffset.Now.Month && a.date.Year == DateTimeOffset.Now.Year));
                }
            }  
              foreach(var m in students)
            {
                sss.Add(db.Students.ToList().FirstOrDefault(a => a.stdId == m.stdId));
            }
                
            List<Attendance> stds = new List<Attendance>();
            foreach (var item in students)
            {

                stds.Add(item);
            }
            ViewBag.sss = sss;

            return PartialView(stds);
        }
        public ActionResult changestatues(int id)
        {


            Attendance student = db.attendances.FirstOrDefault(a => a.stdId == id&& a.date.Day == DateTimeOffset.Now.Day && a.date.Month == DateTimeOffset.Now.Month && a.date.Year == DateTimeOffset.Now.Year);
            student.exitTime = DateTimeOffset.Now.TimeOfDay;
            Student std = db.Students.FirstOrDefault(a => a.stdId == id);
            std.IsAttend = false;
            db.SaveChanges();


            return RedirectToAction(nameof(Index));
        }
    }
}