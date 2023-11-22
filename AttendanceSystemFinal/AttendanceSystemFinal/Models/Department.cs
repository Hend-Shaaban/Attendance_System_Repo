using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystemFinal.Models
{
    public class Department
    {
        [Key]
        public int DeptNo { get; set; }
        public string DeptName { get; set; }

        public List<Student> students { get; set; }
    }
}



   