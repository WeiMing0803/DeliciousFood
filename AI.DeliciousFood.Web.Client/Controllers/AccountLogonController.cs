using AI.DeliciousFood.Core.Model;
using AI.DeliciousFood.Core.Model.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Client.Controllers
{
    public class AccountLogonController(UserManager<FoodUser> _userManager, SignInManager<FoodUser> _signInManager) : CommonControllerBase
    {

        #region 登录和注册

        [HttpGet]
        public IActionResult LogOn(string returnUrl)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LogOn(LogOnViewModel model, string returnUrl)
        {
            bool returnLogon = false;
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError(string.Empty, "邮箱或密码不能为空");
                returnLogon = true;
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "邮箱或者密码错误");
                returnLogon = true;
            }

            if (!returnLogon)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.ToString());
                }
            }
            return View("LogOn", model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(LogOnViewModel model)
        {
            if (ModelState.IsValid)
            {
                var emailExists = await _userManager.FindByEmailAsync(model.Email) != null;
                var usernameExists = await _userManager.FindByEmailAsync(model.UserName) != null;
                if (!emailExists && !usernameExists)
                {
                    var user = new FoodUser
                    {
                        Email = model.Email,
                        UserName = model.UserName,
                        PhoneNumber = model.PhoneNumber
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    if (emailExists)
                    {
                        ModelState.AddModelError("Email", "邮箱已存在");
                    }

                    if (usernameExists)
                    {
                        ModelState.AddModelError("UserName", "用户名已存在");
                    }
                }
            }
            return View("LogOn", model);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
