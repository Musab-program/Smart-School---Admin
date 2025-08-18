using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.Dtos
{
    public class SubmittedAssignmentDto
    {
        public int? Id { get; set; }
        public int StudentId { get; set; } // ForignKey
        public int AssignmentId { get; set; }
        public string FilePath { get; set; } //Show file Path in DB or Any To (don't need to stored in db)
        public double Mark { get; set; }
        public string Notes { get; set; }
    }
}
