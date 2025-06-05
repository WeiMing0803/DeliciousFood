using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using AI.DeliciousFood.Core.ManageServer;
using AI.DeliciousFood.Core.Model;
using AI.DeliciousFood.Web.Manage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Manage.Controllers;

[Authorize]
public class UserManagementController(IUserManagementRepository userManagement) : Controller
{
    public async Task<IActionResult> UserList()
    {
        IEnumerable<FoodRole> result = await userManagement.GetRoleListAsync();
        ViewBag.Roles = result.ToList();
        return View();
    }

    public async Task<IActionResult> GetUserList(string username, string roletype, int limit, int offset)
    {

        List<UserManagerModel> userList = await userManagement.GetUserListAsync(username, roletype, offset, limit);

        // 获取总数
        int total = await userManagement.GetUserListCountAsync(username, roletype);

        // 返回包含 total 和 rows 的对象
        return Json(new PagedResult<UserManagerModel>{ Total = total, Rows = userList });
    }

    [HttpPost]
    public async Task<IActionResult> SaveUser(SaveUserModel model)
    {
        await userManagement.SaveUser(model);
        return Ok(new { success = true });
    }


    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        IEnumerable<FoodRole> result = await userManagement.GetRoleListAsync();
        return Ok(new
        {
            success = true,
            data = result
        });
    }
}
