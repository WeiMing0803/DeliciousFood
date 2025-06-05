using AI.DeliciousFood.Core.Common.Extensions;
using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using AI.DeliciousFood.Core.Common.Model;
using AI.DeliciousFood.Core.ManageServer;
using AI.DeliciousFood.Web.Manage.Models;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Manage.Controllers;

public class RecipeManagementController(IRecipeManagementRepository recipeManagementRepository, GlobalConfig globalConfig) : CommonControllerBase
{
    public IActionResult RecipeList()
    {
        List<EnumOptionModel> recipeStatus = EnumExtensions.GetEnumDropdownItems<StatusEnum>();
        ViewBag.RecipeStatus = recipeStatus;
        return View();
    }

    public IActionResult ReviewRecipes()
    {
        return View();
    }

    public async Task<IActionResult> GetRecipeList(string recipeName, string username, string recipeStatus, int limit, int offset)
    {
        List<RecipeManagementModel> recipeList = await recipeManagementRepository.GetRecipeListAsync(recipeName, username, recipeStatus, offset, limit);

        // 查询总数
        int total = await recipeManagementRepository.GetRecipeListCountAsync(recipeName, username, recipeStatus);

        // 返回包含 total 和 rows 的对象
        return Json(new PagedResult<RecipeManagementModel> { Total = total, Rows = recipeList });
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
