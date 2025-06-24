using AI.DeliciousFood.Core.Server;
using AI.DeliciousFood.Web.Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Client.Controllers;

public class MenuController(GlobalConfig globalConfig, IMenuRepository menuRepository) : CommonControllerBase
{
    public IActionResult Index()
    {

        ViewBag.CookingTechniques = menuRepository.GetBaseCategory("口味");
        ViewBag.FlavorsList = menuRepository.GetBaseCategory("烹饪工艺");
        ViewBag.KitchenToolsList = globalConfig.KitchenToolsList;
        ViewBag.RecipeCategories = menuRepository.GetRecipeCategories();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SaveMenu([FromBody] MenuDataModel menuData)
    {
        await menuRepository.SaveRecipeAsync(menuData, UserInfo);
        return Ok(new
        {
            success = true,
            redirectUrl = Url.Action("GetUserInfo", "Account")
        });
    }

    public async Task<IActionResult> UploadImage()
    {
        IFormFile imgFile = HttpContext.Request.Form.Files[0];
        using MemoryStream memoryStream = new();
        await imgFile.CopyToAsync(memoryStream);
        byte[] fileBytes = memoryStream.ToArray();
        return Ok(new { FileName = imgFile.FileName, FileBytes = fileBytes });
    }
}
