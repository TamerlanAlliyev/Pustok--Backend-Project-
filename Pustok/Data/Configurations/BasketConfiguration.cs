using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Pustok.Models;
using Pustok.Data.Configurations.BaseConfigurations;

namespace Pustok.Data.Configurations
{
	public class BasketConfiguration : BaseEntityConfiguration<Basket>
	{
		public override void Configure(EntityTypeBuilder<Basket> builder)
		{
			base.Configure(builder);

			builder.Property(b=>b.Count).HasColumnType("int").HasMaxLength(250).IsRequired(true);
		}
	}
}
