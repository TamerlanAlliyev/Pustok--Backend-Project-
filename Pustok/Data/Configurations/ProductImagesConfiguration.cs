using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Data.Configurations.BaseConfigurations;
using Pustok.Models;

namespace Pustok.Data.Configurations
{
	public class ProductImagesConfiguration : BaseEntityConfiguration<ProductImages>
	{
		public override void Configure(EntityTypeBuilder<ProductImages> builder)
		{
			base.Configure(builder);

			builder.HasKey(pi=>pi.Id);

			builder.Property(pi => pi.Url).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
			builder.Property(pi => pi.IsMain).HasColumnType("bit");
			builder.Property(pi => pi.IsHover).HasColumnType("bit");

			builder.HasOne(pi=>pi.Product)
				   .WithMany(pi=>pi.Images)
				   .HasForeignKey(pi=>pi.ProductId);
		}
	}
}
