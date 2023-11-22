using AttendanceSystemFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttendanceSystemFinal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db;

        public AdminController()
        {
            db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }

        public ActionResult Index()
        {
            ViewBag.id = new SelectList(db.Departments, "DeptNo", "DeptName");
            return View();
        }

        public ActionResult showpermissions(int id)
        {
            PermissionModel per = new PermissionModel();
            per.permissions = new List<permission>();
            List<Student> ss = db.Students.ToList().FindAll(a => a.DeptNo == id);
            foreach (Student s in ss)
            {
                List<permission> p = db.permissions.ToList().FindAll(a => a.stdId == s.stdId && a.status == null);
                if (p.Count > 0)
                {
                    per.permissions.AddRange(p);
                }
            }
            List<Student> stu = new List<Student>();
            foreach (permission p in per.permissions)
            {
                stu.Add(ss.Find(a => a.stdId == p.stdId));
            }
            ViewBag.students = stu;
            return PartialView(per);

        }
        public ActionResult submitpermissions(PermissionModel prs)
        {
            List<permission> pp = db.permissions.ToList();
            PermissionModel pr = new PermissionModel();
            pr.permissions = new List<permission>();

            Student s = db.Students.ToList().Find(a => a.stdId == prs.permissions[0].stdId);
            int deptid = s.DeptNo;
            
            if (prs.permissions != null)
            {
                foreach (permission i in prs.permissions)
                {
                    if (i.accepted == true)
                    {
                        permission p = pp.FirstOrDefault(a => a.id == i.id);
                        p.status = "accepted";
                        p.accepted = true;
                        p.refused = false;
                    }
                    if (i.refused == true)
                    {
                        permission p = pp.FirstOrDefault(a => a.id == i.id);
                        p.status = "refused";
                        p.accepted = false;
                        p.refused = true;

                    }
                    if(i.refused==i.accepted)
                    {
                        pr.permissions.Add(i);
                    }
                }

                db.SaveChanges();
            }
            return RedirectToAction("index");//"showpermissions","Admin", new { @id = deptid });
           

        }

        public ActionResult ShowStudents()
        {
            List<Department> departments = db.Departments.ToList();
            ViewBag.departments = new SelectList(departments, " DeptNo", " DeptName");
            return View();
        }


        public ActionResult Showattendances(int departments, string from, string to)
        {
            int late = 0;
            int ontime = 0;
            int absent = 0;
            int permession = 0;

            List<int> permessNo = new List<int>();
            List<int> lateNo = new List<int>();
            List<int> ontimeNo = new List<int>();
            List<int> absentNo = new List<int>();

            TimeSpan time = new TimeSpan(9, 0, 0);
            var from1 = DateTime.Parse(from);
            var to1 = DateTime.Parse(to);

            List<string> studs = new List<string>();
            List<int> ids = new List<int>();
            //view model to send data to the view
            List<AttendanceViewModel> attendsView = new List<AttendanceViewModel>();

            //get list of students in specific department
            List<Student> stdsInDept = db.Students.Where(a => a.DeptNo == departments).ToList<Student>();

            //get list of students' names and ids
            foreach (var item in stdsInDept)
            {
                studs.Add(item.stdName);
                ids.Add(item.stdId);
            }
            //get list of attendances in specific period of time
           
            List<Attendance> attends = new List<Attendance>();
            attends.AddRange(db.attendances.ToList().FindAll(b => b.date.Date >= from1 && b.date.Date <= to1));
            List<List<Attendance>> DeptAttends = new List<List<Attendance>>();
            //store each student in this department with his attendances in this specific period 
            foreach (var item in stdsInDept)
            {
                DeptAttends.Add(attends.FindAll(a => a.stdId == item.stdId));

            }


            List<DateTime> dates = new List<DateTime>();
            DateTime dateFrom = from1;
            DateTime dateTo = to1;
            //create a list of dates between two dates
            while (dateFrom <= dateTo)
            {
                dates.Add(dateFrom);
                dateFrom = dateFrom.AddDays(1);
            }

            

            //get absent times for each student
            foreach (var item in ids)
            {
                foreach (var d in dates)
                {
                    bool student = db.attendances.Any(a => a.stdId == item &&
                                   a.date.Day == d.Day && a.date.Month == d.Month && a.date.Year == d.Year)
                                   ||db.permissions.Any(a => a.stdId == item &&
                                   a.date.Day == d.Day && a.date.Month == d.Month && a.date.Year == d.Year&&a.status== "accepted");
                    if (student == false)
                    {
                        absent += 1;
                    }
                }
                absentNo.Add(absent);
                absent = 0;
            }
            //get permession times for each student
            foreach (var item in ids)
            {
                foreach (var d in dates)
                {
                    permession += db.permissions.Count(a => a.stdId == item && a.date.Day == d.Day && a.date.Month == d.Month && a.date.Year == d.Year && a.status== "accepted");
    
                }
                permessNo.Add(permession);
                permession = 0;
            }
            //get ontime and late times for each student
            foreach (var item in DeptAttends)
            {
                foreach (var t in item)
                {

                    if (t.attTime != null)
                    {
                        if (t.attTime <= time)
                        {
                            ontime += 1;
                        }
                        else
                        {
                            if (t.attTime > time)
                            {
                                late += 1;
                            }

                        }
                    }

                }
                ontimeNo.Add(ontime);
                lateNo.Add(late);
                late = 0;
                ontime = 0;
            }
            //fill the model with data for each student to send to "Show view" to present data
            for (int i = 0; i < studs.Count(); i++)
            {
                AttendanceViewModel attendView = new AttendanceViewModel();
                attendView.stdname = studs[i];
                attendView.ontime = ontimeNo[i];
                attendView.late = lateNo[i];
                attendView.absent = absentNo[i];
                attendView.permission = permessNo[i];
                attendsView.Add(attendView);
            }

            return PartialView(attendsView);
        }

        //sod

        public ActionResult showStd()
        {
            ViewBag.id = new SelectList(db.Departments, "DeptNo", "DeptName");
            return View();
        }
        public ActionResult getStudentsdata(int id, string EnterDate)
        {
            DateTime d = DateTime.Parse(EnterDate);
            List<Student> students = db.Students.Where(a => a.DeptNo == id).ToList<Student>();
            List<Attendance> attend = db.attendances.Where(a =>a.date.Day == d.Day && a.date.Month == d.Month && a.date.Year == d.Year).ToList<Attendance>();
            List<Attendance> stdAttend = new List<Attendance>();
            List<Student> sss = new List<Student>();
            Attendance at = new Attendance();
            at.id = 0;
            at.date = d;
            at.attTime = TimeSpan.Parse("0");

            foreach (var i in students)
            {
                Attendance attendences = attend.Find(a => a.stdId == i.stdId);
                if (attendences != null)
                {
                    stdAttend.Add(attendences);
                    sss.Add(i);
                }
                else
                {
                    at.stdId = i.stdId;
                    stdAttend.Add(at);
                }

            }

            List<permission> per = new List<permission>();
            List<string> status = new List<string>();

            foreach (Student st in students)
            {
                permission pe = db.permissions.ToList().Find(a => a.stdId == st.stdId && a.date.Day == d.Day && a.date.Month == d.Month && a.date.Year == d.Year);
                TimeSpan time = new TimeSpan(9, 0, 0);
                if (pe != null&&sss.Find(a=>a.stdId==st.stdId)==null)
                {
                    status.Add("permission");
                }
                else if(pe==null&& sss.Find(a => a.stdId == st.stdId) == null)
                {
                    status.Add("absent");
                }
                else if(pe==null && sss.Find(a => a.stdId == st.stdId) != null && stdAttend.Find(a=>a.stdId==st.stdId).attTime>time)
                {
                    status.Add("late");
                }
                else if (pe == null && sss.Find(a => a.stdId == st.stdId) != null && stdAttend.Find(a => a.stdId == st.stdId).attTime <= time)
                {
                    status.Add("ontime");
                }

            }
           
            ViewBag.stds = students;
            ViewBag.status = status;
            return PartialView(stdAttend);
        }

       
    }
}



