using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBin.Api.Migrations
{
    /// <inheritdoc />
    public partial class start : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentificationToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BinSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelPercentage = table.Column<float>(type: "real", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BinId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BinSections_Bins_BinId",
                        column: x => x.BinId,
                        principalTable: "Bins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BinSections_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CrowdDensities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DensityLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PeopleCount = table.Column<int>(type: "int", nullable: false),
                    AiConfidence = table.Column<float>(type: "real", nullable: false),
                    TimeStmp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BinId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrowdDensities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrowdDensities_Bins_BinId",
                        column: x => x.BinId,
                        principalTable: "Bins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurroundingWastes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WasteLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetectedObjectsCount = table.Column<int>(type: "int", nullable: false),
                    AiConfidence = table.Column<float>(type: "real", nullable: false),
                    TimeStmp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BinId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurroundingWastes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurroundingWastes_Bins_BinId",
                        column: x => x.BinId,
                        principalTable: "Bins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStmp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AiConfidence = table.Column<float>(type: "real", nullable: true),
                    BinId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Bins_BinId",
                        column: x => x.BinId,
                        principalTable: "Bins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bins_UserId",
                table: "Bins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BinSections_BinId",
                table: "BinSections",
                column: "BinId");

            migrationBuilder.CreateIndex(
                name: "IX_BinSections_MaterialId",
                table: "BinSections",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CrowdDensities_BinId",
                table: "CrowdDensities",
                column: "BinId");

            migrationBuilder.CreateIndex(
                name: "IX_SurroundingWastes_BinId",
                table: "SurroundingWastes",
                column: "BinId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BinId",
                table: "Transactions",
                column: "BinId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_MaterialId",
                table: "Transactions",
                column: "MaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BinSections");

            migrationBuilder.DropTable(
                name: "CrowdDensities");

            migrationBuilder.DropTable(
                name: "SurroundingWastes");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Bins");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
