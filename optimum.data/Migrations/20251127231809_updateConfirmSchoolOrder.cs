using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace optimum.data.Migrations
{
    /// <inheritdoc />
    public partial class updateConfirmSchoolOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AIParsedItemId",
                table: "SchoolConfirmedRequestItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "SchoolConfirmedRequestItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolConfirmedRequestItems_AIParsedItemId",
                table: "SchoolConfirmedRequestItems",
                column: "AIParsedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolConfirmedRequestItems_ProductId",
                table: "SchoolConfirmedRequestItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolConfirmedRequestItems_AIParsedRequestItems_AIParsedItemId",
                table: "SchoolConfirmedRequestItems",
                column: "AIParsedItemId",
                principalTable: "AIParsedRequestItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolConfirmedRequestItems_Products_ProductId",
                table: "SchoolConfirmedRequestItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolConfirmedRequestItems_AIParsedRequestItems_AIParsedItemId",
                table: "SchoolConfirmedRequestItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolConfirmedRequestItems_Products_ProductId",
                table: "SchoolConfirmedRequestItems");

            migrationBuilder.DropIndex(
                name: "IX_SchoolConfirmedRequestItems_AIParsedItemId",
                table: "SchoolConfirmedRequestItems");

            migrationBuilder.DropIndex(
                name: "IX_SchoolConfirmedRequestItems_ProductId",
                table: "SchoolConfirmedRequestItems");

            migrationBuilder.DropColumn(
                name: "AIParsedItemId",
                table: "SchoolConfirmedRequestItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "SchoolConfirmedRequestItems");
        }
    }
}
