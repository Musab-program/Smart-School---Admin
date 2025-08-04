using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Adminstration
    {
        //These Attributes Are The Culomns for Adminstration Table In Database
        public int Id { get; set; }
        //[Required, MaxLength(100)]
        public User User { get; set; } //Navigation Properity From User(1) To Adminstration (1)
        public int UserId { get; set; } //Forign With User Table
        public DateTime EnrollmentDate { get; set; }

    }
}
