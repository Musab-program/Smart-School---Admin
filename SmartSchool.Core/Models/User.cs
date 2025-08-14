using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class User
    {
        //These Attributes Are The Culomns for User Table In Database
        public int Id { get; set; }
        [Required,MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public byte[] Password { get; set; }
        [Required, MaxLength(100)]
        public string Email  { get; set; }
        [Required, MaxLength(50)]
        public string Phone { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        [Required, MaxLength(150)]
        public string Address { get; set; }
        [Required, MaxLength(20)]
        public string gender { get; set; }

        //Forign Key 1 to n With Role Table
        public Role Role { get; set; } //Navigation Properity From User (1) To Role(n)
        public int RoleId { get; set; }
        public Guardian Guardian { get; set; } //Navigation Properity From User (1) To Guardian(1)
        public Student Student { get; set; } //Navigation Properity From User (1) To Student(1)
        public Teacher Teacher { get; set; } //Navigation Properity From User (1) To Teacher(1)
    }
}
