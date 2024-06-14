using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class newdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IMageUrl",
                table: "FoodItems");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "FoodItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_CategoryId",
                table: "FoodItems",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Category_CategoryId",
                table: "FoodItems",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Category_CategoryId",
                table: "FoodItems");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_FoodItems_CategoryId",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "FoodItems");

            migrationBuilder.AddColumn<string>(
                name: "IMageUrl",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
