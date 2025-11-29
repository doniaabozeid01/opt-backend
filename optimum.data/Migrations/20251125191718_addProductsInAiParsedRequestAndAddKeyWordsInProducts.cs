using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace optimum.data.Migrations
{
    /// <inheritdoc />
    public partial class addProductsInAiParsedRequestAndAddKeyWordsInProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "AIParsedRequestItems",
                newName: "ExtractedName");

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Confidence",
                table: "AIParsedRequestItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "AIParsedRequestItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AIParsedRequestItems_ProductId",
                table: "AIParsedRequestItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AIParsedRequestItems_Products_ProductId",
                table: "AIParsedRequestItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AIParsedRequestItems_Products_ProductId",
                table: "AIParsedRequestItems");

            migrationBuilder.DropIndex(
                name: "IX_AIParsedRequestItems_ProductId",
                table: "AIParsedRequestItems");

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Confidence",
                table: "AIParsedRequestItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "AIParsedRequestItems");

            migrationBuilder.RenameColumn(
                name: "ExtractedName",
                table: "AIParsedRequestItems",
                newName: "ProductName");
        }
    }
}
