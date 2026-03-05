using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBin.Api.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrowdDensitys_Bins_BinId",
                table: "CrowdDensitys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CrowdDensitys",
                table: "CrowdDensitys");

            migrationBuilder.RenameTable(
                name: "CrowdDensitys",
                newName: "CrowdDensities");

            migrationBuilder.RenameIndex(
                name: "IX_CrowdDensitys_BinId",
                table: "CrowdDensities",
                newName: "IX_CrowdDensities_BinId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CrowdDensities",
                table: "CrowdDensities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CrowdDensities_Bins_BinId",
                table: "CrowdDensities",
                column: "BinId",
                principalTable: "Bins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrowdDensities_Bins_BinId",
                table: "CrowdDensities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CrowdDensities",
                table: "CrowdDensities");

            migrationBuilder.RenameTable(
                name: "CrowdDensities",
                newName: "CrowdDensitys");

            migrationBuilder.RenameIndex(
                name: "IX_CrowdDensities_BinId",
                table: "CrowdDensitys",
                newName: "IX_CrowdDensitys_BinId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CrowdDensitys",
                table: "CrowdDensitys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CrowdDensitys_Bins_BinId",
                table: "CrowdDensitys",
                column: "BinId",
                principalTable: "Bins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
