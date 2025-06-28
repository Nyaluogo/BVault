using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingi_Storage.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductUpgrade2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDemo",
                table: "ProductPayload",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ExternalLinks",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoTrailerUrl",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ExternalLinks", "Genre", "VideoTrailerUrl" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ExternalLinks", "Genre", "VideoTrailerUrl" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ExternalLinks", "Genre", "VideoTrailerUrl" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ExternalLinks", "Genre", "VideoTrailerUrl" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ExternalLinks", "Genre", "VideoTrailerUrl" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ExternalLinks", "Genre", "VideoTrailerUrl" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ExternalLinks", "Genre", "VideoTrailerUrl" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ExternalLinks", "Genre", "VideoTrailerUrl" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ExternalLinks", "Genre", "VideoTrailerUrl" },
                values: new object[] { null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDemo",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "ExternalLinks",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "VideoTrailerUrl",
                table: "Product");
        }
    }
}
