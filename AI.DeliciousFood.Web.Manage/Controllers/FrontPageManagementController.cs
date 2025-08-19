using AI.DeliciousFood.Core.Common.Extensions;
using AI.DeliciousFood.Core.Common.ManageModel.FrontPageManagement;
using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using AI.DeliciousFood.Core.Common.Model;
using AI.DeliciousFood.Core.ManageServer;
using AI.DeliciousFood.Web.Manage.Models;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Manage.Controllers;

public class FrontPageManagementController(IFrontPageManagementRepository frontPageManagementRepository) : Controller
{
    [HttpGet]
    public IActionResult RecommendedRecipes()
    {
        List<EnumOptionModel> recommendType = EnumExtensions.GetEnumDropdownItems<RecommendType>();
        ViewBag.RecommendType = recommendType;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetRecommendedRecipeList(string type, int limit, int offset)
    {
        List<FrontPageManagementModel> RecommendedRecipeList = await frontPageManagementRepository.GetRecommendedRecipeList(type, offset, limit);

        // 查询总数
        int total = await frontPageManagementRepository.GetRecipeListCountAsync(type);

        // 返回包含 total 和 rows 的对象
        return Json(new PagedResult<FrontPageManagementModel> { Total = total, Rows = RecommendedRecipeList });
    }

    /// <summary>
    /// 这个方法用于获取菜谱列表
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> GetRecipeList(string recipeName, string username, string recipeStatus, int limit, int offset)
    {
        List<RecipeManagementModel> recipeList = await frontPageManagementRepository.GetRecipeListAsync(recipeName, username, recipeStatus, offset, limit);

        // 查询总数
        int total = await frontPageManagementRepository.GetRecipeListCountAsync(recipeName, username, recipeStatus);

        // 返回包含 total 和 rows 的对象
        return Json(new PagedResult<RecipeManagementModel> { Total = total, Rows = recipeList });
    }

    [HttpPost]
    public async Task<IActionResult> AddRecommendedRecipe([FromBody] List<SaveFrontPageRecipeModel> model)
    {
        await frontPageManagementRepository.AddRecommendedRecipe(model);
        return Json(new { success = true, message = "操作成功" });
    }

    [HttpPost]
    public async Task<IActionResult> CancelRecommendedRecipe(Guid guid)
    {
        await frontPageManagementRepository.CancelRecommendedRecipe(guid);
        return Json(new { success = true, message = "操作成功" });
    }
}
