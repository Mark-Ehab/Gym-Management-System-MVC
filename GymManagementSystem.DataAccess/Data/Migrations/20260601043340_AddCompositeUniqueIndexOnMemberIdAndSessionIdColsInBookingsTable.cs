using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCompositeUniqueIndexOnMemberIdAndSessionIdColsInBookingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_MemberId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MemberId_SessionId",
                table: "Bookings",
                columns: new[] { "MemberId", "SessionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_MemberId_SessionId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MemberId",
                table: "Bookings",
                column: "MemberId");
        }
    }
}
