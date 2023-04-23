using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AI.DeliciousFood.Core.Model
{
    public class FoodDbContext : IdentityDbContext<FoodUser, FoodRole, long>
    {
        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options)
        {
            
        }
    }
}