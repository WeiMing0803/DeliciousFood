using AI.DeliciousFood.Core.Common.Model.RecipeCategories;
using AI.DeliciousFood.Core.Server;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Encodings.Web;

namespace AI.DeliciousFood.Web.Client.Controllers;

public class SearchController(ISearchRepository searchRepository) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string searchQuery, int offset = 1, int limit = 12)
    {
        List<RecipeModel> recipes = await searchRepository.GetRecipes(searchQuery, offset, limit);
        int total = await searchRepository.GetRecipesCount(searchQuery);
        RecipeListViewModel RecipeList = new()
        {
            Recipes = recipes,
            pagination = new()
            {
                ActionName = "",
                ControllerName = "",
                CurrentPage = offset,
                PageSize = limit,
                TotalCount = total,
                TotalPages = (int)Math.Ceiling((double)total / limit)
            }
        };
        ViewBag.SearchQuery = searchQuery;
        return View(RecipeList);
    }

    public async Task<IActionResult> GetRecipeList(string searchQuery = null, int offset = 1, int limit = 12)
    {
        searchQuery = WebUtility.HtmlDecode(searchQuery);
        List<RecipeModel> recipes = await searchRepository.GetRecipes(searchQuery, offset, limit);
        int total = await searchRepository.GetRecipesCount(searchQuery);
        RecipeListViewModel RecipeList = new()
        {
            Recipes = recipes,
            pagination = new()
            {
                ActionName = "",
                ControllerName = "",
                CurrentPage = offset,
                PageSize = limit,
                TotalCount = total,
                TotalPages = (int)Math.Ceiling((double)total / limit)
            }
        };
        return PartialView("_RecipeList", RecipeList);
    }
}
