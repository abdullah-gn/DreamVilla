using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamVilla_VillaApi.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 12, 25, 20, 11, 30, 637, DateTimeKind.Local).AddTicks(7867));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 12, 25, 20, 11, 30, 637, DateTimeKind.Local).AddTicks(7910));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 12, 25, 20, 11, 30, 637, DateTimeKind.Local).AddTicks(7912));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 12, 25, 20, 11, 30, 637, DateTimeKind.Local).AddTicks(7914));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 12, 25, 20, 11, 30, 637, DateTimeKind.Local).AddTicks(7916));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 12, 25, 20, 10, 41, 774, DateTimeKind.Local).AddTicks(5969));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 12, 25, 20, 10, 41, 774, DateTimeKind.Local).AddTicks(6009));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 12, 25, 20, 10, 41, 774, DateTimeKind.Local).AddTicks(6012));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 12, 25, 20, 10, 41, 774, DateTimeKind.Local).AddTicks(6014));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 12, 25, 20, 10, 41, 774, DateTimeKind.Local).AddTicks(6016));
        }
    }
}
