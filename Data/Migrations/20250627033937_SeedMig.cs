using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bingi_Storage.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "AgeRestriction", "AverageRating", "CreatedAt", "DeleteDate", "Description", "DownloadCount", "FileSize", "ImageUrl", "IsBettingEnabled", "Price", "ProductPublishingStatus", "PublisherId", "ReleaseDate", "ShortDescription", "SuspensionDate", "SystemRequirements", "Title", "TotalRatings", "UpdatedAt", "Version" },
                values: new object[,]
                {
                    { 1, 18, 4.5m, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), null, "This is a detailed description of the sample game.", 1000, 5.0m, "https://example.com/sample-game.jpg", false, 29.99m, 0, null, null, "An exciting action game.", null, "Windows 10 or higher", "BINGIMAN", 200, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), 1.0m },
                    { 2, 18, 4.5m, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), null, "This is a detailed description of the sample game.", 1000, 5.0m, "https://example.com/sample-game.jpg", false, 29.99m, 0, null, null, "An exciting action game.", null, "Windows 10 or higher", "Savage Gears", 200, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), 1.0m },
                    { 3, 18, 4.5m, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), null, "This is a detailed description of the sample game.", 1000, 5.0m, "https://example.com/sample-game.jpg", false, 29.99m, 0, null, null, "An exciting action game.", null, "Windows 10 or higher", "BINGIMAN 3", 200, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), 1.0m },
                    { 4, 18, 4.5m, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), null, "This is a detailed description of the sample game.", 1000, 5.0m, "https://example.com/sample-game.jpg", false, 29.99m, 0, null, null, "An exciting strategy game.", null, "Windows 10 or higher", "Debe", 200, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), 1.0m },
                    { 5, 18, 4.5m, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), null, "This is a detailed description of the sample game.", 1000, 5.0m, "https://example.com/sample-game.jpg", false, 29.99m, 0, null, null, "An exciting racing game.", null, "Windows 10 or higher", "Political Rally", 200, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), 1.0m },
                    { 6, 18, 4.5m, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), null, "This is a detailed description of the sample game.", 1000, 5.0m, "https://example.com/sample-game.jpg", false, 29.99m, 0, null, null, "An exciting action game.", null, "Windows 10 or higher", "Nafas", 200, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), 1.0m },
                    { 7, 18, 4.5m, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), null, "This is a detailed description of the sample game.", 1000, 5.0m, "https://example.com/sample-game.jpg", false, 29.99m, 0, null, null, "An exciting action game.", null, "Windows 10 or higher", "Political Fighter", 200, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), 1.0m },
                    { 8, 18, 4.5m, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), null, "This is a detailed description of the sample game.", 1000, 5.0m, "https://example.com/sample-game.jpg", false, 29.99m, 0, null, null, "An exciting action game.", null, "Windows 10 or higher", "Bingivision", 200, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), 1.0m },
                    { 9, 18, 4.5m, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), null, "This is a detailed description of the sample game.", 1000, 5.0m, "https://example.com/sample-game.jpg", false, 29.99m, 0, null, null, "An exciting action game.", null, "Windows 10 or higher", "Armed Rebellion", 200, new DateTime(2025, 6, 26, 16, 46, 50, 0, DateTimeKind.Utc), 1.0m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
