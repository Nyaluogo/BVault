using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bingi_Storage.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingCartMig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "PaymentMethods",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PaymentMethods",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "PaymentMethods",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "Id", "ApiKey", "CreatedAt", "Description", "IsActive", "Name", "PayType", "SecretKey", "TransProvider", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), "Pay directly with M-Pesa", true, "M-Pesa", 4, null, 4, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), "Pay with multiple methods via Flutterwave", true, "Flutterwave", 2, null, 6, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), "Pay with cryptocurrency", true, "Crypto Wallet", 3, null, 9, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc) },
                    { 4, null, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), "Pay with Airtel Money", true, "Airtel Money", 4, null, 7, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "PaymentMethods");
        }
    }
}
