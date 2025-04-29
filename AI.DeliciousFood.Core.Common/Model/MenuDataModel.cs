using Newtonsoft.Json;

namespace AI.DeliciousFood.Web.Client.Models;

public class MenuDataModel
{
    public Guid? RecipeGuid { get; set; }
    public bool IsDraft { get; set; }
    public required string RecipeName { get; set; }
    public string? Description { get; set; }
    public string? ProductionDifficulty { get; set; }
    public string? NeedsTime { get; set; }
    public Guid? Taste { get; set; }
    public Guid? CookingCraft { get; set; }
    public string[] KitchenUtensils { get; set; }
    public string? Tips { get; set; }
    public string? Steps { get; set; }
    public List<IngredientDetail> IngredientsDetails { get; set; }
    public List<FileUpload> Files { get; set; }
    public string[] DeletedFiles { get; set; }
}

public class IngredientDetail
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("quantity")]
    public string Quantity { get; set; }
}

public class FileUpload
{
    [JsonProperty("fileName")]
    public string FileName { get; set; }

    [JsonProperty("fileBytes")]

    public byte[] FileBytes { get; set; }

    [JsonProperty("previewId")]

    public string PreviewId { get; set; }
}
