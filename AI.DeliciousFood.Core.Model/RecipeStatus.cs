using AI.DeliciousFood.Core.Common.Model;

namespace AI.DeliciousFood.Core.Model;

/// <summary>
/// 菜谱状态表
/// </summary>
public class RecipeStatus
{
    public Guid Guid { get; set; }

    /// <summary>
    /// 菜谱Guid
    /// </summary>
    public Guid RecipeGuid { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public StatusEnum Status { get; set; }

    /// <summary>
    /// 审批人
    /// </summary>
    public long Approver { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 导航属性 (一个菜谱状态只能有一个审批人)
    /// </summary>
    public FoodUser ApproverUser { get; set; }

    /// <summary>
    /// 导航属性 (一个菜谱状态只能有一个菜谱)
    /// </summary>
    public Recipe Recipe { get; set; }
}
