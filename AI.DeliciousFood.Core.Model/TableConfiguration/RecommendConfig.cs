using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.DeliciousFood.Core.Model.TableConfiguration
{
    public class RecommendConfig : IEntityTypeConfiguration<Recommend>
    {
        public void Configure(EntityTypeBuilder<Recommend> builder)
        {
            builder.ToTable("T_Recommend");
            builder.HasKey(b => b.Guid).IsClustered(false);
            builder.Property(b => b.Guid).HasColumnType("uniqueidentifier").HasDefaultValueSql("NEWID()");
            builder.Property(b => b.RecipeGuid).HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(b => b.StartTime).HasColumnType("datetime").IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(b => b.EndTime).HasColumnType("datetime").IsRequired(false);
            builder.Property(b => b.Type).HasColumnType("varchar(20)").IsRequired();
            builder.Property(b => b.IsActive).HasColumnType("bit").IsRequired();
        }
    }
}
