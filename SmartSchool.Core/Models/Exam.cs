using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Exam
    {
        //These Attributes Are The Culomns for Exam Table In Database
        public int Id { get; set; }
        public SubjectDetail SubjectDetails { get; set; }
        public int SubjectDetailsId { get; set; }//Forign Key n to 1 With SubjectDetails Table
        public DateTime ExamDate { get; set; }
        public ExamType ExamType { get; set; }
        public int ExamTypeId { get; set; }//Forign Key 1 to n With ExamType Table
        public int LimitTime { get; set; }
    }
}
