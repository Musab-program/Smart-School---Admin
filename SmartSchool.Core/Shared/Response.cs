using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Core.Shared
{
    public class Response<T>
    {
        public object? Data { get ; set; } 
        public long? Code { get; set; }
        public string Message { get; set; }
    }
}
