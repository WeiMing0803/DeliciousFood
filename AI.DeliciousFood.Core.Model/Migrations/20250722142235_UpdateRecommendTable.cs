using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.DeliciousFood.Core.Model.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecommendTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_T_Recommend_RecipeGuid",
                table: "T_Recommend",
                column: "RecipeGuid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Recommend_T_Recipe_RecipeGuid",
                table: "T_Recommend",
                column: "RecipeGuid",
                principalTable: "T_Recipe",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Recommend_T_Recipe_RecipeGuid",
                table: "T_Recommend");

            migrationBuilder.DropIndex(
                name: "IX_T_Recommend_RecipeGuid",
                table: "T_Recommend");
        }
    }
}
