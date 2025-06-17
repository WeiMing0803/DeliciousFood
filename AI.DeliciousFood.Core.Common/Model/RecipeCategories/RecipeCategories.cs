using AI.DeliciousFood.Core.Common.Common;
using AI.DeliciousFood.Core.Model;

namespace AI.DeliciousFood.Core.Common.Model.RecipeCategories;

public record RecipeCategory(string Guid, string Name, string Category);

public class RecipeCategoriesIndexViewModel
{
    public List<RecipeCategory> Categories { get; set; }
    public RecipeListViewModel RecipeList { get; set; }
}

public class RecipeListViewModel
{
    public List<RecipeModel> Recipes { get; set; }
    public PaginationModel pagination { get; set; }
}

public class RecipeModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
}
