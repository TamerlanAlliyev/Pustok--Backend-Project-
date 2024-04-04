using Microsoft.EntityFrameworkCore;
using Pustok.Models;

namespace Pustok.Data
{
    public class PustokContext:DbContext
    {
        public PustokContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PustokContext).Assembly);
        }

    }
}
