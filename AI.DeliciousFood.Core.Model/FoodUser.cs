using Microsoft.AspNetCore.Identity;

namespace AI.DeliciousFood.Core.Model;

public class FoodUser : IdentityUser<long>
{
    public DateOnly MembershipExpireAt { get; set; }
    public DateTime CreateDateTime { get; set; }


    /// <summary>
    /// 导航属性 (一个用户可以有多个菜谱)
    /// </summary>
    public ICollection<Recipe> Recipes { get; set; }

    /// <summary>
    ///  导航属性 (一个用户可以批准多个菜谱状态)
    /// </summary>
    public ICollection<RecipeStatus> ApprovedRecipeStatuses { get; set; }
}
