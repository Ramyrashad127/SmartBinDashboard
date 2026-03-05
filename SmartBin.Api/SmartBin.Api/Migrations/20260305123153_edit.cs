using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBin.Api.Migrations
{
    /// <inheritdoc />
    public partial class edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Bins_BinId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_WasteTypes_Bins_BinId",
                table: "WasteTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WasteTypes",
                table: "WasteTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "WasteTypes",
                newName: "CrowdDensitys");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "SurroundingWastes");

            migrationBuilder.RenameIndex(
                name: "IX_WasteTypes_BinId",
                table: "CrowdDensitys",
                newName: "IX_CrowdDensitys_BinId");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_BinId",
                table: "SurroundingWastes",
                newName: "IX_SurroundingWastes_BinId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CrowdDensitys",
                table: "CrowdDensitys",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurroundingWastes",
                table: "SurroundingWastes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CrowdDensitys_Bins_BinId",
                table: "CrowdDensitys",
                column: "BinId",
                principalTable: "Bins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurroundingWastes_Bins_BinId",
                table: "SurroundingWastes",
                column: "BinId",
                principalTable: "Bins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrowdDensitys_Bins_BinId",
                table: "CrowdDensitys");

            migrationBuilder.DropForeignKey(
                name: "FK_SurroundingWastes_Bins_BinId",
                table: "SurroundingWastes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurroundingWastes",
                table: "SurroundingWastes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CrowdDensitys",
                table: "CrowdDensitys");

            migrationBuilder.RenameTable(
                name: "SurroundingWastes",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "CrowdDensitys",
                newName: "WasteTypes");

            migrationBuilder.RenameIndex(
                name: "IX_SurroundingWastes_BinId",
                table: "Roles",
                newName: "IX_Roles_BinId");

            migrationBuilder.RenameIndex(
                name: "IX_CrowdDensitys_BinId",
                table: "WasteTypes",
                newName: "IX_WasteTypes_BinId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WasteTypes",
                table: "WasteTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Bins_BinId",
                table: "Roles",
                column: "BinId",
                principalTable: "Bins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WasteTypes_Bins_BinId",
                table: "WasteTypes",
                column: "BinId",
                principalTable: "Bins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
