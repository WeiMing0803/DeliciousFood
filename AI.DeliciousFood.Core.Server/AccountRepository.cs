using AI.DeliciousFood.Core.Common.ClientHelper;
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
    Task<EditRecipeViewModel> GetRecipeForEditAsync(Guid recipeGuid, CancellationToken cancellationToken = default);
    bool IsUserNameTaken(string userName, long userId);
    bool IsEmailTaken(string email, long userId);
    bool IsPhoneNumberTaken(string phoneNumber, long userId);
    bool IsValidEmail(string email);
    Task SaveUserInfoAsync(SaveUserInfoModel userInfo, CancellationToken cancellationToken = default);
    string SendResetPasswordEmail(FoodUser user);
}

public class AccountRepository(GenericRepository<FoodDbContext> dbContextBase, FoodDbContext dbContext, UserManager<FoodUser> userManager, IEmailRepository emailRepository) : IAccountRepository
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
            .Where(x => x.UserId == userId)
            .Include(x => x.RecipeStatus)
            .Select(x => new
            {
                x.Guid,
                x.RecipeName,
                x.RecipeDescription,
                FileName = x.FileNames.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault(),
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
            Flavors = dbContext.BaseCategoryItem
                .Where(c => c.Guid == u.recipese.Flavors)
                .Select(c => c.Name)
                .FirstOrDefault(),
            CookingCraft = dbContext.BaseCategoryItem.
                Where(c => c.Guid == u.recipese.CookingCraft)
                .Select(c => c.Name)
                .FirstOrDefault(),
            UseKitchenUtensils = u.recipese.UseKitchenUtensils,
            Practice = u.recipese.Practice,
            Tips = u.recipese.Tips,
            CreateTime = u.recipese.CreateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            UpdateTime = u.recipese.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            FileNames = u.recipese.FileNames,
            ImageUrl = u.recipese.ImageUrl,
            Ingredients = JsonConvert.DeserializeObject<List<Ingredients>>(u.recipese.Ingredients)!,
            RecipeStatus = u.recipeStatus.Status.GetDescription(),
        })
        .FirstAsync();

        // 如果没有找到结果，抛出自定义异常，避免返回空值
        if (recipe == null)
        {
            throw new KeyNotFoundException($"Recipe with Guid {recipeGuid} was not found.");
        }

        return recipe;
    }

    public async Task<EditRecipeViewModel> GetRecipeForEditAsync(Guid recipeGuid, CancellationToken cancellationToken = default)
    {
        EditRecipeViewModel recipe = await dbContext.Recipes
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
        .Select(u => new EditRecipeViewModel
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
            RecipeStatus = u.recipeStatus.Status.GetDescription(),
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

    public string SendResetPasswordEmail(FoodUser user)
    {
        string password = UserHelper.GeneratedPassword();
        string emailBody = $@"
        <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <h2>亲爱的用户，</h2>
                <p>您正在重置 <strong>美食每客</strong> 的账户密码。</p>
                <p>您的新密码为：</p>
                <div style='padding: 10px; background-color: #f2f2f2; border-radius: 5px; display: inline-block;'>
                    <strong>{password}</strong>
                </div>
                <p style='margin-top: 20px;'>请使用此密码登录，并在登录后及时修改密码以保障账户安全。</p>
                <p>如果您未申请重置密码，请忽略此邮件。</p>
                <p style='margin-top: 30px;'>—— 美食每客团队</p>
            </body>
        </html>";

        emailRepository.SendEmail(user.Email!, "重置您的美食每客网站密码", emailBody);
        return password;
    }
}
