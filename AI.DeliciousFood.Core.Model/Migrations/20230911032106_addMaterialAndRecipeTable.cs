using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.DeliciousFood.Core.Model.Migrations
{
    public partial class addMaterialAndRecipeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Recipe",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RecipeName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    FinishedPicture = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    RecipeDescription = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    RoductionDifficulty = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    TasksTime = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Flavors = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CookingCraft = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    UseKitchenUtensils = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Practice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tips = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsApproval = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Recipe", x => x.Guid)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_T_Recipe_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_Material",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IngredientName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Usage = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Material", x => x.Guid)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_T_Material_T_Recipe_RecipeGuid",
                        column: x => x.RecipeGuid,
                        principalTable: "T_Recipe",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_Material_RecipeGuid",
                table: "T_Material",
                column: "RecipeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_T_Recipe_UserId",
                table: "T_Recipe",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_Material");

            migrationBuilder.DropTable(
                name: "T_Recipe");
        }
    }
}
