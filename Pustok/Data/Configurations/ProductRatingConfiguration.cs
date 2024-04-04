using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Data.Configurations.BaseConfigurations;
using Pustok.Models.BaseEntitys;

namespace Pustok.Models.Configurations
{
	public class ProductRatingConfiguration : BaseEntityConfiguration<ProductRating>
	{
		public override void Configure(EntityTypeBuilder<ProductRating> builder)
		{
			builder.HasKey(pr => pr.Id);

			builder.Property(pr => pr.Rating).HasColumnType("Int").HasMaxLength(5).IsRequired(true);
			builder.Property(pr => pr.Comment).HasColumnType("nvarchar").HasMaxLength(250).IsRequired(true);
		}
	}
}
