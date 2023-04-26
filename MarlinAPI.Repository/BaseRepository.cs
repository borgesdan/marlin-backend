using MarlinAPI.Domain.Entities;
using MarlinAPI.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MarlinAPI.Repository
{
    public class BaseRepository<T> where T : BaseEntity
    {
        protected AppDbContext? appContext;

        public AppDbContext? AppContext => appContext;

        public BaseRepository(AppDbContext context)
        {
            appContext = context;
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            appContext.Add(entity);
            await appContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            appContext.Update(entity);
            await appContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            appContext.Remove(entity);
            var result = await appContext.SaveChangesAsync();

            return result != 0;
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await appContext.Set<T>().Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await appContext.Set<T>().Where(whereExpression).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await appContext.Set<T>().ToListAsync();
        }        
    }
}