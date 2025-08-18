using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.Dtos
{
    public class TimeTableDto
    {
        public int? Id { get; set; }
     
        public int GroupId { get; set; }//Forign Key 1 to n With Group Table
        public int SubjectDetailId { get; set; }//Forign Key 1 to n With SujectDetails Table
        public int TeacherId { get; set; }//Forign Key n to 1 With Role Table
        public string DayOfWeek { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
