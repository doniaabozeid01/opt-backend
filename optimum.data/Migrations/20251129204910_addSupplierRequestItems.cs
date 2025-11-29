using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace optimum.data.Migrations
{
    /// <inheritdoc />
    public partial class addSupplierRequestItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupplierRequestItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierRequestId = table.Column<int>(type: "int", nullable: false),
                    SchoolConfirmedRequestItemId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierRequestItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierRequestItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierRequestItem_SchoolConfirmedRequestItems_SchoolConfirmedRequestItemId",
                        column: x => x.SchoolConfirmedRequestItemId,
                        principalTable: "SchoolConfirmedRequestItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupplierRequestItem_SupplierRequest_SupplierRequestId",
                        column: x => x.SupplierRequestId,
                        principalTable: "SupplierRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRequestItem_ProductId",
                table: "SupplierRequestItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRequestItem_SchoolConfirmedRequestItemId",
                table: "SupplierRequestItem",
                column: "SchoolConfirmedRequestItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRequestItem_SupplierRequestId",
                table: "SupplierRequestItem",
                column: "SupplierRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierRequestItem");
        }
    }
}
