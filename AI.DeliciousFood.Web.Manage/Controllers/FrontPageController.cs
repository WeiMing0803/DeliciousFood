using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Manage.Controllers;

public class FrontPageController : Controller
{
    public IActionResult RecommendedRecipes()
    {
        return View();
    }
}
