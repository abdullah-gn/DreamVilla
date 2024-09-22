using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamVilla_VillaApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLocalPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 9, 21, 16, 44, 39, 25, DateTimeKind.Local).AddTicks(65));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 9, 21, 16, 44, 39, 25, DateTimeKind.Local).AddTicks(107));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 9, 21, 16, 44, 39, 25, DateTimeKind.Local).AddTicks(110));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 9, 21, 16, 44, 39, 25, DateTimeKind.Local).AddTicks(112));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 9, 21, 16, 44, 39, 25, DateTimeKind.Local).AddTicks(114));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 2, 23, 16, 22, 51, DateTimeKind.Local).AddTicks(5583));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 2, 23, 16, 22, 51, DateTimeKind.Local).AddTicks(5626));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 2, 23, 16, 22, 51, DateTimeKind.Local).AddTicks(5629));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 2, 23, 16, 22, 51, DateTimeKind.Local).AddTicks(5631));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 2, 23, 16, 22, 51, DateTimeKind.Local).AddTicks(5634));
        }
    }
}
