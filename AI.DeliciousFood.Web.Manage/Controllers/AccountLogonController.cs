using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using AI.DeliciousFood.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AI.DeliciousFood.Web.Manage.Controllers;

public class AccountLogonController(SignInManager<FoodUser> signInManager, UserManager<FoodUser> userManager) : Controller
{
    [HttpGet]
    public IActionResult LogOn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LogOn(LogOnModel model)
    {
        FoodUser user = await userManager.FindByNameAsync(model.Username);
        if (user != null)
        {
            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (result.Succeeded)
            {
                bool isAdmin = await userManager.IsInRoleAsync(user, "Admin");
                if (isAdmin)
                    return Json(new LogOnResponse(StatusCodes.Status200OK, "登录成功"));
                return Json(new LogOnResponse(StatusCodes.Status403Forbidden, "访问被拒绝，仅限管理员."));
            }
        }
        return Json(new LogOnResponse(StatusCodes.Status401Unauthorized, "登录失败，用户名或密码无效."));
        
    }
}
