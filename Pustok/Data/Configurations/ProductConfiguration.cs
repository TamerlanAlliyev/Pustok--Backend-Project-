using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Data.Configurations.BaseConfigurations;
using Pustok.Models;

namespace Pustok.Data.Configurations
{
	public class ProductConfiguration: BaseEntityConfiguration<Product>
	{
		public override void Configure(EntityTypeBuilder<Product> builder)
		{
			base.Configure(builder);

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Name).HasColumnType("nvarchar").HasMaxLength(200).IsRequired(true);
			builder.Property(p => p.Description).HasColumnType("nvarchar").HasMaxLength(700).IsRequired(false);
			builder.Property(p => p.ProductCode).HasColumnType("nvarchar").HasMaxLength(100).IsRequired(false);
			builder.Property(p => p.ExTax).HasColumnType("decimal").IsRequired(true);
			builder.Property(p => p.Price).HasColumnType("decimal").IsRequired(true);
			builder.Property(p => p.DiscountPrice).HasColumnType("decimal").IsRequired(false);
			builder.Property(p => p.RewardPoint).HasColumnType("int").IsRequired(false);
			builder.Property(p => p.Count).HasColumnType("int").IsRequired(true);
			//builder.Property(p => p.Availability).HasColumnType("bit");

			builder.ToTable("Products");

			builder.HasMany(t => t.ProductTag)
				.WithOne(p => p.Product);
		}
	}
}
