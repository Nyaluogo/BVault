using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bingi_Storage.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserSeedMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a18be9c0-aa65-4af8-bd17-00bd9344e575", null, "Admin", "ADMIN" },
                    { "ad376a8f-9eab-4bb9-9fca-30b01540f445", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "DateOfLastLogin", "DateOfRegistration", "Discriminator", "Email", "EmailConfirmed", "FirstName", "IsAdmin", "IsEmailVerified", "IsPhoneVerified", "IsPublisher", "IsSuperAdmin", "LockoutEnabled", "LockoutEnd", "MyAccStatus", "MyKycStatus", "NormalizedEmail", "NormalizedUserName", "OtherName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "a18be9c0-aa65-4af8-bd17-00bd9344e576", 0, "2f436326-8d6f-4453-94f3-4bae951ce6f1", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AppUser", "admin@bingi.com", true, null, true, false, false, false, true, false, null, 0, 3, "ADMIN@BINGI.COM", "ADMIN@BINGI.COM", null, null, null, false, "a18be9c0-aa65-4af8-bd17-00bd9344e576", false, null },
                    { "a18be9c0-aa65-4af8-bd17-00bd9344e577", 0, "4e42e078-90d1-40cf-82d8-47d43741bf27", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AppUser", "user1@bingi.com", true, null, false, false, false, false, false, false, null, 0, 3, "USER1@BINGI.COM", "USER1@BINGI.COM", null, null, null, false, "a18be9c0-aa65-4af8-bd17-00bd9344e577", false, null },
                    { "a18be9c0-aa65-4af8-bd17-00bd9344e578", 0, "1fffef40-8f24-4bde-8369-3b0629a146aa", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AppUser", "user2@bingi.com", true, null, false, false, false, false, false, false, null, 0, 3, "USER2@BINGI.COM", "USER2@BINGI.COM", null, null, null, false, "a18be9c0-aa65-4af8-bd17-00bd9344e578", false, null },
                    { "a18be9c0-aa65-4af8-bd17-00bd9344e579", 0, "786ea194-191d-4958-8de3-6f5932ee7695", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AppUser", "user3@bingi.com", true, null, false, false, false, false, false, false, null, 0, 3, "USER3@BINGI.COM", "USER3@BINGI.COM", null, null, null, false, "a18be9c0-aa65-4af8-bd17-00bd9344e579", false, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "a18be9c0-aa65-4af8-bd17-00bd9344e575", "a18be9c0-aa65-4af8-bd17-00bd9344e576" },
                    { "ad376a8f-9eab-4bb9-9fca-30b01540f445", "a18be9c0-aa65-4af8-bd17-00bd9344e577" },
                    { "ad376a8f-9eab-4bb9-9fca-30b01540f445", "a18be9c0-aa65-4af8-bd17-00bd9344e578" },
                    { "ad376a8f-9eab-4bb9-9fca-30b01540f445", "a18be9c0-aa65-4af8-bd17-00bd9344e579" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a18be9c0-aa65-4af8-bd17-00bd9344e575", "a18be9c0-aa65-4af8-bd17-00bd9344e576" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ad376a8f-9eab-4bb9-9fca-30b01540f445", "a18be9c0-aa65-4af8-bd17-00bd9344e577" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ad376a8f-9eab-4bb9-9fca-30b01540f445", "a18be9c0-aa65-4af8-bd17-00bd9344e578" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ad376a8f-9eab-4bb9-9fca-30b01540f445", "a18be9c0-aa65-4af8-bd17-00bd9344e579" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad376a8f-9eab-4bb9-9fca-30b01540f445");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e576");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e577");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e578");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e579");
        }
    }
}
