namespace AI.DeliciousFood.Core.Model;

/// <summary>
/// 基本类型项目表
/// </summary>
public class BaseCategoryItem
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Guid { get; set; }

    /// <summary>
    /// 关联基本类型表
    /// </summary>
    public int BaseCategoryId { get; set; }

    /// <summary>
    /// 类型名称
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    /// 导航属性 (一个基本类型项目只能属于一个基本类型)
    /// </summary>
    public BaseCategory BaseCategory { get; set; }
}