using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.DeliciousFood.Core.Model.TableConfiguration
{
    public class RecipeConfig : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("T_Recipe");
            builder.HasKey(b => b.Guid).IsClustered(false);
            builder.Property(b => b.Guid).HasColumnType("uniqueidentifier").HasDefaultValueSql("NEWID()");
            builder.Property(b => b.UserId).HasColumnType("bigint");
            builder.Property(b => b.RecipeName).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(b => b.FileNames).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(b => b.ImageUrl).HasColumnType("nvarchar(255)").IsRequired(false);
            builder.Property(b => b.RecipeDescription).HasColumnType("nvarchar(500)").IsRequired(false);
            builder.Property(b => b.RoductionDifficulty).HasColumnType("nvarchar(50)").IsRequired(false);
            builder.Property(b => b.TasksTime).HasColumnType("nvarchar(50)").IsRequired(false);
            builder.Property(b => b.Flavors).HasColumnType("nvarchar(50)").IsRequired(false);
            builder.Property(b => b.CookingCraft).HasColumnType("nvarchar(50)").IsRequired(false);
            builder.Property(b => b.UseKitchenUtensils).HasColumnType("nvarchar(50)").IsRequired(false);
            builder.Property(b => b.Ingredients).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(b => b.Practice).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(b => b.Tips).HasColumnType("nvarchar(500)").IsRequired(false);
            builder.Property(b => b.CreateTime).HasColumnType("datetime").IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(b => b.UpdateTime).HasColumnType("datetime").IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(b => b.IsDelete).HasColumnType("bit").IsRequired();
            builder.Property(b => b.IsApproval).HasColumnType("bit").IsRequired();
            
            builder.HasOne(b => b.User)
                .WithMany(u => u.Recipes)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
