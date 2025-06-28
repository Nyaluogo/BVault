using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingi_Storage.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductUpdateMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BinaryUrl",
                table: "ProductMedia",
                newName: "PreviewImageUrl");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Product",
                newName: "SalePrice");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductMedia",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProductMedia",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductMedia",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CustomUrl",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultPrice",
                table: "Product",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Product",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Documentation",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAIGen",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PricingState",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductPayload",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DownloadCount = table.Column<int>(type: "int", nullable: false),
                    ProductPublishingStatus = table.Column<int>(type: "int", nullable: false),
                    _Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPayload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPayload_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CustomUrl", "DefaultPrice", "Discount", "Documentation", "IsAIGen", "PricingState", "Tags" },
                values: new object[] { null, 0.0m, 0.0m, null, false, 2, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CustomUrl", "DefaultPrice", "Discount", "Documentation", "IsAIGen", "PricingState", "Tags" },
                values: new object[] { null, 0.0m, 0.0m, null, false, 2, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CustomUrl", "DefaultPrice", "Discount", "Documentation", "IsAIGen", "PricingState", "Tags" },
                values: new object[] { null, 0.0m, 0.0m, null, false, 2, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CustomUrl", "DefaultPrice", "Discount", "Documentation", "IsAIGen", "PricingState", "Tags" },
                values: new object[] { null, 0.0m, 0.0m, null, false, 2, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CustomUrl", "DefaultPrice", "Discount", "Documentation", "IsAIGen", "PricingState", "Tags" },
                values: new object[] { null, 0.0m, 0.0m, null, false, 2, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CustomUrl", "DefaultPrice", "Discount", "Documentation", "IsAIGen", "PricingState", "Tags" },
                values: new object[] { null, 0.0m, 0.0m, null, false, 2, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CustomUrl", "DefaultPrice", "Discount", "Documentation", "IsAIGen", "PricingState", "Tags" },
                values: new object[] { null, 0.0m, 0.0m, null, false, 2, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CustomUrl", "DefaultPrice", "Discount", "Documentation", "IsAIGen", "PricingState", "Tags" },
                values: new object[] { null, 0.0m, 0.0m, null, false, 2, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CustomUrl", "DefaultPrice", "Discount", "Documentation", "IsAIGen", "PricingState", "Tags" },
                values: new object[] { null, 0.0m, 0.0m, null, false, 2, null });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPayload_ProductId",
                table: "ProductPayload",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProductMedia");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductMedia");

            migrationBuilder.DropColumn(
                name: "CustomUrl",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DefaultPrice",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Documentation",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IsAIGen",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PricingState",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "PreviewImageUrl",
                table: "ProductMedia",
                newName: "BinaryUrl");

            migrationBuilder.RenameColumn(
                name: "SalePrice",
                table: "Product",
                newName: "Price");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductMedia",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
