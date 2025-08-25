using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.Dtos
{
    public class GuardianDto
    {
        // Guardian Properties
        public int? Id { get; set; }
        public int RelationTypeId { get; set; } //Forign Key 1 to 1 With RelationType Table
        public string SecondryPhone { get; set; }

        // User Properties
        public int UserId { get; set; } //Forign With User Table
        public string UserName { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
        public string Phone { get; set; }
        public byte[] Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public string gender { get; set; }
    }
}
