namespace AI.DeliciousFood.Core.Common.ManageModel.FrontPageManagement;

public class FrontPageManagementModel
{
    public Guid Guid { get; set; }
    public required Guid RecipeGuid { get; set; }
    public required string RecipeName { get; set; }
    public required string UserName { get; set; }
    public required string StartTime { get; set; }
    public string? EndTime { get; set; }
    public required string Type { get; set; }
    public required bool IsActive { get; set; }
}
