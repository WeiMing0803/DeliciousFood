namespace AI.DeliciousFood.Core.Model;

/// <summary>
/// 基本类型表
/// </summary>
public class BaseCategory
{
    /// <summary>
    /// 主键
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 类型名称
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    /// 导航属性 (一个基本类型可以有多个基本类型项目)
    /// </summary>
    public ICollection<BaseCategoryItem> BaseCategoryItems { get; set; }
}