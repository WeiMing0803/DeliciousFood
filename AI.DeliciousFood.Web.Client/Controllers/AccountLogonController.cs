using AI.DeliciousFood.Core.Model;
using AI.DeliciousFood.Core.Common.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AI.DeliciousFood.Core.Common.Model.Logon;

namespace AI.DeliciousFood.Web.Client.Controllers
{
    public class AccountLogonController(UserManager<FoodUser> _userManager, SignInManager<FoodUser> _signInManager) : CommonControllerBase
    {

        #region 登录和注册

        [HttpGet]
        public IActionResult LogOn(string returnUrl)
        {
            LogOnPageViewModel model = new LogOnPageViewModel
            {
                LoginModel = new LoginViewModel(),
                RegisterModel = new RegisterViewModel(),
                ForgotModel = new ForgotPasswordViewModel()
            };
            ViewBag.ActiveForm = "login";
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOn([Bind(Prefix = "LoginModel")] LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                bool returnLogon = false;
                FoodUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "邮箱或者密码错误");
                    returnLogon = true;
                }

                if (!returnLogon)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.ToString());
                    }
                }
            }
            ViewBag.ActiveForm = "login";
            return View("LogOn", new LogOnPageViewModel { LoginModel = model });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
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
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        IdentityResult roleAssignmentResult = await _userManager.AddToRoleAsync(user, Roles.USER.ToString());
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
            ViewBag.ActiveForm = "register";
            return View("LogOn", new LogOnPageViewModel { RegisterModel = model });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 忘记密码逻辑
                return null;
            }
            else
            {
                ViewBag.ActiveForm = "forgot";
                return View("LogonPage", new LogOnPageViewModel { ForgotModel = model });
            }
            // ...
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
