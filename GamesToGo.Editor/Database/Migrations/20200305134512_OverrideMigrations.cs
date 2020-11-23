using Microsoft.EntityFrameworkCore.Migrations;
// ReSharper disable All

namespace GamesToGo.Editor.Database.Migrations
{
    public partial class OverrideMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    FileID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OriginalName = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    NewName = table.Column<string>(maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.FileID);
                });

            migrationBuilder.CreateTable(
                name: "Proyects",
                columns: table => new
                {
                    LocalProyectID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatorID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    MinNumberPlayers = table.Column<int>(nullable: false),
                    MaxNumberPlayers = table.Column<int>(nullable: false),
                    NumberCards = table.Column<int>(nullable: false),
                    NumberTokens = table.Column<int>(nullable: false),
                    NumberBoxes = table.Column<int>(nullable: false),
                    OnlineProyecrID = table.Column<int>(nullable: false),
                    ModerationStatus = table.Column<int>(nullable: false),
                    ComunityStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyects", x => x.LocalProyectID);
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relations");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Proyects");
        }
    }
}
