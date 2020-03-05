using Microsoft.EntityFrameworkCore.Migrations;

namespace GamesToGo.Desktop.Migrations
{
    public partial class MaxMinPlayers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileRelation_File_FileID",
                table: "FileRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_FileRelation_ProyectInfo_ProyectID",
                table: "FileRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProyectInfo",
                table: "ProyectInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileRelation",
                table: "FileRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_File",
                table: "File");

            migrationBuilder.DropColumn(
                name: "NumberPlayers",
                table: "ProyectInfo");

            migrationBuilder.RenameTable(
                name: "ProyectInfo",
                newName: "Proyects");

            migrationBuilder.RenameTable(
                name: "FileRelation",
                newName: "Relations");

            migrationBuilder.RenameTable(
                name: "File",
                newName: "Files");

            migrationBuilder.RenameIndex(
                name: "IX_FileRelation_ProyectID",
                table: "Relations",
                newName: "IX_Relations_ProyectID");

            migrationBuilder.AddColumn<int>(
                name: "MaxNumberPlayers",
                table: "Proyects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinNumberPlayers",
                table: "Proyects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Proyects",
                table: "Proyects",
                column: "LocalProyectID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Relations",
                table: "Relations",
                columns: new[] { "FileID", "ProyectID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files",
                table: "Files",
                column: "FileID");

            migrationBuilder.AddForeignKey(
                name: "FK_Relations_Files_FileID",
                table: "Relations",
                column: "FileID",
                principalTable: "Files",
                principalColumn: "FileID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Relations_Proyects_ProyectID",
                table: "Relations",
                column: "ProyectID",
                principalTable: "Proyects",
                principalColumn: "LocalProyectID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relations_Files_FileID",
                table: "Relations");

            migrationBuilder.DropForeignKey(
                name: "FK_Relations_Proyects_ProyectID",
                table: "Relations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Relations",
                table: "Relations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Proyects",
                table: "Proyects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Files",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "MaxNumberPlayers",
                table: "Proyects");

            migrationBuilder.DropColumn(
                name: "MinNumberPlayers",
                table: "Proyects");

            migrationBuilder.RenameTable(
                name: "Relations",
                newName: "FileRelation");

            migrationBuilder.RenameTable(
                name: "Proyects",
                newName: "ProyectInfo");

            migrationBuilder.RenameTable(
                name: "Files",
                newName: "File");

            migrationBuilder.RenameIndex(
                name: "IX_Relations_ProyectID",
                table: "FileRelation",
                newName: "IX_FileRelation_ProyectID");

            migrationBuilder.AddColumn<int>(
                name: "NumberPlayers",
                table: "ProyectInfo",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileRelation",
                table: "FileRelation",
                columns: new[] { "FileID", "ProyectID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProyectInfo",
                table: "ProyectInfo",
                column: "LocalProyectID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_File",
                table: "File",
                column: "FileID");

            migrationBuilder.AddForeignKey(
                name: "FK_FileRelation_File_FileID",
                table: "FileRelation",
                column: "FileID",
                principalTable: "File",
                principalColumn: "FileID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileRelation_ProyectInfo_ProyectID",
                table: "FileRelation",
                column: "ProyectID",
                principalTable: "ProyectInfo",
                principalColumn: "LocalProyectID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
