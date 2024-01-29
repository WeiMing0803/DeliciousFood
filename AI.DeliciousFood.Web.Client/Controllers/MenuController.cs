using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Client.Controllers
{
    public class MenuController(GlobalConfig globalConfig) : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.CookingTechniques = globalConfig.CookingTechniquesList;
            ViewBag.FlavorsList = globalConfig.FlavorsList;
            ViewBag.KitchenToolsList = globalConfig.KitchenToolsList;
            return View();
        }
    }
}
