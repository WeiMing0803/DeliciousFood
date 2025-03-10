using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using AI.DeliciousFood.Core.ManageServer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AI.DeliciousFood.Web.Manage.Controllers;

public class RecipeManagement(IRecipeManagementRepository recipeManagementRepository) : Controller
{
    public IActionResult RecipeList()
    {
        return View();
    }

    public async Task<IActionResult> GetRecipeList(int limit, int offset)
    {
        List<RecipeManagementModel> recipeList = await recipeManagementRepository.GetRecipeListAsync();

        // 获取总数
        int total = recipeList.Count;

        // 分页
        var pagedResult = recipeList
            .Skip(offset)
            .Take(limit)
            .ToList();

        // 返回包含 total 和 rows 的对象
        return Json(new { total, rows = pagedResult });
    }

    public async Task<IActionResult> GetRecipe(Guid recipeGuid)
    {
        RecipeModel recipe = await recipeManagementRepository.GetRecipeAsync(recipeGuid);
        return View(recipe);
    }
}
