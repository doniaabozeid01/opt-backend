using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace optimum.data.Migrations
{
    /// <inheritdoc />
    public partial class updateSupplierOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "SupplierOffers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchaseCost",
                table: "SupplierOffers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FinalPrice",
                table: "SupplierOffers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryDays",
                table: "SupplierOffers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "SupplierOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "SupplierOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "SupplierOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmedBySupplier",
                table: "SupplierOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OfferType",
                table: "SupplierOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ProfitMarginPercent",
                table: "SupplierOffers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProfitMarginValue",
                table: "SupplierOffers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RawText",
                table: "SupplierOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "SupplierConfirmedAt",
                table: "SupplierOffers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "SupplierOffers");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "SupplierOffers");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "SupplierOffers");

            migrationBuilder.DropColumn(
                name: "IsConfirmedBySupplier",
                table: "SupplierOffers");

            migrationBuilder.DropColumn(
                name: "OfferType",
                table: "SupplierOffers");

            migrationBuilder.DropColumn(
                name: "ProfitMarginPercent",
                table: "SupplierOffers");

            migrationBuilder.DropColumn(
                name: "ProfitMarginValue",
                table: "SupplierOffers");

            migrationBuilder.DropColumn(
                name: "RawText",
                table: "SupplierOffers");

            migrationBuilder.DropColumn(
                name: "SupplierConfirmedAt",
                table: "SupplierOffers");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "SupplierOffers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchaseCost",
                table: "SupplierOffers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FinalPrice",
                table: "SupplierOffers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryDays",
                table: "SupplierOffers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
