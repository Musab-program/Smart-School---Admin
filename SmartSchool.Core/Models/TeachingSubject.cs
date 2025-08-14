using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class TeachingSubject
    {
        //These Attributes Are The Culomns for TeachingSubject Table In Database
        public int Id { get; set; }
        [Required]
        public DateTime AcademicYear { get; set; }
        [Required]
        public int Semster { get; set; }
        public SubjectDetail SubjectDetail { get; set; } //Navigation proprity from SubjectDetail(1) to TeachingSubject(n) 
        public int SubjectDetailId { get; set; }//Forign Key n to 1 With SubjectDetail Table
        public Teacher Teacher { get; set; } //Navigation proprity from Teacher(1) to TeachingSubject(n) 
        public int TeacherId { get; set; }//Forign Key n to 1 With Teacher Table



    }
}
