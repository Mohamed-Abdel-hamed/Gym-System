using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSupportsAutoRenewalColumnInSubcriptionPlanes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SupportsAutoRenewal",
                table: "SubscriptionPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportsAutoRenewal",
                table: "SubscriptionPlans");
        }
    }
}
