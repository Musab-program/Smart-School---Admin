using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class StudentAttendance
    {
        //These Attributes Are The Culomns for StudentAttendance Table In Database
        //                      This is an Intermediatte Table
        public int Id { get; set; }
        public Student Student { get; set; }
        public int StudentId { get; set; }//Forign Key n to 1 With Student Table
        public DateTime AttendanceDate { get; set; }
        public string Status { get; set; }
        public Teacher Teacher { get; set; }
        public int TeacherId { get; set; } //Forign Key n to 1 With Teacher Table
    }
}
