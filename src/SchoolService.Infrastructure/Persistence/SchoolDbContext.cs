using Microsoft.EntityFrameworkCore;
using SchoolService.Domain.Entities;
using SchoolService.Infrastructure.Configurations;

namespace SchoolService.Infrastructure.Persistence
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options ) : base(options)
        {
        }
        public DbSet<School> Schools { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new SchoolConfiguration());
        }
    }
}
