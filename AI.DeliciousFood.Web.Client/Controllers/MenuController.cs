using AI.DeliciousFood.Web.Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Client.Controllers
{
    public class MenuController(GlobalConfig globalConfig) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.CookingTechniques = globalConfig.CookingTechniquesList;
            ViewBag.FlavorsList = globalConfig.FlavorsList;
            ViewBag.KitchenToolsList = globalConfig.KitchenToolsList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveMenu([FromBody] MenuDataModel menuData)
        {
            return Ok();
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
