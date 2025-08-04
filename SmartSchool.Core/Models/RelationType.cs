using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class RelationType
    {
        //These Attributes Are The Culomns for RelationTpye Table In Database
        public int Id { get; set; }
        public string Name { get; set; }
        public Guardian Guardian { get; set; } //Navigation Properity From RelationType (1) To Guardian(1)
    }
}
