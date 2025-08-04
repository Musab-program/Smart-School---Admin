using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class TeacherHoliday
    {
        //These Attributes Are The Culomns for TeacherHolidays Table In Database
        public int Id { get; set; }
        public Teacher Teacher { get; set; }
        public int TeacherId { get; set; }//Forign Key n to 1 With Teacher Table
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public bool IsAgreeded { get; set; }
    }
}
