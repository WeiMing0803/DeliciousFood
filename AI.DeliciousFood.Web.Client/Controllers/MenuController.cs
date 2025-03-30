using AI.DeliciousFood.Core.Server;
using AI.DeliciousFood.Web.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AI.DeliciousFood.Web.Client.Controllers
{
    public class MenuController(GlobalConfig globalConfig, IMenuRepository menuRepository) : CommonControllerBase
    {
        public IActionResult Index()
        {
            ViewBag.CookingTechniques = globalConfig.CookingTechniquesList;
            ViewBag.FlavorsList = globalConfig.FlavorsList;
            ViewBag.KitchenToolsList = globalConfig.KitchenToolsList;

           
    //            // 创建 optgroup 分组
    //            var foodGroup = new SelectListGroup { Name = "食物" };
    //            var itemsGroup = new SelectListGroup { Name = "物品" };

    //            // 创建选项，并指定它们属于哪个 optgroup
    //            var cookingTechniques = new List<SelectListItem>
    //{
    //    new SelectListItem { Text = "零食", Value = "snacks", Group = foodGroup },
    //    new SelectListItem { Text = "面包", Value = "bread", Group = foodGroup },
    //    new SelectListItem { Text = "调料", Value = "seasoning", Group = foodGroup },

    //    new SelectListItem { Text = "帐篷", Value = "tent", Group = itemsGroup },
    //    new SelectListItem { Text = "手电筒", Value = "flashlight", Group = itemsGroup },
    //    new SelectListItem { Text = "卫生纸", Value = "toiletpaper", Group = itemsGroup }
    //};
    //        }

    //        @Html.DropDownList("CookingTechniques", new SelectList(cookingTechniques, "Value", "Text", null, "Group.Name"),
    //            "没有选中任何项",
    //            new { id = "taste", @class = "form-control selectpicker" })

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveMenu([FromBody] MenuDataModel menuData)
        {
            await menuRepository.SaveRecipeAsync(menuData, UserInfo);
            return Ok(new
            {
                success = true,
                redirectUrl = Url.Action("Index", "Home")
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
}
