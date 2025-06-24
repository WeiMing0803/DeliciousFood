using AI.DeliciousFood.Core.Common.Model.RecipeCategories;
using AI.DeliciousFood.Core.Server;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Client.Controllers;

public class RecipeCategoriesController(IRecipeCategoriesRepository recipeCategoriesRepository) : Controller
{
    public async Task<IActionResult> Index(string category = null, int offset = 1, int limit = 12)
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
                pagination = new()
                {
                    ActionName = "Index",
                    ControllerName = "RecipeCategories",
                    CurrentPage = offset,
                    PageSize = limit,
                    TotalCount = total,
                    TotalPages = (int)Math.Ceiling((double)total / limit)
                }
            }
        };
        return View(vm);
    }

    public async Task<IActionResult> GetRecipeList(string category = null, int offset = 1, int limit = 12)
    {
        List<RecipeCategory> categories = await recipeCategoriesRepository.GetAllCategories();
        List<RecipeModel> recipes = await recipeCategoriesRepository.GetRecipes(category, offset, limit);
        int total = await recipeCategoriesRepository.GetRecipesCount(category);
        RecipeListViewModel RecipeList = new()
        {
            Recipes = recipes,
            pagination = new()
            {
                ActionName = "GetRecipeList",
                ControllerName = "RecipeCategories",
                CurrentPage = offset,
                PageSize = limit,
                TotalCount = total,
                TotalPages = (int)Math.Ceiling((double)total / limit)
            }
        };
        return PartialView("_RecipeList", RecipeList);
    }
}
