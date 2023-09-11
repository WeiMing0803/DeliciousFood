using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.DeliciousFood.Core.Model.TableConfiguration
{
    public class MaterialConfig : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.ToTable("T_Material");
            builder.HasKey(b => b.Guid).IsClustered(false);
            builder.Property(b => b.Guid).HasColumnType("uniqueidentifier");
            builder.Property(b => b.RecipeGuid).HasColumnType("uniqueidentifier");
            builder.Property(b => b.IngredientName).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(b => b.Usage).HasColumnType("nvarchar(50)").IsRequired();

            builder.HasOne(m => m.Recipe)
                .WithMany(r => r.Materials)
                .HasForeignKey(m => m.RecipeGuid)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
