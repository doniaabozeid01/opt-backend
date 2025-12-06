using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace optimum.data.Migrations
{
    /// <inheritdoc />
    public partial class addRatingResultToSupplierAndAddSupplierRatingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AverageRating",
                table: "Suppliers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RatingCount",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SupplierRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    SupplierRequestId = table.Column<int>(type: "int", nullable: true),
                    SupplierOfferId = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierRating_SupplierOffers_SupplierOfferId",
                        column: x => x.SupplierOfferId,
                        principalTable: "SupplierOffers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupplierRating_SupplierRequest_SupplierRequestId",
                        column: x => x.SupplierRequestId,
                        principalTable: "SupplierRequest",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupplierRating_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRating_SupplierId",
                table: "SupplierRating",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRating_SupplierOfferId",
                table: "SupplierRating",
                column: "SupplierOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRating_SupplierRequestId",
                table: "SupplierRating",
                column: "SupplierRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierRating");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "RatingCount",
                table: "Suppliers");
        }
    }
}
