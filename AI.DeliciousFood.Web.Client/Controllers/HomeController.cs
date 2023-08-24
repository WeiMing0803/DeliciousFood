using AI.DeliciousFood.Core.Model;
using AI.DeliciousFood.Web.Client.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AI.DeliciousFood.Web.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<FoodUser> _signInManager;
        private readonly UserManager<FoodUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
            SignInManager<FoodUser> signInManager,
            UserManager<FoodUser> userManager)
        {
            _logger = logger;
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}