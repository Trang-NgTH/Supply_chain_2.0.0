using Olwen_2._0._0.DependencyInjection;
using Olwen_2._0._0.Model;
using Olwen_2._0._0.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Olwen_2._0._0.Repositories.Implements
{
    public class BaseAsyncRepository<T> : IAsyncRepository<T> where T : class
    {
        protected DbEntities dbcontext;

        public BaseAsyncRepository()
        {
            dbcontext = new DbEntities();
        }

        public async Task<T> Add(T entity)
        {
            try
            {
                dbcontext.Set<T>().Add(entity);
                await dbcontext.SaveChangesAsync();
                return entity;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbcontext.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
           try
           {
                return await dbcontext.Set<T>().FindAsync(id);
           }
           catch
           {
                return null;
           }
        }

        public async Task<IEnumerable<T>> GetFilterAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await dbcontext.Set<T>().Where(predicate).ToListAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Remove(object id)
        {
            try
            {
                T obj = dbcontext.Set<T>().Find(id);
                if (obj != null)
                {
                    dbcontext.Set<T>().Remove(obj);
                    await dbcontext.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                dbcontext = new DbEntities();
                dbcontext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                await dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<T> GetAll()
        {
            return dbcontext.Set<T>().ToList();
        }

        public IEnumerable<T> GetFilter(Func<T, bool> where)
        {
            return dbcontext.Set<T>().Where(where).ToList();
        }
    }
}
