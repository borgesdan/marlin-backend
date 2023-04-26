using MarlinAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarlinAPI.Repository.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<ClassEntity> Classes { get; set; }

        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
    }
}