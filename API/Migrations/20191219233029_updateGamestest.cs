using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class updateGamestest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    teamOneWin = table.Column<bool>(nullable: false),
                    teamTwoWin = table.Column<bool>(nullable: false),
                    draw = table.Column<bool>(nullable: false),
                    teamOneList = table.Column<string>(nullable: true),
                    teamTwoList = table.Column<string>(nullable: true),
                    date = table.Column<DateTime>(nullable: false),
                    deleted = table.Column<bool>(nullable: false)
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    gid = table.Column<int>(nullable: false),
                    pid = table.Column<int>(nullable: false),
                    teamOneScore = table.Column<int>(nullable: false),
                    teamTwoScore = table.Column<int>(nullable: false),
                    result = table.Column<string>(nullable: true),
                    teamOne = table.Column<bool>(nullable: false),
                    teamTwo = table.Column<bool>(nullable: false),
                    draw = table.Column<bool>(nullable: false)
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    wins = table.Column<int>(nullable: false),
                    losses = table.Column<int>(nullable: false),
                    draws = table.Column<int>(nullable: false),
                    gamesWon = table.Column<int>(nullable: false),
                    gamesLost = table.Column<int>(nullable: false),
                    attented = table.Column<int>(nullable: false),
                    deleted = table.Column<bool>(nullable: false)
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
