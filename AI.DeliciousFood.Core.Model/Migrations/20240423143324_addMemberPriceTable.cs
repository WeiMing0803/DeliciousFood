using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.DeliciousFood.Core.Model.Migrations
{
    /// <inheritdoc />
    public partial class addMemberPriceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_MemberPrice",
                columns: table => new
                {
                    MenberPriceGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    MemberName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MemberPriceConfig", x => x.MenberPriceGuid)
                        .Annotation("SqlServer:Clustered", false);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_MemberPriceConfig");
        }
    }
}
