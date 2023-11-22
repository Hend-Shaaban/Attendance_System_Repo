using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AttendanceSystemFinal.Models
{
    public class Student
    {
       
        [Key]
        public int stdId { get; set; }
        public string stdName { get; set; }

        [ForeignKey("Department")]
        public int DeptNo { get; set; }

        public Department Department { get; set; }
        public string Email { get; set; }

       
        public Boolean IsAttend { get; set; }  
        public List<Attendance> attendance { get; set; }
        public List<permission> permission { get; set; }

    }
}


  