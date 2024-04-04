using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Data.Configurations.BaseConfigurations;
using Pustok.Models;

namespace Pustok.Data.Configurations
{
    public class CategoryConfiguration:BaseEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);

            builder.HasKey(x => x.Id);
            builder.Property(c => c.Name).HasColumnType("nvarchar").HasMaxLength(150).IsRequired(true);
            builder.Property(c=>c.ParentCategoryId).HasColumnType("int").IsRequired(false);
            builder.ToTable("Categories");

            builder
                 .HasOne(c => c.ParentCategories)
                 .WithMany(c => c.ChildCategories)
                 .HasForeignKey(c => c.ParentCategoryId);
        }
    }
}
