namespace AI.DeliciousFood.Web.Manage;

public class GlobalConfig(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
{
    public string ClientHost => configuration["ClientHost"] ?? throw new ArgumentNullException(nameof(ClientHost));
    public int HotListRecommendCount => Convert.ToInt16(configuration["HotListRecommendCount"]);
    public int MonthlyRecommendCount => Convert.ToInt16(configuration["MonthlyRecommendCount"]);

}
