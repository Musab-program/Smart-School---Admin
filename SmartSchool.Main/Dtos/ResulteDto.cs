using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.Dtos
{
    public class ResulteDto
    {
        public int Id { get; set; }
        public int SubjectDetailId { get; set; }//Forign Key n to 1 With SubjectDetails Table
        public int StudentId { get; set; } //Forign Key n to 1 With Student Table
        public double Mark { get; set; }
        public string Rate { get; set; }
    }
}
