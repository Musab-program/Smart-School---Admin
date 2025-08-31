using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartSchool.Core.Models
{
    public class RelationType
    {
        //These Attributes Are The Culomns for RelationTpye Table In Database
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Guardian> Guardians { get; set; } //Navigation Properity From RelationType (1) To Guardian(1)
    }
}
