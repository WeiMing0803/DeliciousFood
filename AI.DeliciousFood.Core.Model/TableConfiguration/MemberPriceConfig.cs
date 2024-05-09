using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.DeliciousFood.Core.Model.TableConfiguration
{
    public class MemberPriceConfig : IEntityTypeConfiguration<MemberPrice>
    {
        public void Configure(EntityTypeBuilder<MemberPrice> builder)
        {
            builder.ToTable("T_MemberPrice");
            builder.HasKey(b => b.MemberPriceGuid).IsClustered(false);  //非聚集索引
            builder.Property(b => b.MemberPriceGuid).HasColumnType("uniqueidentifier").HasDefaultValueSql("NEWID()");
            builder.Property(b => b.MemberName).HasColumnType("nvarchar(50)");
            builder.Property(b => b.Price).HasColumnType("decimal(18, 2)").IsRequired().HasDefaultValueSql("0");
        }
    }
}
