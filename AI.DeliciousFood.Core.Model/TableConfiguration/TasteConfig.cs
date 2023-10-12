using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.DeliciousFood.Core.Model.TableConfiguration
{
    public class TasteConfig : IEntityTypeConfiguration<Taste>
    {
        public void Configure(EntityTypeBuilder<Taste> builder)
        {
            builder.ToTable("T_Taste");
            builder.HasKey(b => b.Guid).IsClustered(false);
            builder.Property(b => b.Guid).HasColumnType("uniqueidentifier");
            builder.Property(b => b.TasteName).HasColumnType("nvarchar(50)").IsRequired();
        }
    }
}
