using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace optimum.data.Migrations
{
    /// <inheritdoc />
    public partial class makeProductIdNullableInSupplierRequestItemAndSchoolConfirmedRequestItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolConfirmedRequestItems_Products_ProductId",
                table: "SchoolConfirmedRequestItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierRequestItem_Products_ProductId",
                table: "SupplierRequestItem");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "SupplierRequestItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "SchoolConfirmedRequestItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolConfirmedRequestItems_Products_ProductId",
                table: "SchoolConfirmedRequestItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierRequestItem_Products_ProductId",
                table: "SupplierRequestItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolConfirmedRequestItems_Products_ProductId",
                table: "SchoolConfirmedRequestItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierRequestItem_Products_ProductId",
                table: "SupplierRequestItem");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "SupplierRequestItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "SchoolConfirmedRequestItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolConfirmedRequestItems_Products_ProductId",
                table: "SchoolConfirmedRequestItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierRequestItem_Products_ProductId",
                table: "SupplierRequestItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
