using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class TeacherHoliday
    {
        //These Attributes Are The Culomns for TeacherHolidays Table In Database
        public int Id { get; set; }
        public Teacher Teacher { get; set; } //Navigation Properity From Teacher(1) To TeacherHoliday(n)
        public int TeacherId { get; set; }//Forign Key n to 1 With Teacher Table
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public string Reason { get; set; }
        public bool IsAgreeded { get; set; }
    }
}
