using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Specialty
    {
        //These Attributes Are The Culomns for Specialty Table In Database
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Qualification { get; set; }
        public Teacher Teacher { get; set; } //Navigation Properity From Specialty(1) To Teacher(1)
    }
}
