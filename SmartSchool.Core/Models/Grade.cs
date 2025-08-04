using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Grade
    {
        //These Attributes Are The Culomns for Grade Table In Database
        public int Id { get; set; }
        public string Name { get; set; }
        public string Stage { get; set; }
        public int Capacity { get; set; }
        public ICollection<SubjectDetail> SubjectDetails { get; set; }//Navigation proprity from SubjectDetail(n) to Grade(1) 
    }
}
