using AI.DeliciousFood.Core.Common.Model;
using AI.DeliciousFood.Core.Common.Model.RecipeCategories;
using AI.DeliciousFood.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace AI.DeliciousFood.Core.Server;

public interface ISearchRepository
{
    Task<List<RecipeModel>> GetRecipes(string query, int offset, int limit, CancellationToken cancellationToken = default);
    Task<int> GetRecipesCount(string query, CancellationToken cancellationToken = default);
}

public class SearchRepository(FoodDbContext foodDbContext) : ISearchRepository
{
    public async Task<List<RecipeModel>> GetRecipes(string query, int offset, int limit, CancellationToken cancellationToken)
    {
        List<RecipeModel> result = await foodDbContext.Recipes
            .Include(r => r.RecipeStatus)
            .AsNoTracking()
            .Where(r => (string.IsNullOrEmpty(query) || r.RecipeName.Contains(query)) &&
                   r.RecipeStatus.Status == StatusEnum.Approved)
            .OrderByDescending(r => r.CreateTime)
            .Skip(offset - 1)
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

    public async Task<int> GetRecipesCount(string query, CancellationToken cancellationToken)
    {
        int count = await foodDbContext.Recipes
            .AsNoTracking()
            .Where(r => (string.IsNullOrEmpty(query) || r.RecipeName.Contains(query)) &&
                   r.RecipeStatus.Status == StatusEnum.Approved)
            .CountAsync(cancellationToken);
        return count;
    }
}