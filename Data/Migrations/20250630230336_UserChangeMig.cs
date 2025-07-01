using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingi_Storage.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserChangeMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e576",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "UserName" },
                values: new object[] { "83f1174d-2992-419a-b240-0a096a2465cc", "AQAAAAIAAYagAAAAEMG87dSyKDdRxdiZHpJt7OH2xaT/4KEfNeR2PSkLSzHyG6xiU56usSlF1fufuajo8g==", "Alpha" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e577",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "UserName" },
                values: new object[] { "ded66a78-1ee6-43f2-9fd0-87cd3e250951", "AQAAAAIAAYagAAAAEKWzNpB8//TGBW0JaC/n+/OxvIAGzJnwyosGF1NVnY+9drFJpmVw20aj955zzKkiJA==", "user1@bingi.com" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e578",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "UserName" },
                values: new object[] { "f12f8ac0-0be8-499c-bd70-a0a7369ae84d", "AQAAAAIAAYagAAAAEOtBGqqlDOiuoMbBlwW2LVscIbYW4jAYNzWbfvT9j6Ycx1SXGB8crB6UfBwI9rZJ1w==", "user2@bingi.com" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e579",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "UserName" },
                values: new object[] { "84f17889-5ac4-4283-bc54-9013242c82ff", "AQAAAAIAAYagAAAAEM7MY3PQM5YaeEDzDOOtb9oSFpFp4UZXwE3LR8Zsvgbgk7Oig3EDgrGyN8zWQxYBjA==", "user3@bingi.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e576",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "UserName" },
                values: new object[] { "43d890c6-0321-444c-8c8d-d8d146ead484", null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e577",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "UserName" },
                values: new object[] { "761af644-fb1e-4e7e-9062-65697cde948f", null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e578",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "UserName" },
                values: new object[] { "f6c8c0e8-d726-4c3c-8da6-4646b71e8acc", null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e579",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "UserName" },
                values: new object[] { "39ab3415-124f-4863-8990-5ac0d32e7c97", null, null });
        }
    }
}
