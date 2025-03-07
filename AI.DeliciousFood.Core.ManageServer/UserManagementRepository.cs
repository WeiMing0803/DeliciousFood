using AI.DeliciousFood.Core.Common.ManageModel.UserManager;
using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AI.DeliciousFood.Core.ManageServer;

public interface IUserManagementRepository
{
    Task<List<UserManagerModel>> GetUserListAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<FoodRole>> GetRolestAsync(CancellationToken cancellationToken = default);
    Task SaveUser(SaveUserModel model, CancellationToken cancellationToken = default);

}

public class UserManagementRepository(GenericRepository<FoodDbContext> dbContextBase, FoodDbContext dbContext, RoleManager<FoodRole> roleManager) : IUserManagementRepository
{

    public static IEnumerable<FoodRole> Roles = null;
    public async Task<List<UserManagerModel>> GetUserListAsync(CancellationToken cancellationToken = default)
    {
        List<UserManagerModel> users = await dbContext.Users
            .Join(dbContext.UserRoles,
                  user => user.Id,
                  userRole => userRole.UserId,
                  (user, userRole) => new { user, userRole })
            .OrderBy(u => u.user.Id)  // 确保有序，以支持分页
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
        return users;
    }

    public async Task<IEnumerable<FoodRole>> GetRolestAsync(CancellationToken cancellationToken = default)
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
