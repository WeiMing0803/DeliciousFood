using AI.DeliciousFood.Core.Common.Model;
using AI.DeliciousFood.Core.Common.Model.Home;
using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AI.DeliciousFood.Core.Server;

public interface IHomeRepository
{    
    Task<List<RecommendViewModel>> GetRecommend(RecommendType recommendType, CancellationToken cancellationToken = default);
}


public class HomeRepository(GenericRepository<FoodDbContext> dbContext, FoodDbContext foodDbContext, ILogger<MenuRepository> logger) : IHomeRepository
{
    public async Task<List<RecommendViewModel>> GetRecommend(RecommendType recommendType, CancellationToken cancellationToken)
    {
        var query = foodDbContext.Recommends
            .Where(x => x.Type == recommendType && x.IsActive)
            .Include(r => r.Recipe)
                .ThenInclude(u => u.User);

        List<Recommend> data = await query
            .OrderByDescending(r => r.StartTime)
            .ToListAsync(cancellationToken);

        List<RecommendViewModel> result = data.Select(r => new RecommendViewModel
        {
            Guid = r.Guid,
            RecipeGuid = r.RecipeGuid,
            Type = r.Type,
            UserName = r.Recipe.User.UserName,
            RecipeName = r.Recipe.RecipeName,
            RecipeDescription = r.Recipe.RecipeDescription,
            FileName = r.Recipe.FileNames?.Split(',').First(),
            ImageUrl = r.Recipe.ImageUrl,
            CreatedAt = r.StartTime
        }).ToList();

        return result;
    }
}