using Microsoft.EntityFrameworkCore.Migrations;
// ReSharper disable All

namespace GamesToGo.Desktop.Database.Migrations
{
    public partial class AddProyectFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Proyects");

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
                    ComunityStatus = table.Column<int>(nullable: false),
                    FileID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyects", x => x.LocalProyectID);
                    table.ForeignKey(
                        name: "FK_ProyectInfo_Files_FileID",
                        column: x => x.FileID,
                        principalTable: "Files",
                        principalColumn: "FileID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Proyects");

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
        }
    }
}
