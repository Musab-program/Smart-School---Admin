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
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required, MaxLength(100)]
        public string Address { get; set; }
        [Required, MaxLength(100)]
        public string gender { get; set; }

        //Forign Key 1 to 1 With Role Table
        public Role Role { get; set; }
        public int RoleId { get; set; }
    }
}
