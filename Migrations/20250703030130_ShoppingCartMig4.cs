using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingi_Storage.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingCartMig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalReference",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalReference",
                table: "Transactions");
        }
    }
}
