using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class TimeTable
    {
        //These Attributes Are The Culomns for User TimeTable In Database
        public int Id { get; set; }
        public Group Group { get; set; } //Navigation Properity From Group(1) To TimeTable(n)
        public int GroupId { get; set; }//Forign Key 1 to n With Group Table
        public SubjectDetail SubjectDetail { get; set; } //Navigation proprity from SubjectDetail(1) to TimeTable(n) 
        public int SubjectDetailId { get; set; }//Forign Key 1 to n With SujectDetails Table
        public Teacher Teacher { get; set; } //Navigation Properity From Teacher(1) To TimeTable(n)
        public int TeacherId { get; set; }//Forign Key n to 1 With Role Table
        public string DayOfWeek { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
