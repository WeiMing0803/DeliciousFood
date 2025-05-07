namespace AI.DeliciousFood.Core.Common.ManageModel.UserManager;

public class RecipeManagementModel
{
    public Guid RecipeGuid { get; set; }
    public required string RecipeName { get; set; }
    public required string UserName { get; set; }
    public required string CreateTime { get; set; }
    public required string UpdateTime { get; set; }
    public required string RecipeStatus { get; set; }
}

public class RecipeModel
{
    public Guid RecipeGuid { get; set; }
    public required string UserName { get; set; }
    public required string RecipeName { get; set; }
    public required string RecipeDescription { get; set; }
    public required string RoductionDifficulty { get; set; }
    public required string TasksTime { get; set; }
    public string? Flavors { get; set; }
    public string? CookingCraft { get; set; }
    public required string UseKitchenUtensils { get; set; }
    public required string Practice { get; set; }
    public required string Tips { get; set; }
    public required string CreateTime { get; set; }
    public required string UpdateTime { get; set; }
    public required string FileNames { get; set; }
    public required string ImageUrl { get; set; }
    public required List<Ingredients> Ingredients { get; set; }
    public required string RecipeStatus { get; set; }
}


public class Ingredients
{
    public required string Name { get; set; }
    public required string Quantity { get; set; }
}

public class SaveRecipeCommentModel
{
    public Guid RecipeGuid { get; set; }
    public string? Comment { get; set; }
    public bool Approval { get; set; }
}