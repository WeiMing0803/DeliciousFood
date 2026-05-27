using AI.DeliciousFood.Core.Common.Extensions;
using AI.DeliciousFood.Core.Common.ManageModel.FrontPageManagement;
using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using AI.DeliciousFood.Core.Common.Model;
using AI.DeliciousFood.Core.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AI.DeliciousFood.Core.ManageServer;

public interface IFrontPageManagementRepository
{
    Task<List<FrontPageManagementModel>> GetRecommendedRecipeList(string type, int offset, int limit, CancellationToken cancellationToken = default);
    Task<int> GetRecipeListCountAsync(string type, CancellationToken cancellationToken = default);
    Task<RecipeModel> GetRecipeAsync(Guid recipeGuid, CancellationToken cancellationToken = default);
    Task AddRecommendedRecipe(List<SaveFrontPageRecipeModel> model, CancellationToken cancellationToken = default);
    Task CancelRecommendedRecipe(Guid guid, CancellationToken cancellationToken = default);
    Task<List<RecipeManagementModel>> GetRecipeListAsync(string recipeName, string username, string recipeStatus, int offset, int limit, CancellationToken cancellationToken = default);
    Task<int> GetRecipeListCountAsync(string recipeName, string username, string recipeStatus, CancellationToken cancellationToken = default);

}

public class FrontPageManagementRepository(FoodDbContext dbContext) : IFrontPageManagementRepository
{

    public async Task<List<FrontPageManagementModel>> GetRecommendedRecipeList(string type, int offset, int limit, CancellationToken cancellationToken)
    {
        var query = dbContext.Recommends
            .Include(r => r.Recipe)
            .Include(r => r.Recipe.User)
            .Where(r => r.Type.ToString() == type && r.IsActive)
            .AsNoTracking();

        List<FrontPageManagementModel> pagedList = await query
            .Skip(offset)
            .Take(limit)
            .Select(u => new FrontPageManagementModel
            {
                Guid = u.Guid,
                RecipeGuid = u.RecipeGuid,
                RecipeName = u.Recipe.RecipeName,
                UserName = u.Recipe.User.UserName!,
                StartTime = u.StartTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                EndTime = u.EndTime.HasValue ? u.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") : "",
                Type = u.Type.GetDescription(),
                IsActive = u.IsActive
            })
            .ToListAsync(cancellationToken);

        return pagedList;
    }

    public async Task<int> GetRecipeListCountAsync(string type, CancellationToken cancellationToken)
    {
        var query = dbContext.Recommends
            .Include(r => r.Recipe)
            .Include(r => r.Recipe.User)
            .Where(r => r.Type.ToString() == type && r.IsActive)
            .AsNoTracking();

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
             Categories = dbContext.RecipeCategories
                .Where(c => c.Guid == u.recipese.Categories)
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

    public Task AddRecommendedRecipe(List<SaveFrontPageRecipeModel> models, CancellationToken cancellationToken = default)
    {
        DateTime now = DateTime.Now;
        List<Recommend> recommends = new List<Recommend>(models.Count);

        foreach (SaveFrontPageRecipeModel item in models)
        {
            recommends.Add(new Recommend
            {
                Guid = Guid.NewGuid(),
                RecipeGuid = item.RecipeGuid,
                StartTime = now,
                EndTime = null,
                Type = Enum.Parse<RecommendType>(item.Type),
                IsActive = true
            });
        }

        dbContext.Recommends.AddRange(recommends);
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task CancelRecommendedRecipe(Guid guid, CancellationToken cancellationToken = default)
    {
        await dbContext.Recommends
               .Where(u => u.Guid == guid)
               .ExecuteUpdateAsync(setters => setters
                   .SetProperty(u => u.IsActive, false)
                   .SetProperty(u => u.EndTime, DateTime.Now), cancellationToken);
    }

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
             (string.IsNullOrWhiteSpace(recipeStatus) || u.status.Status.ToString() == recipeStatus) &&
             // 这个条件：排除出现在 Recommends 中并且 IsActive == true 的配方
             !dbContext.Recommends.Any(r => r.RecipeGuid == u.recipe.Guid && r.IsActive))
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
                (string.IsNullOrWhiteSpace(recipeStatus) || u.status.Status.ToString() == recipeStatus) &&
                // 这个条件：排除出现在 Recommends 中并且 IsActive == true 的配方
                !dbContext.Recommends.Any(r => r.RecipeGuid == u.recipe.Guid && r.IsActive));

        return await query.CountAsync(cancellationToken);
    }
}
