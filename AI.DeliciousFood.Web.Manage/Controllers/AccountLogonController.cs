using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Manage.Controllers;

public class AccountLogonController : Controller
{
    [HttpGet]
    public IActionResult LogOn()
    {
        return View();
    }

    [HttpPost]
    public IActionResult LogOn(LogOnModel model) 
    {
        return Json(new LogOnResponse(StatusCodes.Status200OK, "success"));
    }
}
