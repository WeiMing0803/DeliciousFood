using AI.DeliciousFood.Core.Model;
using AI.DeliciousFood.Web.Client.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;

namespace AI.DeliciousFood.Web.Client.Controllers
{
    public class HomeController : CommonControllerBase
    {
        private readonly SignInManager<FoodUser> _signInManager;
        private readonly UserManager<FoodUser> _userManager;

        public HomeController(
            SignInManager<FoodUser> signInManager,
            UserManager<FoodUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            var a = UserInfo;
            //Log.Error("Hello World");
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