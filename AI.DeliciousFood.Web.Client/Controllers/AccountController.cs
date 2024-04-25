using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Client.Controllers
{
    public partial class AccountController : CommonControllerBase
    {
        public IActionResult GetUserInfo()
        {
            return View();
        }

        public IActionResult GetMenberPrice()
        {

            return View();
        }
    }
}
