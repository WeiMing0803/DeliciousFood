using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.DeliciousFood.Core.Model.TableConfiguration
{
    public class KitchenutensilsConfig : IEntityTypeConfiguration<Kitchenutensils>
    {
        public void Configure(EntityTypeBuilder<Kitchenutensils> builder)
        {
            builder.ToTable("T_Kitchenutensils");
            builder.HasKey(b => b.Guid).IsClustered(false);
            builder.Property(b => b.Guid).HasColumnType("uniqueidentifier");
            builder.Property(b => b.KitchenutensilsName).HasColumnType("nvarchar(50)").IsRequired();
        }
    }
}
