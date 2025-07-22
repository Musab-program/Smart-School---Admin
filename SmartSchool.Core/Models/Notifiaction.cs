using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class Notifiaction
    {
        //These Attributes Are The Culomns for Notifiaction Table In Database
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; } //Forign Key n to 1 With user Table
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
