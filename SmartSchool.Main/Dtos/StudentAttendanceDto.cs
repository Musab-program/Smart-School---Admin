using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.Dtos
{
    public class StudentAttendanceDto
    {
        public int? Id { get; set; }
        public int StudentId { get; set; } 
        public string Status { get; set; }
        public int TeacherId { get; set; }
    }
}
