using MarlinAPI.Domain.Entities;
using MarlinAPI.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MarlinAPI.Repository
{
    public class ClassRepository : BaseRepository<ClassEntity>
    {
        public ClassRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ClassEntity?> GetAsync(int id, bool includeStudents)
        {
            var query = appContext.Classes.Where(c => c.Id == id);

            if (includeStudents)
                query = query.Include(c => c.Students);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<ClassEntity?> GetAsync(string registry, bool includeStudents)
        {
            var query = appContext.Classes.Where(c => c.Registry == registry);

            if (includeStudents)
                query = query.Include(c => c.Students);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<ClassEntity?> GetAsync(Expression<Func<ClassEntity, bool>> whereExpression, bool includeStudents)
        {
            var query = appContext.Classes.Where(whereExpression);

            if (includeStudents)
                query = query.Include(c => c.Students);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ClassEntity>> GetAllAsync(Expression<Func<ClassEntity, bool>> whereExpression, bool includeStudents)
        {
            var query = appContext.Classes.Where(whereExpression);

            if (includeStudents)
                query = query.Include(c => c.Students);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ClassEntity>> GetAllByStudentRegistryAsync(string studentRegistry, bool includeStudents = false)
        {
            var query = appContext.Classes.Where(c => c.Students.FirstOrDefault(s => s.Registry == studentRegistry) != null);

            if (includeStudents)
                query = query.Include(c => c.Students);

            return await query.ToListAsync();
        }
    }
}
