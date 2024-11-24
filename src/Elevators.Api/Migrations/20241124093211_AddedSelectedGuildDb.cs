using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elevators.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedSelectedGuildDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SelectedGuilds",
                columns: table => new
                {
                    GuildId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayedGames = table.Column<int>(type: "INTEGER", nullable: false),
                    JoinedUsers = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    GameRoomId = table.Column<long>(type: "INTEGER", nullable: false),
                    TopFloor = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedGuilds", x => x.GuildId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelectedGuilds");
        }
    }
}
