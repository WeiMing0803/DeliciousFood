using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.DeliciousFood.Core.Model.Migrations
{
    /// <inheritdoc />
    public partial class updateBaseCategoryItemTableAddTypeFiled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "T_BaseCategoryItem",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "T_BaseCategoryItem");
        }
    }
}
