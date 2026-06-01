using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFilterForUniqueInexesOnDbTablesToApplyOnlyOnRecordsWhoseIsDeletedIsFalse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trainers_Email",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_Phone",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Members_Email",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_Phone",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_HealthRecords_MemberId",
                table: "HealthRecords");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_MemberId_SessionId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Email",
                table: "Trainers",
                column: "Email",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Phone",
                table: "Trainers",
                column: "Phone",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Members_Email",
                table: "Members",
                column: "Email",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Members_Phone",
                table: "Members",
                column: "Phone",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_MemberId",
                table: "HealthRecords",
                column: "MemberId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MemberId_SessionId",
                table: "Bookings",
                columns: new[] { "MemberId", "SessionId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trainers_Email",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_Phone",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Members_Email",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_Phone",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_HealthRecords_MemberId",
                table: "HealthRecords");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_MemberId_SessionId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Email",
                table: "Trainers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Phone",
                table: "Trainers",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_Email",
                table: "Members",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_Phone",
                table: "Members",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_MemberId",
                table: "HealthRecords",
                column: "MemberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MemberId_SessionId",
                table: "Bookings",
                columns: new[] { "MemberId", "SessionId" },
                unique: true);
        }
    }
}
