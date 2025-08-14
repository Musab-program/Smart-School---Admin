using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Content
    {
        //These Attributes Are The Culomns for Content Table In Database
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Descripion { get; set; }
        public byte[] AttachmentFile { get; set; }
        public string VedioUrl { get; set; }
        public SubjectDetail SubjectDetail { get; set; }//Navigation proprity from SubjectDetail(1) to Content(n) 
        public int SubjectDetailId { get; set; }
    }
}
