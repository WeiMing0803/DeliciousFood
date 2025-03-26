using AI.DeliciousFood.Core.Common.Model.Account;
using AI.DeliciousFood.Core.Server;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Client.Controllers;

public partial class AccountController(IAlipayRepository alipayRepository, IAccountRepository accountRepository) : CommonControllerBase
{
    public async Task<IActionResult> GetUserInfo()
    {
        GetUserInfoModel user = await accountRepository.GetUserInfoAsync(UserInfo.UserId);

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> SaveUserInfo([FromBody] SaveUserInfoModel userInfo)
    {
        if (userInfo == null ||
            string.IsNullOrWhiteSpace(userInfo.UserName) ||
            string.IsNullOrWhiteSpace(userInfo.Email) ||
            string.IsNullOrWhiteSpace(userInfo.PhoneNumber))
        {
            return Ok(new { success = false, errors = new { general = "请求数据不能为空" } });
        }

        if (!accountRepository.IsValidEmail(userInfo.Email))
        {
            return Ok(new { success = false, errors = new { email = "电子邮件格式无效" } });
        }

        // 检查是否有重复数据
        var duplicateErrors = new Dictionary<string, string>();

        if (accountRepository.IsUserNameTaken(userInfo.UserName, userInfo.Id))
        {
            duplicateErrors["userName"] = "当前名称已被使用";
        }

        if (accountRepository.IsEmailTaken(userInfo.Email, userInfo.Id))
        {
            duplicateErrors["email"] = "当前电子邮件已被使用";
        }

        if (accountRepository.IsPhoneNumberTaken(userInfo.PhoneNumber, userInfo.Id))
        {
            duplicateErrors["phoneNumber"] = "当前电话号码已被使用";
        }

        // 如果有重复数据，返回错误信息
        if (duplicateErrors.Any())
        {
            return Ok(new { success = false, errors = duplicateErrors });
        }

        await accountRepository.SaveUserInfoAsync(userInfo);
        return Ok(new { success = true });
    }


    [HttpGet]
    public async Task<IActionResult> GetRecipe(Guid recipeGuid)
    {
        RecipeModel recipe = await accountRepository.GetRecipeAsync(recipeGuid);
        return View(recipe);
    }
}
