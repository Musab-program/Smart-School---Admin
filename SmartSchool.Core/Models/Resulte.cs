using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Resulte
    {
        //These Attributes Are The Culomns for Resulte Table In Database
        public int Id { get; set; }
        public SubjectDetails SubjectDetails { get; set; }
        public int SubjectDetailsId { get; set; }//Forign Key n to 1 With SubjectDetails Table
        public Student Student { get; set; }
        public int StudentId { get; set; } //Forign Key n to 1 With Student Table
        public double Mark { get; set; }
        public string Rate { get; set; }
    }
}
