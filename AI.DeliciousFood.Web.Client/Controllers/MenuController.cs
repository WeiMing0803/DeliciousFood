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

        public IActionResult UploadImage()
        {
            var data = new
            {
                Message = "处理成功",
                Data = "good" 
            };
            return Ok(data);
        }
    }
}
