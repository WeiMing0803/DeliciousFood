using AI.DeliciousFood.Core.Common.Model;

namespace AI.DeliciousFood.Core.Model;

/// <summary>
/// 首页推荐表
/// </summary>
public class Recommend
{
    public Guid Guid { get; set; }

    /// <summary>
    /// 菜谱GUID
    /// </summary>
    public Guid RecipeGuid { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public RecommendType Type { get; set; }

    /// <summary>
    /// 是否推荐
    /// </summary>
    public bool IsActive { get; set; }

    public Recipe Recipe { get; set; } = null!;
}
