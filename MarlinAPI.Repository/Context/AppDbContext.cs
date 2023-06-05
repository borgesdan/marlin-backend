using MarlinAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarlinAPI.Repository.Context
{
    public class AppDbContext : DbContext
    {
        static bool isFirstRun = true;

        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<ClassEntity> Classes { get; set; }

        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            EnsureCreated(this);
        }

        public static void EnsureCreated(AppDbContext context)
        {
            if (isFirstRun)
            {
                context.Database.EnsureCreated();
                isFirstRun = false;
            }                
        }
    }
}