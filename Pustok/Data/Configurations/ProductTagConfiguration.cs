using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Models;

namespace Pustok.Data.Configurations
{
	public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
	{
		public void Configure(EntityTypeBuilder<ProductTag> builder)
		{
			builder.HasKey(t => t.Id);

			//builder.Property(t => t.Count)
			//	   .HasColumnType("int")
			//	   .IsRequired(true);


			builder.HasOne(pt => pt.Product)
				.WithMany(pt => pt.ProductTag)
				.HasForeignKey(pt => pt.ProductId);

			builder.HasOne(pt => pt.Tag)
				.WithMany(pt => pt.ProductTag)
				.HasForeignKey(pt => pt.TagId);
		}
	}
}
