using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using AI.DeliciousFood.Core.ManageServer;
using AI.DeliciousFood.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Manage.Controllers;

public class UserManagement(IUserManagementRepository userManagement) : Controller
{
    public IActionResult UserList()
    {
        return View();
    }

    public async Task<IActionResult> GetUserList(int limit, int offset)
    {
        List<UserManagerModel> userList = await userManagement.GetUserListAsync();

        // 获取总数
        int total = userList.Count;

        // 分页
        var pagedResult = userList
            .Skip(offset)
            .Take(limit)
            .ToList();

        // 返回包含 total 和 rows 的对象
        return Json(new { total, rows = pagedResult });
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
        IEnumerable<FoodRole> result = await userManagement.GetRolestAsync();
        return Ok(new
        {
            success = true,
            data = result
        });
    }
}
