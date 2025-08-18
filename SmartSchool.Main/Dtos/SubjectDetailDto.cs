using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.Dtos
{
    public class SubjectDetailDto
    {
        //These Attributes Are The Culomns for SubjectDetails Table In Database
        public int? Id { get; set; }
        public bool IsActive { get; set; }
        public int GradeId { get; set; }
        public int SubjectId { get; set; }
    }
}
