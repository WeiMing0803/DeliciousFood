using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.DeliciousFood.Core.Model
{
    /// <summary>
    /// 食材表
    /// </summary>
    public class Material
    {
        public Guid Guid { get; set; }
        
        /// <summary>
        /// 关联菜谱表
        /// </summary>
        public Guid RecipeGuid { get; set; }

        /// <summary>
        /// 食材名称
        /// </summary>
        public string IngredientName { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public string Usage { get; set; }

        public Recipe Recipe { get; set; }
    }
}
