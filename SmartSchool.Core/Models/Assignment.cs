using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Title { get; set; }
        public DateTime LastDate { get; set; }
        public DateTime UploadDate { get; set; } // When Teatcher submited The Assignment
        public ICollection<SubmittedAssignment> SubmittedAssignments { get; set; }//Navigation proprity from SubmittedAssignment(n) to Assignment(1) 
    }
}
