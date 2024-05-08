using AI.DeliciousFood.Core.Server;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Client.Controllers
{
    public partial class AccountController(IAccountRepository accountRepository) : CommonControllerBase
    {
        public IActionResult GetUserInfo()
        {
            return View();
        }

    }
}
