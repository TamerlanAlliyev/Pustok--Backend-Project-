using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Pustok.Models;
using Pustok.Data.Configurations.BaseConfigurations;

namespace Pustok.Data.Configurations
{
	public class WishConfiguration : BaseEntityConfiguration<Wish>
	{
		public override void Configure(EntityTypeBuilder<Wish> builder)
		{
			base.Configure(builder);

		}
	}
}
