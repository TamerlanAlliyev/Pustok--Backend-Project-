using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Data.Configurations.BaseConfigurations;
using Pustok.Models;

namespace Pustok.Data.Configurations;

public class TagConfiguration : BaseEntityConfiguration<Tag>
{
	public override void Configure(EntityTypeBuilder<Tag> builder)
	{
		base.Configure(builder);

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name).HasColumnType("nvarchar").HasMaxLength(120).IsRequired(true);

		builder.ToTable("Tags");

		builder.HasMany(t => t.ProductTag)
			.WithOne(t => t.Tag);
	}
}