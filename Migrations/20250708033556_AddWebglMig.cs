using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingi_Storage.Migrations
{
    /// <inheritdoc />
    public partial class AddWebglMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuildTarget",
                table: "ProductPayload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CanvasHeight",
                table: "ProductPayload",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CanvasWidth",
                table: "ProductPayload",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableAudioAutoPlay",
                table: "ProductPayload",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableWebGLStreaming",
                table: "ProductPayload",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GameCodeUrl",
                table: "ProductPayload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GameControls",
                table: "ProductPayload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GameDataUrl",
                table: "ProductPayload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GameFrameworkUrl",
                table: "ProductPayload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GameSymbolsUrl",
                table: "ProductPayload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QualitySettings",
                table: "ProductPayload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresKeyboard",
                table: "ProductPayload",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresMouse",
                table: "ProductPayload",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SupportsFullscreen",
                table: "ProductPayload",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SupportsMobile",
                table: "ProductPayload",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UnityVersion",
                table: "ProductPayload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://img.itch.zone/aW1nLzM1Mzc5MDcuanBn/105x83%23/viiAbE.jpg");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ImageUrl", "ShortDescription" },
                values: new object[] { "https://img.itch.zone/aW1nLzE5NDM1MjUyLmpwZw==/105x83%23/bNN%2F9B.jpg", "The Ultimate KEnyan Political strategy game." });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "https://img.itch.zone/aW1nLzE4MTU3OTY4LnBuZw==/105x83%23/5Qz2Ii.png");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ImageUrl", "ShortDescription" },
                values: new object[] { "https://img.itch.zone/aW1nLzExNzk3MzkyLmpwZw==/105x83%23/csj7TB.jpg", "An exciting simulation game." });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "https://img.itch.zone/aW1nLzE4MTU3OTY4LnBuZw==/105x83%23/5Qz2Ii.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildTarget",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "CanvasHeight",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "CanvasWidth",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "EnableAudioAutoPlay",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "EnableWebGLStreaming",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "GameCodeUrl",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "GameControls",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "GameDataUrl",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "GameFrameworkUrl",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "GameSymbolsUrl",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "QualitySettings",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "RequiresKeyboard",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "RequiresMouse",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "SupportsFullscreen",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "SupportsMobile",
                table: "ProductPayload");

            migrationBuilder.DropColumn(
                name: "UnityVersion",
                table: "ProductPayload");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://example.com/sample-game.jpg");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ImageUrl", "ShortDescription" },
                values: new object[] { "https://example.com/sample-game.jpg", "An exciting strategy game." });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "https://example.com/sample-game.jpg");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ImageUrl", "ShortDescription" },
                values: new object[] { "https://example.com/sample-game.jpg", "An exciting action game." });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "https://example.com/sample-game.jpg");
        }
    }
}
