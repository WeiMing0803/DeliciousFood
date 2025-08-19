using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.DeliciousFood.Core.Model.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceUniqueIndexWithFiltered : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_T_Recommend_RecipeGuid_IsActive",
                table: "T_Recommend");

            // 添加只限制 IsActive = 1 的唯一索引
            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX IX_T_Recommend_RecipeGuid_ActiveOnly
                ON T_Recommend(RecipeGuid)
                WHERE IsActive = 1
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_T_Recommend_RecipeGuid_IsActive",
                table: "T_Recommend",
                columns: new[] { "RecipeGuid", "IsActive" },
                unique: true);
        }
    }
}
