using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AI.DeliciousFood.Core.Model;

public class FoodDbContext : IdentityDbContext<FoodUser, FoodRole, long>
{
    public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options)
    {
        
    }

    public DbSet<FoodUser> FoodUsers { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<RecipeStatus> RecipeStatus { get; set; }
    public DbSet<MemberPrice> MemberPrice { get; set; }
    public DbSet<BaseCategory> BaseCategory { get; set; }
    public DbSet<BaseCategoryItem> BaseCategoryItem { get; set; }
    public DbSet<RecipeCategories> RecipeCategories { get; set; }
    public DbSet<Recommend> Recommends { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }

}