using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Resulte
    {
        //These Attributes Are The Culomns for Resulte Table In Database
        public int Id { get; set; }
        public SubjectDetail SubjectDetail { get; set; }//Navigation proprity from SubjectDetail(1) to Resault(n) 
        public int SubjectDetailId { get; set; }//Forign Key n to 1 With SubjectDetails Table
        public Student Student { get; set; } //Navigation proprity from Student(1) to Resault(n) 
        public SubjectDetail SubjectDetails { get; set; }
        public int SubjectDetailsId { get; set; }//Forign Key n to 1 With SubjectDetails Table
        public int StudentId { get; set; } //Forign Key n to 1 With Student Table
        public double Mark { get; set; }
        public string Rate { get; set; }
    }
}
