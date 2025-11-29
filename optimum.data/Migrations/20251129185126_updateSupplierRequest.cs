using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace optimum.data.Migrations
{
    /// <inheritdoc />
    public partial class updateSupplierRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SupplierOffers_SupplierRequestId",
                table: "SupplierOffers");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOffers_SupplierRequestId",
                table: "SupplierOffers",
                column: "SupplierRequestId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SupplierOffers_SupplierRequestId",
                table: "SupplierOffers");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOffers_SupplierRequestId",
                table: "SupplierOffers",
                column: "SupplierRequestId");
        }
    }
}
