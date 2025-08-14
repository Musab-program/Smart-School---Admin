using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Grade
    {
        //These Attributes Are The Culomns for Grade Table In Database
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Stage { get; set; }
        public int Capacity { get; set; }
        public ICollection<SubjectDetail> SubjectDetails { get; set; }//Navigation proprity from SubjectDetail(n) to Grade(1) 
        public ICollection<Group> Groups { get; set; } //Navigation Properity From Group (n) To Grade(1)
    }
}
