using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pustok.Models;

namespace Pustok.Data
{
	public class PustokContext : IdentityDbContext<AppUser>
	{
		public PustokContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Category> Categories { get; set; } = null!;
		public DbSet<Product> Products { get; set; } = null!;
		public DbSet<Tag> Tags { get; set; } = null!;
		public DbSet<ProductTag> ProductTag { get; set; } = null!;
		public DbSet<ProductRating> ProductRating { get; set; } = null!;
		//public virtual DbSet<AppUser> AppUsers { get; set; } = null!;

		public DbSet<Wish> Wishes { get; set; } = null!;
		public DbSet<Basket> Baskets { get; set; } = null!;
		public DbSet<ProductImages> ProductImages { get; set; } = null!;

        public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(PustokContext).Assembly);
			//modelBuilder.Ignore<IdentityUser>();
			modelBuilder.Entity<IdentityUser>()
		   .HasKey(u => u.Id);
		}

	}
}
