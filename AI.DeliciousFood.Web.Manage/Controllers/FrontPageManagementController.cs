using AI.DeliciousFood.Core.Common.Extensions;
using AI.DeliciousFood.Core.Common.ManageModel.FrontPageManagement;
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
}
