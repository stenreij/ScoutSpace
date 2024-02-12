using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PlayerNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notitie_Player_playerId",
                table: "Notitie");

            migrationBuilder.AddForeignKey(
                name: "FK_Notitie_Player_playerId",
                table: "Notitie",
                column: "playerId",
                principalTable: "Player",
                principalColumn: "playerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notitie_Player_playerId",
                table: "Notitie");

            migrationBuilder.AddForeignKey(
                name: "FK_Notitie_Player_playerId",
                table: "Notitie",
                column: "playerId",
                principalTable: "Player",
                principalColumn: "playerId");
        }
    }
}
