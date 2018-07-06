using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace API.Migrations
{
    public partial class db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    date = table.Column<string>(nullable: true),
                    deleted = table.Column<bool>(nullable: false),
                    teamOneWin = table.Column<bool>(nullable: false),
                    teamTwoWin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GamesWon",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    gid = table.Column<int>(nullable: false),
                    pid = table.Column<int>(nullable: false),
                    result = table.Column<string>(nullable: true),
                    teamOne = table.Column<bool>(nullable: false),
                    teamOneScore = table.Column<int>(nullable: false),
                    teamTwo = table.Column<bool>(nullable: false),
                    teamTwoScore = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamesWon", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    attented = table.Column<int>(nullable: false),
                    deleted = table.Column<bool>(nullable: false),
                    draws = table.Column<int>(nullable: false),
                    gamesLost = table.Column<int>(nullable: false),
                    gamesWon = table.Column<int>(nullable: false),
                    losses = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    wins = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "GamesWon");

            migrationBuilder.DropTable(
                name: "Player");
        }
    }
}
