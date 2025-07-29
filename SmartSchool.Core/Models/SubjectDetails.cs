using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class SubjectDetails
    {
        //These Attributes Are The Culomns for SubjectDetails Table In Database
        public int Id { get; set; }
        public Subject Subject { get; set; }
        public int SubjectId { get; set; }//Forign Key 1 to n With Subject Table
        public Grade Grade { get; set; }
        public int GradeId { get; set; }//Forign Key n to 1 With Grade Table
        public bool IsActive { get; set; }
    }
}
