using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AI.DeliciousFood.Core.Model
{
    /// <summary>
    /// 菜谱表
    /// </summary>
    public class Recipe
    {
        public Guid Guid { get; set; }

        /// <summary>
        /// 关联用户表
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 菜谱名
        /// </summary>
        public string RecipeName { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string FileNames { get; set; }

        /// <summary>
        /// 菜谱描述
        /// </summary>
        public string RecipeDescription { get; set; }

        /// <summary>
        /// 制作难度
        /// </summary>
        public string RoductionDifficulty { get; set; }

        /// <summary>
        /// 需要时间
        /// </summary>
        public string TasksTime { get; set; }

        /// <summary>
        /// 口味
        /// </summary>
        public string Flavors { get; set; }

        /// <summary>
        /// 烹饪工艺
        /// </summary>
        public string CookingCraft { get; set; }

        /// <summary>
        /// 使用厨具
        /// </summary>
        public string UseKitchenUtensils { get; set; }

        /// <summary>
        /// 食材
        /// </summary>
        public string Ingredients { get; set; } 

        /// <summary>
        /// 做法
        /// </summary>
        public string Practice { get; set; }

        /// <summary>
        /// 小窍门
        /// </summary>
        public string Tips { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 是否批准
        /// </summary>
        public bool IsApproval { get; set; }

        //public ICollection<Material> Materials { get; set; }

        public FoodUser User { get; set; }
    }
}
