using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Group
    {
        //These Attributes Are The Culomns for Group Table In Database
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Grade Grade { get; set; } //Navigation Properity From Grade(1) To Group(n)
        public int GradeId { get; set; } //Forign Key n to 1 With Grade Table
        [Required]
        public DateTime AcademicYear { get; set; }
        public ICollection<Student> Students { get; set; } //Navigation Properity From Student(n) To Group(1)
        public ICollection<TimeTable> TimeTables { get; set; } //Navigation Properity From TimeTable(n) To Group(1)
    }
}
