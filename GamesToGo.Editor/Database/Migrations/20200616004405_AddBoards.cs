using Microsoft.EntityFrameworkCore.Migrations;
// ReSharper disable All

namespace GamesToGo.Editor.Database.Migrations
{
    public partial class AddBoards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberBoards",
                table: "Projects",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberBoards",
                table: "Projects");
        }
    }
}
