using MarlinAPI.Domain.Entities;
using MarlinAPI.Repository.Context;

namespace MarlinAPI.Repository
{
    public class StudentRepository : BaseRepository<StudentEntity>
    {
        public StudentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
