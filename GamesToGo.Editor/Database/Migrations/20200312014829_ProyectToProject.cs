using System;
using Microsoft.EntityFrameworkCore.Migrations;
// ReSharper disable All

namespace GamesToGo.Editor.Database.Migrations
{
    public partial class ProyectToProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proyects");

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
                    MinNumberPlayers = table.Column<int>(nullable: false),
                    MaxNumberPlayers = table.Column<int>(nullable: false),
                    NumberCards = table.Column<int>(nullable: false),
                    NumberTokens = table.Column<int>(nullable: false),
                    NumberBoxes = table.Column<int>(nullable: false),
                    OnlineProjectID = table.Column<int>(nullable: false),
                    ModerationStatus = table.Column<int>(nullable: false),
                    ComunityStatus = table.Column<int>(nullable: false),
                    LastEdited = table.Column<DateTime>(nullable: false),
                    FileID = table.Column<int>(nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Relations",
                columns: table => new
                {
                    ProjectID = table.Column<int>(nullable: false),
                    FileID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relations", x => new { x.FileID, x.ProjectID });
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relations");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.CreateTable(
                name: "Proyects",
                columns: table => new
                {
                    LocalProyectID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ComunityStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatorID = table.Column<int>(type: "INTEGER", nullable: false),
                    FileID = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxNumberPlayers = table.Column<int>(type: "INTEGER", nullable: false),
                    MinNumberPlayers = table.Column<int>(type: "INTEGER", nullable: false),
                    ModerationStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    NumberBoxes = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberCards = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberTokens = table.Column<int>(type: "INTEGER", nullable: false),
                    OnlineProyecrID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyects", x => x.LocalProyectID);
                    table.ForeignKey(
                        name: "FK_Proyects_Files_FileID",
                        column: x => x.FileID,
                        principalTable: "Files",
                        principalColumn: "FileID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relations",
                columns: table => new
                {
                    ProyectID = table.Column<int>(nullable: false),
                    FileID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relations", x => new { x.FileID, x.ProyectID });
                    table.ForeignKey(
                        name: "FK_Relations_Files_FileID",
                        column: x => x.FileID,
                        principalTable: "Files",
                        principalColumn: "FileID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relations_Proyects_ProyectID",
                        column: x => x.ProyectID,
                        principalTable: "Proyects",
                        principalColumn: "LocalProyectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Relations_ProyectID",
                table: "Relations",
                column: "ProyectID");

            migrationBuilder.CreateIndex(
                name: "IX_Proyects_FileID",
                table: "Proyects",
                column: "FileID",
                unique: true);


        }
    }
}
