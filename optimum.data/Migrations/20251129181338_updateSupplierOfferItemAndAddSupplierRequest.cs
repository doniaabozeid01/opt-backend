using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace optimum.data.Migrations
{
    /// <inheritdoc />
    public partial class updateSupplierOfferItemAndAddSupplierRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplierOffers_SchoolRequests_SchoolRequestId",
                table: "SupplierOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierOffers_Suppliers_SupplierId",
                table: "SupplierOffers");

            migrationBuilder.DropIndex(
                name: "IX_SupplierOffers_SchoolRequestId",
                table: "SupplierOffers");

            migrationBuilder.DropColumn(
                name: "IsSelectedBySchool",
                table: "SupplierOffers");

            migrationBuilder.DropColumn(
                name: "SchoolRequestId",
                table: "SupplierOffers");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "SupplierOffers",
                newName: "SupplierRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplierOffers_SupplierId",
                table: "SupplierOffers",
                newName: "IX_SupplierOffers_SupplierRequestId");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "SupplierOfferItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SupplierRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolRequestId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ViewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastOfferAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierRequest_SchoolRequests_SchoolRequestId",
                        column: x => x.SchoolRequestId,
                        principalTable: "SchoolRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierRequest_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOfferItems_ProductId",
                table: "SupplierOfferItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRequest_SchoolRequestId",
                table: "SupplierRequest",
                column: "SchoolRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRequest_SupplierId",
                table: "SupplierRequest",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierOfferItems_Products_ProductId",
                table: "SupplierOfferItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierOffers_SupplierRequest_SupplierRequestId",
                table: "SupplierOffers",
                column: "SupplierRequestId",
                principalTable: "SupplierRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplierOfferItems_Products_ProductId",
                table: "SupplierOfferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierOffers_SupplierRequest_SupplierRequestId",
                table: "SupplierOffers");

            migrationBuilder.DropTable(
                name: "SupplierRequest");

            migrationBuilder.DropIndex(
                name: "IX_SupplierOfferItems_ProductId",
                table: "SupplierOfferItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "SupplierOfferItems");

            migrationBuilder.RenameColumn(
                name: "SupplierRequestId",
                table: "SupplierOffers",
                newName: "SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplierOffers_SupplierRequestId",
                table: "SupplierOffers",
                newName: "IX_SupplierOffers_SupplierId");

            migrationBuilder.AddColumn<bool>(
                name: "IsSelectedBySchool",
                table: "SupplierOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SchoolRequestId",
                table: "SupplierOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOffers_SchoolRequestId",
                table: "SupplierOffers",
                column: "SchoolRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierOffers_SchoolRequests_SchoolRequestId",
                table: "SupplierOffers",
                column: "SchoolRequestId",
                principalTable: "SchoolRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierOffers_Suppliers_SupplierId",
                table: "SupplierOffers",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
