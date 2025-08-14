using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Exam
    {
        //These Attributes Are The Culomns for Exam Table In Database
        public int Id { get; set; }

        public SubjectDetail SubjectDetail { get; set; }//Navigation proprity from SubjectDetail(1) to Exam(n) 
        public int SubjectDetailId { get; set; }//Forign Key n to 1 With SubjectDetails Table
        public Student Student { get; set; }//Navigation proprity from Student(1) to Exam(n) 
        public int StudentId { get; set; }//Forign Key n to 1 With strudent Table
        [Required]
        public DateTime ExamDate { get; set; }
        public ExamType ExamType { get; set; }//Navigation proprity from ExamType(1) to Exam(n) 
        public SubjectDetail SubjectDetails { get; set; }
        public int SubjectDetailsId { get; set; }//Forign Key n to 1 With SubjectDetails Table
        public int ExamTypeId { get; set; }//Forign Key 1 to n With ExamType Table
        public int LimitTime { get; set; }
    }
}
