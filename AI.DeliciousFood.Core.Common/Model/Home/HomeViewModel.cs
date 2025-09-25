namespace AI.DeliciousFood.Core.Common.Model.Home;

public class RecommendCollectionViewModel
{
    public List<RecommendViewModel> Monthly { get; set; }
    public List<RecommendViewModel> HotList { get; set; }
}

public class RecommendViewModel
{
    public Guid Guid { get; set; }

    public Guid RecipeGuid { get; set; }

    public RecommendType Type { get; set; }

    public string UserName { get; set; }

    public string RecipeName { get; set; }
    public string RecipeDescription { get; set; }

    public string FileName { get; set; }

    public string ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }
}
