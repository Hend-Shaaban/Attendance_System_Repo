using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace AttendanceSystemFinal.Models
{
    public class AttendanceViewModel
    {
        public string stdname { get; set; }
        public int ontime { get; set; }
        public int late { get; set; }
        public int absent { get; set; }
        public int permission { get; set; }
    }
}

  