using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Data.Configurations.BaseConfigurations;
using Pustok.Models;

namespace Pustok.Data.Configurations;

public class HeaderSliderConfiguration : BaseEntityConfiguration<HeaderSlider>
{
	public override void Configure(EntityTypeBuilder<HeaderSlider> builder)
	{
		base.Configure(builder);

		builder.HasKey(hs => hs.Id);

		builder.Property(hs => hs.Name).HasColumnType("nvarchar").HasMaxLength(250).IsRequired(true);
		builder.Property(hs => hs.Description).HasColumnType("nvarchar").HasMaxLength(700).IsRequired(true);
		builder.Property(hs => hs.ImageUrl).HasColumnType("varchar").HasMaxLength(250).IsRequired(true);
		builder.Property(hs => hs.Price).HasColumnType("decimal").IsRequired(true);

		builder.HasOne(hs => hs.Product)
	   .WithOne(hs => hs.HeaderSlider)
	   .HasForeignKey<HeaderSlider>(hs=>hs.ProductId);

	}
}