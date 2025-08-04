using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Guardian
    {
        //These Attributes Are The Culomns for Guardians Table In Database
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; } //Forign With User Table
        public RelationType RelationType { get; set; }
        public int RelationTypeId { get; set; } //Forign Key 1 to 1 With RelationType Table
        public string SecondryPhone { get; set; }
        public User User { get; set; } //Navigation Properity From User (1) To Guardian(1)
        public int UserId { get; set; } //Forign With User Table
        public RelationType RelationType { get; set; } //Navigation Properity From RelationType (1) To Guardian(1)
        public int RelationTypeId { get; set; } //Forign Key 1 to 1 With RelationType Table
        public string SecondryPhone { get; set; }
        public ICollection<Student> Students { get; set; } //Navigation Properity From Student(n) To Guardian(1) 
    }
}
