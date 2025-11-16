using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.ManageServer;
using AI.DeliciousFood.Core.Model;
using AI.DeliciousFood.Web.Manage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<FoodDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration["SqlConnectionStrings"]);
});
builder.Services.AddIdentityCore<FoodUser>(options =>
{
    //options.Lockout.MaxFailedAccessAttempts = 10; //密码错误失败次数
    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);    //密码错误锁定时间
    options.User.AllowedUserNameCharacters = null; //跳过对用户名的验证
    options.Password.RequireDigit = false; //数字
    options.Password.RequiredLength = 6; //长度
    options.Password.RequireLowercase = false; //小写
    options.Password.RequireNonAlphanumeric = false; //特殊字符
    options.Password.RequireUppercase = false; //大写
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;  //用于生成密码重置电子邮件中使用的令牌（现在是生成数字）；如果是把重置链接发到用户，那么就不用配置
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;  //获取或设置令牌提供程序，用于生成在帐户确认电子邮件中使用的令牌；上面生成的验证码太长、太复杂。如果是用户输入的验证码，则要配置
});
IdentityBuilder identityBuilder = new IdentityBuilder(typeof(FoodUser), typeof(FoodRole), builder.Services);
identityBuilder.AddEntityFrameworkStores<FoodDbContext>()
    .AddDefaultTokenProviders()
    .AddUserManager<UserManager<FoodUser>>()
    .AddRoleManager<RoleManager<FoodRole>>()
    .AddSignInManager<SignInManager<FoodUser>>();

//添加cookie认证signInManager.PasswordSignInAsync方法需要使用到
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
})
.AddCookie(IdentityConstants.ApplicationScheme)
.AddCookie(IdentityConstants.ExternalScheme)
.AddCookie(IdentityConstants.TwoFactorUserIdScheme);

builder.Services.AddSingleton<GlobalConfig>();
builder.Services.AddScoped(typeof(GenericRepository<>));
builder.Services.AddScoped<IUserManagementRepository, UserManagementRepository>();
builder.Services.AddScoped<IRecipeManagementRepository, RecipeManagementRepository>();
builder.Services.AddScoped<IFrontPageManagementRepository, FrontPageManagementRepository>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/AccountLogon/Logon"; // 设置未登录时跳转的路径
});

//应用启动或运行时检查依赖注册是否正确
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true; // 在开发环境下默认开启
    options.ValidateOnBuild = true; // 启动时立即验证
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=DefaultIndex}/{id?}");

// 初始化数据，注册Admin用户和角色
//await InitializeDB.InitializeDatabase(app.Services);

app.Run();
