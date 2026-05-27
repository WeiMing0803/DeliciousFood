namespace AI.DeliciousFood.Web.Client
{
    public class GlobalConfig(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        public string[] KitchenToolsList => File.ReadAllLines(Path.Combine(webHostEnvironment.WebRootPath, "constantFile", "KitchenToolsList.txt"));

    }
}
