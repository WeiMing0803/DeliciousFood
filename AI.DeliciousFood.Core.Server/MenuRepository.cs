using AI.DeliciousFood.Core.Common.Model;
using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;
using AI.DeliciousFood.Web.Client.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AI.DeliciousFood.Core.Server;

public interface IMenuRepository
{
    Task SaveRecipeAsync(MenuDataModel menuData, UserInfo user, CancellationToken cancellationToken = default);

    List<SelectListItem> GetBaseCategory(string baseCategoryName);
}


public class MenuRepository(GenericRepository<FoodDbContext> dbContext, FoodDbContext foodDbContext, ILogger<MenuRepository> logger) : IMenuRepository
{
    public async Task SaveRecipeAsync(MenuDataModel menuData, UserInfo user, CancellationToken cancellationToken)
    {
        using var transaction = await foodDbContext.Database.BeginTransactionAsync();
        try
        {
            bool isNew = menuData.RecipeGuid == null;
            Guid recipeGuid = menuData.RecipeGuid ?? Guid.NewGuid();

            Recipe recipe = isNew
                ? CreateNewRecipe(recipeGuid, user.UserId)
                : await GetExistingRecipeAsync(recipeGuid, cancellationToken);

            UpdateRecipeFields(recipe, menuData);

            if (isNew)
                await foodDbContext.AddAsync(recipe, cancellationToken);
            else
            {
                DeleteSpecifiedImages(recipe, menuData.DeletedFiles);
                foodDbContext.Update(recipe);
            }

            string folderPath = isNew 
                ? BuildImageFolderPath(menuData.RecipeName)
                : recipe.ImageUrl;
            await SaveRecipeImagesAsync(folderPath, menuData.Files, cancellationToken);

            if (isNew)
                recipe.ImageUrl = folderPath;
            else
                UpdateRecipeFileNamesFromFolder(recipe);

            await AddOrUpdateRecipeStatusAsync(recipeGuid, menuData.IsDraft, cancellationToken);

            await foodDbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(ex, "保存菜谱失败");
            throw new ApplicationException("保存菜谱失败", ex);
        }
    }

    private Recipe CreateNewRecipe(Guid recipeGuid, long userId)
    {
        return new Recipe
        {
            Guid = recipeGuid,
            UserId = userId
        };
    }

    private async Task<Recipe> GetExistingRecipeAsync(Guid recipeGuid, CancellationToken cancellationToken)
    {
        var recipe = await foodDbContext.Recipes.FirstOrDefaultAsync(r => r.Guid == recipeGuid, cancellationToken);
        if (recipe == null)
            throw new ApplicationException("未找到要修改的菜谱");
        return recipe;
    }

    private void UpdateRecipeFields(Recipe recipe, MenuDataModel menuData)
    {
        recipe.RecipeName = menuData.RecipeName;
        recipe.FileNames = string.Join(",", menuData.Files.Select(f => f.FileName));
        recipe.RecipeDescription = menuData.Description;
        recipe.RoductionDifficulty = menuData.ProductionDifficulty;
        recipe.TasksTime = menuData.NeedsTime;
        recipe.Flavors = menuData.Taste;
        recipe.CookingCraft = menuData.CookingCraft;
        recipe.UseKitchenUtensils = string.Join(',', menuData.KitchenUtensils);
        recipe.Ingredients = JsonConvert.SerializeObject(menuData.IngredientsDetails);
        recipe.Practice = menuData.Steps;
        recipe.Tips = menuData.Tips;
    }

    private string BuildImageFolderPath(string recipeName)
    {
        return Path.Combine(
            "Images",
            DateTime.Now.ToString("yyyyMM"),
            DateTime.Now.Day.ToString(),
            $"{recipeName}_{DateTime.Now:HHmmssff}"
        );
    }

    private async Task SaveRecipeImagesAsync(string folderPath, List<FileUpload> files, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(folderPath);
        foreach (var file in files)
        {
            string filePath = Path.Combine(folderPath, file.FileName);
            await File.WriteAllBytesAsync(filePath, file.FileBytes, cancellationToken);
        }
    }

    private void DeleteSpecifiedImages(Recipe recipe, string[] deletedFiles)
    {
        if (deletedFiles == null || deletedFiles.Length == 0) return;

        string folderPath = recipe.ImageUrl;
        List<string> existingFileNames = recipe.FileNames?.Split(',').ToList() ?? new List<string>();

        foreach (var fileName in deletedFiles)
        {
            string filePath = Path.Combine(folderPath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            existingFileNames.Remove(fileName);
        }

        recipe.FileNames = string.Join(",", existingFileNames);
    }

    private void UpdateRecipeFileNamesFromFolder(Recipe recipe)
    {
        string folderPath = recipe.ImageUrl;
        if (!Directory.Exists(folderPath))
        {
            recipe.FileNames = string.Empty;
            return;
        }

        List<string?> remainingFiles = Directory.GetFiles(folderPath)
                                      .Select(Path.GetFileName)
                                      .ToList();

        recipe.FileNames = string.Join(",", remainingFiles);
    }


    private async Task AddOrUpdateRecipeStatusAsync(Guid recipeGuid, bool isDraft, CancellationToken cancellationToken)
    {
        var existingStatus = await foodDbContext.RecipeStatus
            .FirstOrDefaultAsync(s => s.RecipeGuid == recipeGuid, cancellationToken);

        if (existingStatus != null)
        {
            // 修改已有状态
            existingStatus.Status = isDraft ? StatusEnum.Draft : StatusEnum.UnderReview;
            existingStatus.Approver = null; // 如有需要可以保留原 Approver
            foodDbContext.Update(existingStatus);
        }
        else
        {
            // 新建状态
            RecipeStatus status = new()
            {
                RecipeGuid = recipeGuid,
                Status = isDraft ? StatusEnum.Draft : StatusEnum.UnderReview,
                Approver = null
            };
            await foodDbContext.AddAsync(status, cancellationToken);
        }
    }



    public List<SelectListItem> GetBaseCategory(string baseCategoryName)
    {
        List<SelectListItem> selectListItems = foodDbContext.BaseCategory
            .AsNoTracking()
            .Where(x => x.Name == baseCategoryName)
            .Include(x => x.BaseCategoryItems)
            .SelectMany(x => x.BaseCategoryItems)
            .Select(x => new SelectListItem
            {
                Value = x.Guid.ToString(),
                Text = x.Name,
                Group = new SelectListGroup
                {
                    Name = x.Type
                }
            })
            .ToList();

        return selectListItems;
    }
}