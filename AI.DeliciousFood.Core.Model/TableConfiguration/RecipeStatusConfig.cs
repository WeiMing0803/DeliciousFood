using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.DeliciousFood.Core.Model.TableConfiguration;

public class RecipeStatusConfig : IEntityTypeConfiguration<RecipeStatus>
{
    public void Configure(EntityTypeBuilder<RecipeStatus> builder)
    {
        builder.ToTable("T_RecipeStatus");
        builder.HasKey(b => b.Guid).IsClustered(false);
        builder.Property(b => b.Guid).HasColumnType("uniqueidentifier").HasDefaultValueSql("NEWID()");
        builder.Property(b => b.RecipeGuid).HasColumnType("uniqueidentifier");
        builder.Property(b => b.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(b => b.Approver).HasColumnType("bigint");
        builder.Property(b => b.CreateTime).HasColumnType("datetime").IsRequired().HasDefaultValueSql("GETDATE()");
        builder.Property(b => b.UpdateTime).HasColumnType("datetime").IsRequired().HasDefaultValueSql("GETDATE()");
        

        builder.HasOne(b => b.ApproverUser) // rs 代表 T_RecipeStatus 的一个实例
            .WithMany(u => u.ApprovedRecipeStatuses)// u 代表 User 的一个实例
            .HasForeignKey(b => b.Approver) // 指定外键列
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.Recipe)
            .WithOne(u => u.RecipeStatus)
            .HasForeignKey<RecipeStatus>(b => b.RecipeGuid)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
