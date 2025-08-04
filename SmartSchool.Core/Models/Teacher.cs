using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Teacher
    {
        //These Attributes Are The Culomns for Teacher Table In Database
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public DateTime HireDate { get; set; }
        public Specialty Specialty { get; set; }
        public int SpecialtyId { get; set; } //Forign Key 1 to 1 With Specialty Table
        public double Salary { get; set; }

        public ICollection<TeachingSubject> TeachingSubjects { get; set; }//Navigation proprity from TeachingSubjects(n) to Teacher(1) 
    }
}
