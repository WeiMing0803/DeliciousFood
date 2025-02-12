using AI.DeliciousFood.Core.Common.Model.Account;
using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AI.DeliciousFood.Core.Server;

public interface IAccountRepository
{
    Task<UserInfoModel> GetUserAsync(long userId, CancellationToken cancellationToken = default);
    bool IsUserNameTaken(string userName, long userId);
    bool IsEmailTaken(string email, long userId);
    bool IsPhoneNumberTaken(string phoneNumber, long userId);
    bool IsValidEmail(string email);
    Task SaveUserInfoAsync(SaveUserInfoModel userInfo, CancellationToken cancellationToken = default);
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

    public bool IsEmailTaken(string email, long userId)
    {
        return dbContext.FoodUsers.Any(x => x.Email == email && x.Id != userId);
    }

    public bool IsPhoneNumberTaken(string phoneNumber, long userId)
    {
        return dbContext.FoodUsers.Any(x => x.PhoneNumber == phoneNumber && x.Id != userId);
    }

    public bool IsUserNameTaken(string userName, long userId)
    {
        return dbContext.FoodUsers.Any(x => x.UserName == userName && x.Id != userId);
    }

    public bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // 使用正则表达式验证Email格式
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
            return emailRegex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }

    public async Task SaveUserInfoAsync(SaveUserInfoModel userInfo, CancellationToken cancellationToken = default)
    {
        await dbContext.FoodUsers
           .Where(x => x.Id == userInfo.Id)
           .ExecuteUpdateAsync(setters => setters
               .SetProperty(u => u.UserName, userInfo.UserName)
               .SetProperty(u => u.NormalizedUserName, userInfo.UserName)
               .SetProperty(u => u.Email, userInfo.Email)
               .SetProperty(u => u.NormalizedEmail, userInfo.Email)
               .SetProperty(u => u.PhoneNumber, userInfo.PhoneNumber), cancellationToken);
    }
}
