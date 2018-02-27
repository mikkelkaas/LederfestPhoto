using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LederfestPhoto.Migrations
{
    public partial class AddChallenges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Challenge",
                table: "Photos");

            migrationBuilder.AddColumn<Guid>(
                name: "ChallengeId",
                table: "Photos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Challenges",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenges", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_ChallengeId",
                table: "Photos",
                column: "ChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Challenges_ChallengeId",
                table: "Photos",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Challenges_ChallengeId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "Challenges");

            migrationBuilder.DropIndex(
                name: "IX_Photos_ChallengeId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ChallengeId",
                table: "Photos");

            migrationBuilder.AddColumn<Guid>(
                name: "Challenge",
                table: "Photos",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
