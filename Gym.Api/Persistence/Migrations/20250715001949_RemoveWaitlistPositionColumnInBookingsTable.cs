using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWaitlistPositionColumnInBookingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WaitlistPosition",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WaitlistPosition",
                table: "Bookings",
                type: "int",
                nullable: true);
        }
    }
}
