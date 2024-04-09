using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Models.BaseEntitys;

namespace Pustok.Data.Configurations.BaseConfigurations;

public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseAuditable
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(m => m.Id).HasColumnType("int").UseIdentityColumn(1, 1);
        builder.Property(m => m.Created).HasColumnType("datetime").IsRequired(true);
        builder.Property(m => m.CreatedBy).HasColumnType("varchar").HasMaxLength(300).IsRequired(true);
        builder.Property(m => m.ModifiedBy).HasColumnType("varchar").HasMaxLength(300).IsRequired(false);
        builder.Property(m => m.Modified).HasColumnType("datetime").IsRequired(false);
        builder.Property(m => m.IsDeleted).HasColumnType("bit").IsRequired(true);
        builder.Property(m => m.IPAddress).HasColumnType("varchar(100)").IsRequired(true);
    }
}
