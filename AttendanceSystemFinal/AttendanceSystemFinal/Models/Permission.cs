using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystemFinal.Models
{
    public class permission
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("student")]
        public int stdId { get; set; }
        public DateTime date { get; set; } 
        public string note { get; set; }
        public string status { get; set; }
        [NotMapped]
        public bool accepted { get; set; }
        [NotMapped]
        public bool refused { get; set; }

        public Student student { get; set; }


    }
    public class PermissionModel
    {
        public List<permission> permissions { get; set; }
    }
}




  