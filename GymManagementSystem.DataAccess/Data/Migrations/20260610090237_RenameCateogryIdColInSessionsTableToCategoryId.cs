using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameCateogryIdColInSessionsTableToCategoryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Categories_CateogryId",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "CateogryId",
                table: "Sessions",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_CateogryId",
                table: "Sessions",
                newName: "IX_Sessions_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Categories_CategoryId",
                table: "Sessions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Categories_CategoryId",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Sessions",
                newName: "CateogryId");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_CategoryId",
                table: "Sessions",
                newName: "IX_Sessions_CateogryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Categories_CateogryId",
                table: "Sessions",
                column: "CateogryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
