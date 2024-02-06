using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Scout",
                columns: table => new
                {
                    scoutId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    birthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNr = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scout", x => x.scoutId);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    teamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    teamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    division = table.Column<int>(type: "int", nullable: false),
                    contactNr = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.teamId);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    playerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    birthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    residence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phoneNr = table.Column<int>(type: "int", nullable: true),
                    line = table.Column<int>(type: "int", nullable: true),
                    position = table.Column<int>(type: "int", nullable: true),
                    preferedFoot = table.Column<int>(type: "int", nullable: true),
                    teamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.playerId);
                    table.ForeignKey(
                        name: "FK_Player_Team_teamId",
                        column: x => x.teamId,
                        principalTable: "Team",
                        principalColumn: "teamId");
                });

            migrationBuilder.CreateTable(
                name: "Notitie",
                columns: table => new
                {
                    notitieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    playerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notitie", x => x.notitieId);
                    table.ForeignKey(
                        name: "FK_Notitie_Player_playerId",
                        column: x => x.playerId,
                        principalTable: "Player",
                        principalColumn: "playerId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notitie_playerId",
                table: "Notitie",
                column: "playerId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_teamId",
                table: "Player",
                column: "teamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notitie");

            migrationBuilder.DropTable(
                name: "Scout");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Team");
        }
    }
}
