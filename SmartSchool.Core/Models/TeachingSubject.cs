using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class TeachingSubject
    {
        //These Attributes Are The Culomns for TeachingSubject Table In Database
        public int Id { get; set; }
        public Subject Subject { get; set; }
        public int SubjectId { get; set; }//Forign Key n to 1 With Subject Table
        public DateTime AcademicYear { get; set; }
        public int Semster { get; set; }
    }
}
