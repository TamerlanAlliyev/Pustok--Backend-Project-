using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Models.BaseEntitys;

namespace Pustok.Data.Configurations.BaseConfigurations;

public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseAuditable
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(m => m.Id).HasColumnType("int").UseIdentityColumn(1, 1);
        builder.Property(m => m.Created).HasColumnType("datetime").IsRequired();
        builder.Property(m => m.ModifiedBy).HasColumnType("int");
        builder.Property(m => m.Modified).HasColumnType("datetime");
        builder.Property(m => m.IsDeleted).HasColumnType("bit").IsRequired();
        builder.Property(m => m.IPAddress).HasColumnType("varchar(100)").IsRequired();
    }
}
