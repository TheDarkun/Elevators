using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elevators.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameRoomId",
                table: "SelectedGuilds");

            migrationBuilder.DropColumn(
                name: "JoinedUsers",
                table: "SelectedGuilds");

            migrationBuilder.DropColumn(
                name: "TopFloor",
                table: "SelectedGuilds");

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChannelId = table.Column<Guid>(type: "TEXT", nullable: false),
                    JoinedUsers = table.Column<string>(type: "TEXT", nullable: false),
                    TopFloor = table.Column<int>(type: "INTEGER", nullable: false),
                    GameRoomId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.AddColumn<ulong>(
                name: "GameRoomId",
                table: "SelectedGuilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<string>(
                name: "JoinedUsers",
                table: "SelectedGuilds",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TopFloor",
                table: "SelectedGuilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
