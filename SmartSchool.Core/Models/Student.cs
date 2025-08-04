using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Student
    {
        //These Attributes Are The Culomns for Student Table In Database
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Guardian Guardians { get; set; }
        public int GuardianId { get; set; }//Forign Key n to 1 With Guardians Table
        public Group Group { get; set; }
        public int GroupId { get; set; }//Forign Key n to 1 With Group Table
        public DateTime RegisterDate { get; set; }
        public ICollection<Assignment> Assignment { get; set; }//Navigation proprity from Assignment(n) to Student(1) 
        public ICollection<Resulte> Resultes { get; set; }//Navigation proprity from Resultes(n) to Student(1) 

    }
}
