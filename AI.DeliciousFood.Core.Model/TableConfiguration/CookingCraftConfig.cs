using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.DeliciousFood.Core.Model.TableConfiguration
{
    public class CookingCraftConfig : IEntityTypeConfiguration<CookingCraft>
    {
        public void Configure(EntityTypeBuilder<CookingCraft> builder)
        {
            builder.ToTable("T_CookingCraft");
            builder.HasKey(b => b.Guid).IsClustered(false);
            builder.Property(b => b.Guid).HasColumnType("uniqueidentifier");
            builder.Property(b => b.CookingCraftName).HasColumnType("nvarchar(50)").IsRequired();
        }
    }
}
