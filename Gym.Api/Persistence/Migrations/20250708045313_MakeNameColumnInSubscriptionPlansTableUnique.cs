using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeNameColumnInSubscriptionPlansTableUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlans_Name",
                table: "SubscriptionPlans",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubscriptionPlans_Name",
                table: "SubscriptionPlans");
        }
    }
}
