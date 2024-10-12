using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using AI.DeliciousFood.Core.ManageServer;
using AI.DeliciousFood.Core.Model;
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
        IEnumerable<FoodUser> userList = await userManagement.GetUserListAsync();
        List<UserManagerModel> result = userList
            .OrderBy(x => x.Id) // 确保有序，以支持分页
            .Select(x => new UserManagerModel
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                MembershipExpireAt = x.MembershipExpireAt,
                Status = "1"
            })
            .ToList();

        // 获取总数
        int total = result.Count;

        // 分页
        var pagedResult = result
            .Skip(offset)
            .Take(limit)
            .ToList();

        // 返回包含 total 和 rows 的对象
        return Json(new { total, rows = pagedResult });
    }

}
