using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystemFinal.Models
{
    public class Attendance
    {

        [Key]
        public int id { get; set; }
        [ForeignKey("student")]
        public int stdId { get; set; }
       
        public Student student { get; set; }

        public DateTime date { get; set; } 
        public TimeSpan attTime { get; set; }
        public TimeSpan? exitTime { get; set; }



    }
}



  