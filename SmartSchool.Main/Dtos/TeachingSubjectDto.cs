using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.Dtos
{
    public class TeachingSubjectDto
    {
        public int? Id { get; set; }
        public DateTime AcademicYear { get; set; }
        public int Semster { get; set; }
        public int SubjectDetailId { get; set; }//Forign Key n to 1 With SubjectDetail Table
        public int TeacherId { get; set; }//Forign Key n to 1 With Teacher Table
    }
}
