using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elevators.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedSelectMessageProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "SelectOptionMessageId",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectOptionMessageId",
                table: "Games");
        }
    }
}
