using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.Dtos
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GradeId { get; set; } //Forign Key n to 1 With Grade Table
        public DateTime AcademicYear { get; set; }
    }
}
