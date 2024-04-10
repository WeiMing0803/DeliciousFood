using AI.DeliciousFood.Core.Common.Model;
using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;
using AI.DeliciousFood.Web.Client.Models;
using Newtonsoft.Json;

namespace AI.DeliciousFood.Core.Server
{
    public interface IMenuRepository
    {
        Task SaveRecipeAsync(MenuDataModel menuData, UserInfo user, CancellationToken cancellationToken = default);
    }

    public class MenuRepository(GenericRepository<FoodDbContext> repository) : IMenuRepository
    {
        public async Task SaveRecipeAsync(MenuDataModel menuData, UserInfo user, CancellationToken cancellationToken)
        {
            Recipe recipe = new Recipe()
            {
                UserId = user.UserId,
                RecipeName = menuData.RecipeName,
                FileNames = "asdfdsfds",
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
            await repository.AddAsync(recipe);
        }
    }
}