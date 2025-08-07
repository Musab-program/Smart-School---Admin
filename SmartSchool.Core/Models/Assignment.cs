using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Assignment
    {
        //These Attributes Are The Culomns for Assignment Table In Database
        public int Id { get; set; }
        public SubjectDetail SubjectDetail { get; set; }//Navigation proprity from SubjectDetail(1) to Assignment(n) 
        public int SubjectDetailId { get; set; }//Forign Key n to 1 With SubjectDetails Table
        public Student Student { get; set; }//Navigation proprity from Student(1) to Assignment(n) 
        public int StudentId { get; set; }//Forign Key n to 1 With Student Table
        public SubjectDetail SubjectDetails { get; set; }
        public int SubjectDetailsId { get; set; }//Forign Key n to 1 With SubjectDetails Table
        public string Title { get; set; }
        public DateTime LastDate { get; set; }
        public DateTime SubmitedDate { get; set; }
        public string ChekeState { get; set; }
        public double Mark { get; set; }
    }
}
