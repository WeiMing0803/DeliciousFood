using AI.DeliciousFood.Core.Common.Model;

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
