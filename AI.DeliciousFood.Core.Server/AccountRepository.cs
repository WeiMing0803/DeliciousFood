using AI.DeliciousFood.Core.Common.Model.Account;
using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AI.DeliciousFood.Core.Server;

public interface IAccountRepository
{
    Task<UserInfoModel> GetUserAsync(long userId, CancellationToken cancellationToken = default);
}

public class AccountRepository(GenericRepository<FoodDbContext> dbContextBase, FoodDbContext dbContext, UserManager<FoodUser> userManager) : IAccountRepository
{
    public async Task<UserInfoModel> GetUserAsync(long userId, CancellationToken cancellationToken = default)
    {
        UserInfoModel userInfo = await dbContext.Users
                   .Where(u => u.Id == userId)
                   .Join(dbContext.UserRoles,
                         user => user.Id,
                         userRole => userRole.UserId,
                         (user, userRole) => new UserInfoModel
                         {
                             Id = user.Id,
                             UserName = user.UserName,
                             Email = user.Email,
                             PhoneNumber = user.PhoneNumber,
                             MembershipExpireAt = user.MembershipExpireAt.ToString("yyyy-MM-dd"),
                             CreateDateTime = user.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                             RoleId = userRole.RoleId.ToString()
                         })
                    .FirstOrDefaultAsync(cancellationToken);
        return userInfo;
    }
}
