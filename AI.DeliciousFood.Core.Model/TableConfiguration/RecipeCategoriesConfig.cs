using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.DeliciousFood.Core.Model.TableConfiguration;

public class RecipeCategoriesConfig : IEntityTypeConfiguration<RecipeCategories>
{
    public void Configure(EntityTypeBuilder<RecipeCategories> builder)
    {
        builder.ToTable("T_RecipeCategories");
        builder.HasKey(b => b.Guid).IsClustered(false);  //非聚集索引
        builder.Property(b => b.Guid).HasColumnType("uniqueidentifier").HasDefaultValueSql("NEWID()");
        builder.Property(b => b.Name).HasColumnType("nvarchar(20)").IsRequired();
        builder.Property(b => b.Category).HasColumnType("nvarchar(20)").IsRequired();
        
    }
}
