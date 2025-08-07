using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Subject
    {
        //These Attributes Are The Culomns for Subject Table In Database
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<SubjectDetail> SubjectDetails { get; set; }
    }
}
