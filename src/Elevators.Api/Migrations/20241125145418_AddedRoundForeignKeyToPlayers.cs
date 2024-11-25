using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elevators.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoundForeignKeyToPlayers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Rounds_RoundId",
                table: "Player");

            migrationBuilder.AlterColumn<int>(
                name: "RoundId",
                table: "Player",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Rounds_RoundId",
                table: "Player",
                column: "RoundId",
                principalTable: "Rounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Rounds_RoundId",
                table: "Player");

            migrationBuilder.AlterColumn<int>(
                name: "RoundId",
                table: "Player",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Rounds_RoundId",
                table: "Player",
                column: "RoundId",
                principalTable: "Rounds",
                principalColumn: "Id");
        }
    }
}
