using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Manage.Controllers
{
    public class AccountLogonController : Controller
    {
        [HttpGet]
        public IActionResult LogOn()
        {
            return View();
        }
    }
}
