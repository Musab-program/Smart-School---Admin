using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Content
    {
        //These Attributes Are The Culomns for Content Table In Database
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descripion { get; set; }
        public byte[] AttachmentFile { get; set; }
        public string VedioUrl { get; set; }
    }
}
