using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LacontesLibrary.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Checkouts",
                columns: new[] { "Id", "CheckoutDate", "MaterialId", "PatronId", "ReturnDate" },
                values: new object[,]
                {
                    { 7, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, null },
                    { 8, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, null },
                    { 9, new DateTime(2024, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 3, null },
                    { 10, new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 4, null },
                    { 11, new DateTime(2024, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 5, null }
                });

            migrationBuilder.InsertData(
                table: "Patrons",
                columns: new[] { "Id", "Address", "Email", "FirstName", "IsActive", "LastName" },
                values: new object[,]
                {
                    { 7, "876 Birch Ave", "laura.taylor@example.com", "Laura", false, "Taylor" },
                    { 8, "982 Spruce Blvd", "brian.adams@example.com", "Brian", false, "Adams" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Checkouts",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Checkouts",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Checkouts",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Checkouts",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Checkouts",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Patrons",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Patrons",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
