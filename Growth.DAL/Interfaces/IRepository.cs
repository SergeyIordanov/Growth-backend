using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Growth.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        Task<T> GetAsync(Guid id);

        IQueryable<T> Find(Expression<Func<T, bool>> expression);

        Task CreateAsync(T item);

        Task UpdateAsync(T item);

        Task DeleteAsync(Guid id);
    }
}