using AI.DeliciousFood.Core.Common.Extensions;
using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using AI.DeliciousFood.Core.Common.Model;
using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AI.DeliciousFood.Core.ManageServer;

public interface IRecipeManagementRepository
{
    Task<List<RecipeManagementModel>> GetRecipeListAsync(CancellationToken cancellationToken = default);

}

public class RecipeManagementRepository(GenericRepository<FoodDbContext> dbContextBase, FoodDbContext dbContext, RoleManager<FoodRole> roleManager) : IRecipeManagementRepository
{
    public async Task<List<RecipeManagementModel>> GetRecipeListAsync(CancellationToken cancellationToken = default)
    {
        List<RecipeManagementModel> recipes = await dbContext.Recipes
            .Join(dbContext.FoodUsers,
                  recipese => recipese.UserId,
                  user => user.Id,
                  (recipese, user) => new { recipese, user })
            .Join(dbContext.RecipeStatus,
                  joined => joined.recipese.Guid,
                  recipeStatus => recipeStatus.RecipeGuid,
                  (joined, recipeStatus) => new { joined.recipese, joined.user, recipeStatus })
            .OrderByDescending(u => u.recipese.UpdateTime)  // 确保有序，以支持分页
            .Select(u => new RecipeManagementModel
            {
                RecipeGuid = u.recipese.Guid,
                RecipeName = u.recipese.RecipeName,
                UserName = u.user.UserName!,
                CreateTime = u.recipese.CreateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                UpdateTime = u.recipese.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                RecipeStatus = u.recipeStatus.Status.GetDescription()
            })
            .ToListAsync(cancellationToken);
        return recipes;
    }
}
