using AI.DeliciousFood.Core.Common.ManageModel;
using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using AI.DeliciousFood.Core.ManageServer;
using AI.DeliciousFood.Core.Model;
using Microsoft.AspNetCore.Mvc;

namespace AI.DeliciousFood.Web.Manage.Controllers;

public class UserManagement(IUserManagementRepository userManagement) : Controller
{
    public async Task<IActionResult> UserList(int page = 1, int itemsPerPage = 10)
    {
        // 确保页码和每页条目数有效
        page = Math.Max(page, 1);
        itemsPerPage = Math.Max(itemsPerPage, 1);

        // 获取分页后的用户列表
        IEnumerable<FoodUser> userList = await userManagement.GetUserListAsync();

        // 获取用户总数
        int totalUserCount = userList.Count();

        // 计算总页数
        int totalPages = (int)Math.Ceiling((double)totalUserCount / itemsPerPage);

        // 确保当前页码不超过总页数
        page = Math.Min(page, totalPages);

        // 进行分页处理和投影
        List<UserManagerModel> result = userList
            .OrderBy(x => x.Id) // 确保有序，以支持分页
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .Select(x => new UserManagerModel
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                MembershipExpireAt = x.MembershipExpireAt
            })
            .ToList();

        // 创建分页视图模型
        UserListViewModel viewModel = new UserListViewModel
        {
            Users = result,
            Pagination = new PaginationModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = itemsPerPage,
                TotalCount = totalUserCount
            }
        };

        return View(viewModel);
    }
}
