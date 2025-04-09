using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.DeliciousFood.Core.Model.TableConfiguration;

public class BaseCategoryItemConfig : IEntityTypeConfiguration<BaseCategoryItem>
{
    public void Configure(EntityTypeBuilder<BaseCategoryItem> builder)
    {
        builder.ToTable("T_BaseCategoryItem");
        builder.HasKey(b => b.Guid).IsClustered(false);
        builder.Property(b => b.Guid).HasColumnType("uniqueidentifier").HasDefaultValueSql("NEWID()");
        builder.Property(b => b.BaseCategoryId).HasColumnType("int").IsRequired();
        builder.Property(b => b.Type).HasColumnType("nvarchar(50)").IsRequired();
        builder.Property(b => b.Name).HasColumnType("nvarchar(50)").IsRequired();
    }
}
