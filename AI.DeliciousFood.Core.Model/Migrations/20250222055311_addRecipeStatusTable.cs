using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.DeliciousFood.Core.Model.Migrations
{
    /// <inheritdoc />
    public partial class addRecipeStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_RecipeStatus",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RecipeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Approver = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_RecipeStatus", x => x.Guid)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_T_RecipeStatus_AspNetUsers_Approver",
                        column: x => x.Approver,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_RecipeStatus_T_Recipe_RecipeGuid",
                        column: x => x.RecipeGuid,
                        principalTable: "T_Recipe",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_RecipeStatus_Approver",
                table: "T_RecipeStatus",
                column: "Approver");

            migrationBuilder.CreateIndex(
                name: "IX_T_RecipeStatus_RecipeGuid",
                table: "T_RecipeStatus",
                column: "RecipeGuid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_RecipeStatus");
        }
    }
}
