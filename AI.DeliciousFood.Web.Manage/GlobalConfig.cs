namespace AI.DeliciousFood.Web.Manage;

public class GlobalConfig(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
{
    public string ClientHost => configuration["ClientHost"] ?? throw new ArgumentNullException(nameof(ClientHost));

}
