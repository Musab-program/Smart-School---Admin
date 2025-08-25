using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.Dtos
{
    public class ContentDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Descripion { get; set; }
        public byte[] AttachmentFile { get; set; }
        public string VedioUrl { get; set; }
        public int SubjectDetailId { get; set; }
    }
}
