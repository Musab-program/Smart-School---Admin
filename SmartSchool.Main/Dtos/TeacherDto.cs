using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.Dtos
{
    public class TeacherDto
    {
        //User properties 
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
        public string Phone { get; set; }
        public byte[] Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public string gender { get; set; }


        //Teacher properties
        public int? Id { get; set; }
        public int SpecialtyId { get; set; } //Forign Key 1 to 1 With Specialty Table
        public double Salary { get; set; }

    }
}
