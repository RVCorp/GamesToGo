using Microsoft.EntityFrameworkCore.Migrations;

namespace GamesToGo.Desktop.Migrations
{
    public partial class ContextCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "File",
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
                    table.PrimaryKey("PK_File", x => x.FileID);
                });

            migrationBuilder.CreateTable(
                name: "ProyectInfo",
                columns: table => new
                {
                    LocalProyectID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatorID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    NumberPlayers = table.Column<int>(nullable: false),
                    NumberCards = table.Column<int>(nullable: false),
                    NumberTokens = table.Column<int>(nullable: false),
                    NumberBoxes = table.Column<int>(nullable: false),
                    OnlineProyecrID = table.Column<int>(nullable: false),
                    ModerationStatus = table.Column<int>(nullable: false),
                    ComunityStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProyectInfo", x => x.LocalProyectID);
                });

            migrationBuilder.CreateTable(
                name: "FileRelation",
                columns: table => new
                {
                    ProyectID = table.Column<int>(nullable: false),
                    FileID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileRelation", x => new { x.FileID, x.ProyectID });
                    table.ForeignKey(
                        name: "FK_FileRelation_File_FileID",
                        column: x => x.FileID,
                        principalTable: "File",
                        principalColumn: "FileID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileRelation_ProyectInfo_ProyectID",
                        column: x => x.ProyectID,
                        principalTable: "ProyectInfo",
                        principalColumn: "LocalProyectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileRelation_ProyectID",
                table: "FileRelation",
                column: "ProyectID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileRelation");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "ProyectInfo");
        }
    }
}
