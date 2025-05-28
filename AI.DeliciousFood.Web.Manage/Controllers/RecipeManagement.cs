using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using AI.DeliciousFood.Core.ManageServer;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Manage.Controllers;

public class RecipeManagement(IRecipeManagementRepository recipeManagementRepository, GlobalConfig globalConfig) : CommonControllerBase
{
    public IActionResult RecipeList()
    {
        return View();
    }

    public IActionResult ReviewRecipes()
    {
        return View("RecipeList");
    }

    public async Task<IActionResult> GetRecipeList(string recipeName, string username, int limit, int offset)
    {
        List<RecipeManagementModel> recipeList = await recipeManagementRepository.GetRecipeListAsync(recipeName, username);

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
        recipe.ImageUrl = globalConfig.ClientHost + recipe.ImageUrl;
        return View(recipe);
    }


    [HttpPost]
    public async Task<IActionResult> SaveRecipeComment([FromBody] SaveRecipeCommentModel recipeComment)
    {
        await recipeManagementRepository.SaveRecipeComment(recipeComment, UserInfo);
        return Ok(new { success = true });
    }
}
