using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueToClassAndTrainer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Classes_TrainerId",
                table: "Classes");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TrainerId_Title",
                table: "Classes",
                columns: new[] { "TrainerId", "Title" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Classes_TrainerId_Title",
                table: "Classes");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TrainerId",
                table: "Classes",
                column: "TrainerId");
        }
    }
}
