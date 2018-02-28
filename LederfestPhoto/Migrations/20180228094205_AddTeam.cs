using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LederfestPhoto.Migrations
{
    public partial class AddTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team",
                table: "Photos");

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "Photos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_TeamId",
                table: "Photos",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Teams_TeamId",
                table: "Photos",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Teams_TeamId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Photos_TeamId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Photos");

            migrationBuilder.AddColumn<Guid>(
                name: "Team",
                table: "Photos",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
