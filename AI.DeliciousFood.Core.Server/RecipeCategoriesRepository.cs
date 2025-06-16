using AI.DeliciousFood.Core.Common.Model;
using AI.DeliciousFood.Core.Common.Model.RecipeCategories;
using AI.DeliciousFood.Core.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AI.DeliciousFood.Core.Server;

public interface IRecipeCategoriesRepository
{
    Task<List<RecipeCategory>> GetAllCategories(CancellationToken cancellationToken = default);
    Task<List<RecipeModel>> GetRecipes(string category, int offset, int limit, CancellationToken cancellationToken = default);
    Task<int> GetRecipesCount(string category, CancellationToken cancellationToken = default);
}

public class RecipeCategoriesRepository(FoodDbContext foodDbContext) : IRecipeCategoriesRepository
{
    public async Task<List<RecipeCategory>> GetAllCategories(CancellationToken cancellationToken)
    {
        List<RecipeCategory> result = await foodDbContext.RecipeCategories
            .AsNoTracking()
            .Select(rc => new RecipeCategory(
                rc.Guid.ToString(),
                rc.Name,
                rc.Category))
            .ToListAsync(cancellationToken);
        return result;
    }

    public async Task<List<RecipeModel>> GetRecipes(string category, int offset, int limit, CancellationToken cancellationToken)
    {
        List<RecipeModel> result = await foodDbContext.Recipes
            .Include(r => r.RecipeStatus)
            .AsNoTracking()
            .Where(r => (string.IsNullOrEmpty(category) || 1 == 1) &&
                   r.RecipeStatus.Status == StatusEnum.Approved)
            .OrderByDescending(r => r.CreateTime)
            .Skip(offset)
            .Take(limit)
            .Select(r => new RecipeModel
            {
                Id = r.Guid,
                Name = r.RecipeName,
                Description = r.RecipeDescription,
                ImageUrl = !string.IsNullOrWhiteSpace(r.FileNames) ? r.ImageUrl + "\\" + r.FileNames.Split(',', StringSplitOptions.None).First().ToString() : null
            })
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<int> GetRecipesCount(string category, CancellationToken cancellationToken = default)
    {
        int count = await foodDbContext.Recipes
            .AsNoTracking()
            .Where(r => (string.IsNullOrEmpty(category) || 1 == 1) &&
                   r.RecipeStatus.Status == StatusEnum.Approved)
            .CountAsync(cancellationToken);
        return count;
    }
}