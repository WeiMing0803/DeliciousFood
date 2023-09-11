using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AI.DeliciousFood.Core.Model
{
    public class FoodDbContext : IdentityDbContext<FoodUser, FoodRole, long>
    {
        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}