using AI.DeliciousFood.Core.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Client.Controllers
{
    public class AccountController : Controller
    {
        #region 注册

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            return View();
        }

        #endregion


        #region 登录

        [HttpGet]
        public IActionResult LogOn(string returnUrl)
        {

            return View();
        }


        [HttpPost]
        public IActionResult LogOn(LoginViewModel model, string returnUrl)
        {
            return View();
        }

        #endregion
    }
}
