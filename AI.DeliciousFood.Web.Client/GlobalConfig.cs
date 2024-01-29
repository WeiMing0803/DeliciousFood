namespace AI.DeliciousFood.Web.Client
{
    public class GlobalConfig(IWebHostEnvironment webHostEnvironment)
    {
        public string[] CookingTechniquesList => File.ReadAllLines(Path.Combine(webHostEnvironment.WebRootPath, "constantFile", "CookingTechniquesList.txt"));
        public string[] FlavorsList => File.ReadAllLines(Path.Combine(webHostEnvironment.WebRootPath, "constantFile", "FlavorsList.txt"));
        public string[] KitchenToolsList => File.ReadAllLines(Path.Combine(webHostEnvironment.WebRootPath, "constantFile", "KitchenToolsList.txt"));
    }
}
