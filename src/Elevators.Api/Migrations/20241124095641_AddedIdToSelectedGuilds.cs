using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elevators.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedIdToSelectedGuilds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SelectedGuilds",
                table: "SelectedGuilds");

            migrationBuilder.AlterColumn<long>(
                name: "GuildId",
                table: "SelectedGuilds",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SelectedGuilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SelectedGuilds",
                table: "SelectedGuilds",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SelectedGuilds",
                table: "SelectedGuilds");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SelectedGuilds");

            migrationBuilder.AlterColumn<long>(
                name: "GuildId",
                table: "SelectedGuilds",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SelectedGuilds",
                table: "SelectedGuilds",
                column: "GuildId");
        }
    }
}
