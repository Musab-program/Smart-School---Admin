using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class SubjectDetail
    {
        //These Attributes Are The Culomns for SubjectDetails Table In Database
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public Grade Grade { get; set; }
        public int GradeId { get; set; }//Forign Key n to 1 With Grade Table
        public Subject Subject { get; set; }
        public int SubjectId { get; set; }//Forign Key 1 to n With Subject Table
        public ICollection<Assignment> Assignments { get; set; }//Navigation proprity from Assignment(n) to SubjectDetail(1) 
        public ICollection<Exam> Exams { get; set; }//Navigation proprity from Exams(n) to SubjectDetail(1) 
        public ICollection<Resulte> Resultes  { get; set; }//Navigation proprity from Resultes(n) to SubjectDetail(1) 
        public ICollection<Content> Contents { get; set; }//Navigation proprity from Contents(n) to SubjectDetail(1) 
        public ICollection<TeachingSubject> TeachingSubjects { get; set; }//Navigation proprity from TeachingSubjects(n) to SubjectDetail(1) 
        public ICollection<TimeTable> TimeTables { get; set; }//Navigation proprity from TimeTables(n) to SubjectDetail(1) 
    }
}
