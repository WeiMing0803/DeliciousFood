using AI.DeliciousFood.Core.Model;
using AI.DeliciousFood.Core.Server;
using AI.DeliciousFood.Web.Client.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;

namespace AI.DeliciousFood.Web.Client.Controllers
{
    public class HomeController(IAccountRepository accountRepository) : CommonControllerBase
    {

        public IActionResult Index()
        {
            var a = UserInfo;
            //Log.Error("Hello World");
            return View();
        }

        public IActionResult GetMenberPrice()
        {
            var result = accountRepository.GetMenberPriceAsync();
            return Ok(new
            {
                success = true,
                data = result
            });
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