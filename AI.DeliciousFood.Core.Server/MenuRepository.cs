using AI.DeliciousFood.Core.Common.Model;
using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;
using AI.DeliciousFood.Web.Client.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AI.DeliciousFood.Core.Server;

public interface IMenuRepository
{
    Task SaveRecipeAsync(MenuDataModel menuData, UserInfo user, CancellationToken cancellationToken = default);
}


public class MenuRepository(GenericRepository<FoodDbContext> dbContext, FoodDbContext foodDbContext, ILogger<MenuRepository> logger) : IMenuRepository
{
    public async Task SaveRecipeAsync(MenuDataModel menuData, UserInfo user, CancellationToken cancellationToken)
    {
        using var transaction = await foodDbContext.Database.BeginTransactionAsync();
        try
        {
            string folderPath = Path.Combine("Images",
                         DateTime.Now.ToString("yyyyMM"),
                         DateTime.Now.Day.ToString(),
                         menuData.RecipeName + "_" + DateTime.Now.ToString("HHmmssff"));

            Guid recipeGuid = Guid.NewGuid();

            Recipe recipe = new()
            {
                Guid = recipeGuid,
                UserId = user.UserId,
                RecipeName = menuData.RecipeName,
                FileNames = string.Join(",", menuData.Files.Select(s => s.FileName)),
                ImageUrl = folderPath,
                RecipeDescription = menuData.Description,
                RoductionDifficulty = menuData.ProductionDifficulty,
                TasksTime = menuData.NeedsTime,
                Flavors = menuData.Taste,
                CookingCraft = menuData.CookingCraft,
                UseKitchenUtensils = string.Join(',', menuData.KitchenUtensils),
                Ingredients = JsonConvert.SerializeObject(menuData.IngredientsDetails),
                Practice = menuData.Steps,
                Tips = menuData.Tips
            };
            await dbContext.AddAsync(recipe);

            Directory.CreateDirectory(folderPath);
            foreach (FileUpload file in menuData.Files)
            {
                await File.WriteAllBytesAsync(Path.Combine(folderPath, file.FileName), file.FileBytes);
            }

            RecipeStatus recipeStatus = new()
            {
                RecipeGuid = recipeGuid,
                Status = StatusEnum.UnderReview,
                Approver = null
            };
            await dbContext.AddAsync(recipeStatus);

            await foodDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            // 记录日志 (例如，使用 ILogger)
            logger.LogError(ex, "创建菜谱失败");
            // 可以根据需要抛出自定义异常，或者返回错误信息
            throw new ApplicationException("创建菜谱失败", ex);
        }
    }
}