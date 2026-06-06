using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catkaa.MicroPms.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomCleaningAndPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastCleanedAt",
                table: "Rooms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomPassword",
                table: "Rooms",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastCleanedAt",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomPassword",
                table: "Rooms");
        }
    }
}
