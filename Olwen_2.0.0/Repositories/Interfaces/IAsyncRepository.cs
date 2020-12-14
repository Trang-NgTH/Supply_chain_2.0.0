using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Olwen_2._0._0.Repositories.Interfaces
{
    public interface IAsyncRepository<T> where T : class
    {

        Task<T> GetById(object id);

        Task<T> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Remove(object id);

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetFilterAsync(Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetAll();
        IEnumerable<T> GetFilter(Func<T, bool> where);

    }
}
