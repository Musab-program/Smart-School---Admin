using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class ExamType
    {
        //These Attributes Are The Culomns for ExamType Table In Database
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Year { get; set; }
        public ICollection<Exam> Exams { get; set; } //Navigation proprity from Exam(n) to ExamType(1) 
    }
}
