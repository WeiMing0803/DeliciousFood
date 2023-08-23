using AI.DeliciousFood.Core.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Client.Controllers
{
    public class AccountController : Controller
    {
        #region 注册

        [HttpPost]
        public IActionResult Register(LoginViewModel model)
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
