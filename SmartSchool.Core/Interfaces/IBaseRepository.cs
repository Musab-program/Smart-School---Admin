using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SmartSchool.Core.Consts;

namespace SmartSchool.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        // Read Operations (Queries)
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = "Ascending");

        // Write Operations (Commands)
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        void Delete(T entity); 
        void DeleteRange(IEnumerable<T> entities); 
        //void Attach(T entity);
        //void AttachRange(IEnumerable<T> entities);

        // Counting Operations (Counting and Checking)
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);

        // Sum Operations (Aggregation)
        //Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> criteria = null);
        //Task<double> AverageAsync(Expression<Func<T, double>> selector, Expression<Func<T, bool>> criteria = null);
    }
}
