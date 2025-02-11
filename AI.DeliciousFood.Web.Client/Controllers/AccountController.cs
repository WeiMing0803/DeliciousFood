using AI.DeliciousFood.Core.Common.Model.Account;
using AI.DeliciousFood.Core.Server;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Client.Controllers;

public partial class AccountController(IAlipayRepository alipayRepository, IAccountRepository accountRepository) : CommonControllerBase
{
    public async Task<IActionResult> GetUserInfo()
    {
        UserInfoModel user = await accountRepository.GetUserAsync(UserInfo.UserId);
        return View(user);
    }

}
