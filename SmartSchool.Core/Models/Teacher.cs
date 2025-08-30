using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Teacher
    {
        //These Attributes Are The Culomns for Teacher Table In Database
        public int Id { get; set; }
        [JsonIgnore]
        public User User { get; set; } //Navigation Properity From User (1) To Teacher(1)
        public int UserId { get; set; }
        public DateTime HireDate { get; set; }
        public Specialty Specialty { get; set; } //Navigation Properity From Specialty(1) To Teacher(1)
        public int SpecialtyId { get; set; } //Forign Key 1 to 1 With Specialty Table
        [Required]
        public double Salary { get; set; }
        public ICollection<TeachingSubject> TeachingSubjects { get; set; }//Navigation proprity from TeachingSubjects(n) to Teacher(1) 
        public ICollection<StudentAttendance> StudentAttendances { get; set; } //Navigation Properity From StudentAttendance(n) To Teacher(1)
        public ICollection<TeacherHoliday> TeacherHolidays { get; set; } //Navigation Properity From TeacherHoliday(n) To Teacher(1)
        public ICollection<TimeTable> TimeTables { get; set; } //Navigation Properity From TimeTablen1) To Teacher(1)
    }
}
