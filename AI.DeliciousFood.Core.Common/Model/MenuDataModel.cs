using Newtonsoft.Json;

namespace AI.DeliciousFood.Web.Client.Models
{
    public class MenuDataModel
    {
        public bool IsDraft { get; set; }
        public string RecipeName { get; set; }
        public string Description { get; set; }
        public string ProductionDifficulty { get; set; }
        public string NeedsTime { get; set; }
        public string Taste { get; set; }
        public string CookingCraft { get; set; }
        public string[] KitchenUtensils { get; set; }
        public string Tips { get; set; }
        public string Steps { get; set; }
        public ICollection<IngredientDetail> IngredientsDetails { get; set; }
        public ICollection<FileUpload> Files { get; set; }
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
}
