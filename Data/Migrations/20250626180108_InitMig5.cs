using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingi_Storage.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitMig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserLibraries_LibraryId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LibraryId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LibraryId",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LibraryId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LibraryId",
                table: "AspNetUsers",
                column: "LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserLibraries_LibraryId",
                table: "AspNetUsers",
                column: "LibraryId",
                principalTable: "UserLibraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
