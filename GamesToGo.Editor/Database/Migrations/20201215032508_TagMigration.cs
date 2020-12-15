using System;
using Microsoft.EntityFrameworkCore.Migrations;
// ReSharper disable ArgumentsStyleStringLiteral

namespace GamesToGo.Editor.Database.Migrations
{
    public partial class TagMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Relations");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    LocalProjectID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatorID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    MinNumberPlayers = table.Column<int>(nullable: false),
                    MaxNumberPlayers = table.Column<int>(nullable: false),
                    NumberCards = table.Column<int>(nullable: false),
                    NumberTokens = table.Column<int>(nullable: false),
                    NumberBoxes = table.Column<int>(nullable: false),
                    OnlineProjectID = table.Column<int>(nullable: false),
                    CommunityStatus = table.Column<int>(nullable: false),
                    LastEdited = table.Column<DateTime>(nullable: false),
                    FileID = table.Column<int>(nullable: false),
                    NumberBoards = table.Column<int>(nullable: false),
                    ImageRelationID = table.Column<int>(nullable: true),
                    Tags = table.Column<uint>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.LocalProjectID);
                    table.ForeignKey(
                        name: "FK_Projects_Files_FileID",
                        column: x => x.FileID,
                        principalTable: "Files",
                        principalColumn: "FileID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_Relations_ImageRelationID",
                        column: x => x.ImageRelationID,
                        principalTable: "Relations",
                        principalColumn: "RelationID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Relations",
                columns: table => new
                {
                    RelationID = table.Column<int>(nullable: false),
                    ProjectID = table.Column<int>(nullable: false),
                    FileID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relations", x => x.RelationID);
                    table.ForeignKey(
                        name: "FK_Relations_Files_FileID",
                        column: x => x.FileID,
                        principalTable: "Files",
                        principalColumn: "FileID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relations_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "LocalProjectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Relations_ProjectID",
                table: "Relations",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FileID",
                table: "Projects",
                column: "FileID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Relations_FileID",
                table: "Relations",
                column: "FileID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ImageRelationID",
                table: "Projects",
                column: "ImageRelationID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                // ReSharper disable once StringLiteralTypo
                name: "ComunityStatus",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModerationStatus",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"UPDATE Projects SET ComunityStatus = (SELECT pi.CommunityStatus from Projects pi Where Projects.LocalProjectID == pi.LocalProjectID);");

            migrationBuilder.DropColumn(
                name: "CommunityStatus",
                table: "Projects");
        }
    }
}
