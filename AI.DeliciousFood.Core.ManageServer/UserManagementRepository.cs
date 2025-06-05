using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AI.DeliciousFood.Core.ManageServer;

public interface IUserManagementRepository
{
    Task<List<UserManagerModel>> GetUserListAsync(string username, string roleType,int offset, int limit, CancellationToken cancellationToken = default);
    Task<int> GetUserListCountAsync(string username, string roleType, CancellationToken cancellationToken = default);
    Task<IEnumerable<FoodRole>> GetRoleListAsync(CancellationToken cancellationToken = default);
    Task SaveUser(SaveUserModel model, CancellationToken cancellationToken = default);

}

public class UserManagementRepository(FoodDbContext dbContext, RoleManager<FoodRole> roleManager) : IUserManagementRepository
{

    public static IEnumerable<FoodRole> Roles = null;

    public async Task<List<UserManagerModel>> GetUserListAsync(string username, string roleType, int offset, int limit, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Users
            .Join(dbContext.UserRoles,
                  user => user.Id,
                  userRole => userRole.UserId,
                  (user, userRole) => new { user, userRole })
            .Where(u =>
                (string.IsNullOrWhiteSpace(username) || u.user.UserName!.Contains(username)) &&
                (string.IsNullOrWhiteSpace(roleType) || u.userRole.RoleId.ToString() == roleType))
            .OrderBy(u => u.user.Id);  // 分页前需要排序

        List<UserManagerModel> pagedList = await query
            .Skip(offset)
            .Take(limit)
            .Select(u => new UserManagerModel
            {
                Id = u.user.Id,
                UserName = u.user.UserName ?? string.Empty,
                Email = u.user.Email ?? string.Empty,
                PhoneNumber = u.user.PhoneNumber ?? string.Empty,
                MembershipExpireAt = u.user.MembershipExpireAt,
                CreateDateTime = u.user.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                RoleId = u.userRole.RoleId.ToString(),
                Status = "1"
            })
            .ToListAsync(cancellationToken);

        return pagedList;
    }

    public async Task<int> GetUserListCountAsync(string username, string roleType, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Users
            .Join(dbContext.UserRoles,
                  user => user.Id,
                  userRole => userRole.UserId,
                  (user, userRole) => new { user, userRole })
            .Where(u =>
                (string.IsNullOrWhiteSpace(username) || u.user.UserName!.Contains(username)) &&
                (string.IsNullOrWhiteSpace(roleType) || u.userRole.RoleId.ToString() == roleType));

        return await query.CountAsync(cancellationToken);
    }


    public async Task<IEnumerable<FoodRole>> GetRoleListAsync(CancellationToken cancellationToken = default)
    {
        if (Roles == null)
            Roles = await roleManager.Roles.ToListAsync(cancellationToken);

        return Roles;
    }

    public async Task SaveUser(SaveUserModel model, CancellationToken cancellationToken = default)
    {
        if (model.MembershipExpireAt != DateOnly.MinValue)
            await dbContext.FoodUsers
               .Where(x => x.Id == model.Id)
               .ExecuteUpdateAsync(setters => setters
                   .SetProperty(u => u.MembershipExpireAt, model.MembershipExpireAt), cancellationToken);

        if (!string.IsNullOrEmpty(model.RoleId))
            await dbContext.UserRoles
                .Where(x => x.UserId == model.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.RoleId, long.Parse(model.RoleId)), cancellationToken);
    }
}
