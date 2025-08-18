using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.Dtos
{
    public class ExamDto
    {
        public int Id { get; set; }
        public int SubjectDetailId { get; set; }//Forign Key n to 1 With SubjectDetails Table
        public DateTime ExamDate { get; set; }
        public int ExamTypeId { get; set; }//Forign Key 1 to n With ExamType Table
        public int LimitTime { get; set; }
        public int GroupId { get; set; }

    }
}
