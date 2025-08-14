using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.Dto
{
    public class StudentAttendanceDto
    {
        public int? Id { get; set; }
        public int StudentId { get; set; } 
        public DateTime AttendanceDate { get; set; }
        public string Status { get; set; }
        public int TeacherId { get; set; }
    }
}
