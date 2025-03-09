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
    Task<RecipeModel> GetRecipeAsync(Guid recipeGuid, CancellationToken cancellationToken = default);

}

public class RecipeManagementRepository(GenericRepository<FoodDbContext> dbContextBase, FoodDbContext dbContext, RoleManager<FoodRole> roleManager) : IRecipeManagementRepository
{

    public async Task<List<RecipeManagementModel>> GetRecipeListAsync(CancellationToken cancellationToken = default)
    {
        List<RecipeManagementModel> recipes = await dbContext.Recipes
            .AsNoTracking()
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

    public async Task<RecipeModel> GetRecipeAsync(Guid recipeGuid, CancellationToken cancellationToken = default)
    {
        RecipeModel recipe = await dbContext.Recipes
         .AsNoTracking()
         .Where(u => u.Guid == recipeGuid)
         .Join(dbContext.FoodUsers,
               recipese => recipese.UserId,
               user => user.Id,
               (recipese, user) => new { recipese, user })
         .Join(dbContext.RecipeStatus,
               joined => joined.recipese.Guid,
               recipeStatus => recipeStatus.RecipeGuid,
               (joined, recipeStatus) => new { joined.recipese, joined.user, recipeStatus })
         .Select(u => new RecipeModel
         {
             RecipeGuid = u.recipese.Guid,
             UserName = u.user.UserName!,
             RecipeName = u.recipese.RecipeName,
             RecipeDescription = u.recipese.RecipeDescription,
             RoductionDifficulty = u.recipese.RoductionDifficulty,
             TasksTime = u.recipese.TasksTime,
             Flavors = u.recipese.Flavors,
             CookingCraft = u.recipese.CookingCraft,
             UseKitchenUtensils = u.recipese.UseKitchenUtensils,
             Practice = u.recipese.Practice,
             Tips = u.recipese.Tips,
             CreateTime = u.recipese.CreateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
             UpdateTime = u.recipese.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
             FileNames = u.recipese.FileNames,
             Ingredients = u.recipese.Ingredients,
             RecipeStatus = u.recipeStatus.Status.GetDescription()
         })
         .FirstAsync();

        // 如果没有找到结果，抛出自定义异常，避免返回空值
        if (recipe == null)
        {
            throw new KeyNotFoundException($"Recipe with Guid {recipeGuid} was not found.");
        }

        return recipe;
    }
}
