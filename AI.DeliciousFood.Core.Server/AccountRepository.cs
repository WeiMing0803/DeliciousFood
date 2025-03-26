using AI.DeliciousFood.Core.Common.Extensions;
using AI.DeliciousFood.Core.Common.Model;
using AI.DeliciousFood.Core.Common.Model.Account;
using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace AI.DeliciousFood.Core.Server;

public interface IAccountRepository
{
    Task<UserInfoModel> GetUserAsync(long userId, CancellationToken cancellationToken = default);
    Task<GetUserInfoModel> GetUserInfoAsync(long userId, CancellationToken cancellationToken = default);
    Task<RecipeModel> GetRecipeAsync(Guid recipeGuid, CancellationToken cancellationToken = default);
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

    public async Task<GetUserInfoModel> GetUserInfoAsync(long userId, CancellationToken cancellationToken = default)
    {
        var recipes = await dbContext.Recipes
            .Include(x => x.RecipeStatus)
            .Select(x => new
            {
                x.Guid,
                x.RecipeName,
                x.RecipeDescription,
                FileName = x.FileNames.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).First(),
                x.ImageUrl,
                Status = x.RecipeStatus.Status
            })
            .ToListAsync(cancellationToken);

        Dictionary<StatusEnum, List<MenuSummary>> groupedRecipes = recipes
            .GroupBy(x => x.Status)
            .ToDictionary(g => g.Key, g => g.Select(r => new MenuSummary
            {
                Guid = r.Guid,
                RecipeName = r.RecipeName,
                RecipeDescription = r.RecipeDescription,
                FileName = r.FileName,
                ImageUrl = r.ImageUrl
            }).ToList());

        return new GetUserInfoModel
        {
            UserInfoModel = await GetUserAsync(userId, cancellationToken),
            Approved = groupedRecipes.GetValueOrDefault(StatusEnum.Approved, new List<MenuSummary>()),
            UnderReviewOrNotApproved = groupedRecipes
                .Where(kv => kv.Key is StatusEnum.UnderReview or StatusEnum.NotApproved)
                .SelectMany(kv => kv.Value)
                .ToList(),
            Draft = groupedRecipes.GetValueOrDefault(StatusEnum.Draft, new List<MenuSummary>())
        };
    }


    public async Task<RecipeModel> GetRecipeAsync(Guid recipeGuid, CancellationToken cancellationToken = default)
    {
        RecipeModel recipe = await dbContext.Recipes
        .AsNoTracking()
        .Where(u => u.Guid == recipeGuid)
        .Join(dbContext.FoodUsers,
              recipese => recipese.UserId,
              user => user.Id,
              (recipese, user) => new { recipese, user })
        .Join(dbContext.RecipeStatus,
              joined => joined.recipese.Guid,
              recipeStatus => recipeStatus.RecipeGuid,
              (joined, recipeStatus) => new { joined.recipese, joined.user, recipeStatus })
        .Select(u => new RecipeModel
        {
            RecipeGuid = u.recipese.Guid,
            UserName = u.user.UserName!,
            RecipeName = u.recipese.RecipeName,
            RecipeDescription = u.recipese.RecipeDescription,
            RoductionDifficulty = u.recipese.RoductionDifficulty,
            TasksTime = u.recipese.TasksTime,
            Flavors = u.recipese.Flavors,
            CookingCraft = u.recipese.CookingCraft,
            UseKitchenUtensils = u.recipese.UseKitchenUtensils,
            Practice = u.recipese.Practice,
            Tips = u.recipese.Tips,
            CreateTime = u.recipese.CreateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            UpdateTime = u.recipese.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            FileNames = u.recipese.FileNames,
            ImageUrl = u.recipese.ImageUrl,
            Ingredients = JsonConvert.DeserializeObject<List<Ingredients>>(u.recipese.Ingredients)!,
            RecipeStatus = u.recipeStatus.Status.ToString() == StatusEnum.Approved.ToString() ? "已发布" : u.recipeStatus.Status.GetDescription(),
        })
        .FirstAsync();

        // 如果没有找到结果，抛出自定义异常，避免返回空值
        if (recipe == null)
        {
            throw new KeyNotFoundException($"Recipe with Guid {recipeGuid} was not found.");
        }

        return recipe;
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
