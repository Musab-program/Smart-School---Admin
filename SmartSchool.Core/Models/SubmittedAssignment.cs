using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class SubmittedAssignment
    {
        public int Id { get; set; }
        public Student Student { get; set; } //Navigation proprity from Student(1) to SubmittedAssignment(n)
        public int StudentId { get; set; } // ForignKey
        public Assignment Assignment { get; set; } //Navigation proprity from Assignment(1) to SubmittedAssignment(n)
        public int AssignmentId { get; set; }
        public string FilePath { get; set; } //Show file Path in DB or Any To (don't need to stored in db)
        public double Mark { get; set; }
        public string ChekeState { get; set; }
        public string Notes { get; set; }
    }
}
