using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.Dtos
{
    public class TeacherDto
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int SpecialtyId { get; set; } //Forign Key 1 to 1 With Specialty Table
        public double Salary { get; set; }
        
    }
}
