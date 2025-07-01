using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingi_Storage.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefreshMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d290f1ee-6c54-4b01-90e6-d701748f0851",
                column: "ConcurrencyStamp",
                value: "665f93bf-cc20-429b-9bc9-1091bf0eb1c6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d290f1ee-6c54-4b01-90e6-d701748f0851",
                column: "ConcurrencyStamp",
                value: "1212eb88-36df-49e3-8056-9f5d65d1f341");
        }
    }
}
