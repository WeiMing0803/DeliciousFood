using AI.DeliciousFood.Core.Common.Model.RecipeCategories;
using AI.DeliciousFood.Core.Server;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Client.Controllers;

public class RecipeCategoriesController(IRecipeCategoriesRepository recipeCategoriesRepository) : Controller
{
    public async Task<IActionResult> Index(string category = null, int offset = 0, int limit = 12)
    {
        List<RecipeCategory> categories = await recipeCategoriesRepository.GetAllCategories();
        List<RecipeModel> recipes = await recipeCategoriesRepository.GetRecipes(category, offset, limit);
        int total = await recipeCategoriesRepository.GetRecipesCount(category);
        RecipeCategoriesIndexViewModel vm = new RecipeCategoriesIndexViewModel
        {
            Categories = categories,
            RecipeList = new RecipeListViewModel
            {
                Recipes = recipes,
                Page = 1,
                PageSize = 12,
                Total = total
            }
        };
        return View(vm);
    }
}
