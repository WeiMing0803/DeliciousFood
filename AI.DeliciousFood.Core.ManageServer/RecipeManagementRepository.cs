using AI.DeliciousFood.Core.Common.Extensions;
using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using AI.DeliciousFood.Core.Common.Model;
using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AI.DeliciousFood.Core.ManageServer;

public interface IRecipeManagementRepository
{
    Task<List<RecipeManagementModel>> GetRecipeListAsync(string recipeName, string username, string recipeStatus, int offset, int limit, CancellationToken cancellationToken = default);
    Task<int> GetRecipeListCountAsync(string recipeName, string username, string recipeStatus, CancellationToken cancellationToken = default);
    Task<RecipeModel> GetRecipeAsync(Guid recipeGuid, CancellationToken cancellationToken = default);
    Task SaveRecipeComment(SaveRecipeCommentModel recipeComment, UserInfo user, CancellationToken cancellationToken = default);

}

public class RecipeManagementRepository(FoodDbContext dbContext) : IRecipeManagementRepository
{

    public async Task<List<RecipeManagementModel>> GetRecipeListAsync(string recipeName, string username, string recipeStatus, int offset, int limit, CancellationToken cancellationToken)
    {
        var query = dbContext.Recipes
         .AsNoTracking()
         .Join(dbContext.FoodUsers,
               recipe => recipe.UserId,
               user => user.Id,
               (recipe, user) => new { recipe, user })
         .Join(dbContext.RecipeStatus,
               joined => joined.recipe.Guid,
               status => status.RecipeGuid,
               (joined, status) => new { joined.recipe, joined.user, status })
         .Where(u =>
             (string.IsNullOrWhiteSpace(recipeName) || u.recipe.RecipeName.Contains(recipeName)) &&
             (string.IsNullOrWhiteSpace(username) || u.user.UserName.Contains(username)) &&
             (string.IsNullOrWhiteSpace(recipeStatus) || u.status.Status.ToString() == recipeStatus))
         .OrderByDescending(u => u.recipe.UpdateTime);

        List<RecipeManagementModel> pagedList = await query
            .Skip(offset)
            .Take(limit)
            .Select(u => new RecipeManagementModel
            {
                RecipeGuid = u.recipe.Guid,
                RecipeName = u.recipe.RecipeName,
                UserName = u.user.UserName!,
                CreateTime = u.recipe.CreateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                UpdateTime = u.recipe.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                RecipeStatus = u.status.Status.GetDescription()
            })
            .ToListAsync(cancellationToken);

        return pagedList;
    }

    public async Task<int> GetRecipeListCountAsync(string recipeName, string username, string recipeStatus, CancellationToken cancellationToken)
    {
        var query = dbContext.Recipes
            .Join(dbContext.FoodUsers,
                  recipe => recipe.UserId,
                  user => user.Id,
                  (recipe, user) => new { recipe, user })
            .Join(dbContext.RecipeStatus,
                  joined => joined.recipe.Guid,
                  status => status.RecipeGuid,
                  (joined, status) => new { joined.recipe, joined.user, status })
            .Where(u =>
                (string.IsNullOrWhiteSpace(recipeName) || u.recipe.RecipeName.Contains(recipeName)) &&
                (string.IsNullOrWhiteSpace(username) || u.user.UserName.Contains(username)) &&
                (string.IsNullOrWhiteSpace(recipeStatus) || u.status.Status.ToString() == recipeStatus));

        return await query.CountAsync(cancellationToken);
    }

    public async Task<RecipeModel> GetRecipeAsync(Guid recipeGuid, CancellationToken cancellationToken)
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
             Flavors = dbContext.BaseCategoryItem
                .Where(c => c.Guid == u.recipese.Flavors)
                .Select(c => c.Name)
                .FirstOrDefault(),
             CookingCraft = dbContext.BaseCategoryItem.
                Where(c => c.Guid == u.recipese.CookingCraft)
                .Select(c => c.Name)
                .FirstOrDefault(),
             UseKitchenUtensils = u.recipese.UseKitchenUtensils,
             Practice = u.recipese.Practice,
             Tips = u.recipese.Tips,
             CreateTime = u.recipese.CreateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
             UpdateTime = u.recipese.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
             FileNames = u.recipese.FileNames,
             ImageUrl = u.recipese.ImageUrl,
             Ingredients = JsonConvert.DeserializeObject<List<Ingredients>>(u.recipese.Ingredients)!,
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

    public async Task SaveRecipeComment(SaveRecipeCommentModel recipeComment, UserInfo user, CancellationToken cancellationToken)
    {
        await dbContext.RecipeStatus
               .Where(u => u.RecipeGuid == recipeComment.RecipeGuid)
               .ExecuteUpdateAsync(setters => setters
                   .SetProperty(u => u.Comment, recipeComment.Comment)
                   .SetProperty(u => u.Status, recipeComment.Approval ? StatusEnum.Approved : StatusEnum.NotApproved)
                   .SetProperty(u => u.UpdateTime, DateTime.Now)
                   .SetProperty(u => u.Approver, user.UserId), cancellationToken);
    }
}
