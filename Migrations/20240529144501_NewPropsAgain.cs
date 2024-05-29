using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class NewPropsAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TableBookings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TableBookings_UserId",
                table: "TableBookings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TableBookings_AspNetUsers_UserId",
                table: "TableBookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableBookings_AspNetUsers_UserId",
                table: "TableBookings");

            migrationBuilder.DropIndex(
                name: "IX_TableBookings_UserId",
                table: "TableBookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TableBookings");
        }
    }
}
