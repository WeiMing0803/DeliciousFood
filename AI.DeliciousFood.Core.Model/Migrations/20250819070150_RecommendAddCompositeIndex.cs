using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.DeliciousFood.Core.Model.Migrations
{
    /// <inheritdoc />
    public partial class RecommendAddCompositeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 删除原有的单列唯一索引
            migrationBuilder.DropIndex(
                name: "IX_T_Recommend_RecipeGuid",
                table: "T_Recommend");

            // 创建新的复合唯一索引
            migrationBuilder.CreateIndex(
                name: "IX_T_Recommend_RecipeGuid_IsActive",
                table: "T_Recommend",
                columns: new[] { "RecipeGuid", "IsActive" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 删除复合唯一索引
            migrationBuilder.DropIndex(
                name: "IX_T_Recommend_RecipeGuid_IsActive",
                table: "T_Recommend");

            // 恢复原有的单列唯一索引（如果你需要回滚）
            migrationBuilder.CreateIndex(
                name: "IX_T_Recommend_RecipeGuid",
                table: "T_Recommend",
                column: "RecipeGuid",
                unique: true);
        }
    }
}
