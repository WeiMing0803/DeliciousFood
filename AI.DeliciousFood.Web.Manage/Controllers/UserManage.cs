using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Manage.Controllers
{
    public class UserManage : Controller
    {
        public IActionResult UserList(int page = 1, int item = 10)
        {
            return View();
        }
    }
}
