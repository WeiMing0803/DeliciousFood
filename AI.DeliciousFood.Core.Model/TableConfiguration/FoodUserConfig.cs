using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.DeliciousFood.Core.Model.TableConfiguration
{
    public class FoodUserConfig : IEntityTypeConfiguration<FoodUser>
    {
        public void Configure(EntityTypeBuilder<FoodUser> builder)
        {
            builder.ToTable("AspNetUsers");
            builder.Property(b => b.MembershipExpireAt).HasColumnType("date").HasDefaultValueSql("CAST(GETDATE() AS DATE)");
        }
    }
}
