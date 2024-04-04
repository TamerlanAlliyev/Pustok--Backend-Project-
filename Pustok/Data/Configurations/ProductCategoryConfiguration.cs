using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Models;

namespace Pustok.Data.Configurations
{
	public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
	{
		public void Configure(EntityTypeBuilder<ProductCategory> builder)
		{
			builder.HasKey(pc=>pc.Id);

			builder.Property(m => m.Id).HasColumnType("int").UseIdentityColumn(1, 1);

			builder.HasOne(pc=>pc.Product)
				   .WithMany(pc=>pc.ProductCategory)
				   .HasForeignKey(pc=>pc.ProducId);

			builder.HasOne(pc=>pc.Category)
				   .WithMany(pc=>pc.ProductCategory)
				   .HasForeignKey(pc=>pc.CategoryId);
		}
	}
}
