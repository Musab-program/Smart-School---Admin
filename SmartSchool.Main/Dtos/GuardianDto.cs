using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.Dtos
{
    public class GuardianDto
    {
        public int Id { get; set; }
        public int UserId { get; set; } //Forign With User Table
        public int RelationTypeId { get; set; } //Forign Key 1 to 1 With RelationType Table
        public string SecondryPhone { get; set; }
    }
}
