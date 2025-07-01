using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingi_Storage.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefreshMig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d290f1ee-6c54-4b01-90e6-d701748f0851",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bf0d453f-5d6e-43d3-90f0-ae3c65e9f323", "a3f1c2d4-5e6f-7g8h-9i0j-k1l2m3n4o5p6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d290f1ee-6c54-4b01-90e6-d701748f0851",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "665f93bf-cc20-429b-9bc9-1091bf0eb1c6", "d290f1ee-6c54-4b01-90e6-d701748f085r" });
        }
    }
}
