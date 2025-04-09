using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.DeliciousFood.Core.Model.TableConfiguration;

public class BaseCategoryConfig : IEntityTypeConfiguration<BaseCategory>
{
    public void Configure(EntityTypeBuilder<BaseCategory> builder)
    {
        builder.ToTable("T_BaseCategory");
        builder.HasKey(b => b.Id).IsClustered(true);
        builder.Property(b => b.Id).HasColumnType("int").UseIdentityColumn();
        builder.Property(b => b.Name).HasColumnType("nvarchar(20)").IsRequired();

        // 配置一对多关系：BaseCategory -> BaseCategoryItem
        builder.HasMany(b => b.BaseCategoryItems)
            .WithOne(b => b.BaseCategory)
            .HasForeignKey(b => b.BaseCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
