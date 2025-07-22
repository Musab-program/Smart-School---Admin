using SmartSchool.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.EF.ImplementedClasses
{
    public class BaseReporstery<T> : IBaseReporstery<T> where T : class
    {
        protected ApplicationDbContext _context;

        public BaseReporstery(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
