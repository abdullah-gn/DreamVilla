using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamVilla_VillaApi.Migrations
{
    /// <inheritdoc />
    public partial class addDefaultImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 2, 20, 17, 33, 29, 570, DateTimeKind.Local).AddTicks(2357), "https://placehold.co/600x401" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 2, 20, 17, 33, 29, 570, DateTimeKind.Local).AddTicks(2391), "https://placehold.co/600x402" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 2, 20, 17, 33, 29, 570, DateTimeKind.Local).AddTicks(2395), "https://placehold.co/600x403" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 2, 20, 17, 33, 29, 570, DateTimeKind.Local).AddTicks(2397), "https://placehold.co/600x404" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 2, 20, 17, 33, 29, 570, DateTimeKind.Local).AddTicks(2400), "https://placehold.co/600x405" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 2, 10, 17, 22, 47, 728, DateTimeKind.Local).AddTicks(2451), "https://dotnetmasteryimages.com/bluevillaimages/villa3.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 2, 10, 17, 22, 47, 728, DateTimeKind.Local).AddTicks(2493), "https://dotnetmasteryimages.com/bluevillaimages/villa1.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 2, 10, 17, 22, 47, 728, DateTimeKind.Local).AddTicks(2495), "https://dotnetmasteryimages.com/bluevillaimages/villa4.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 2, 10, 17, 22, 47, 728, DateTimeKind.Local).AddTicks(2498), "https://dotnetmasteryimages.com/bluevillaimages/villa5.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2024, 2, 10, 17, 22, 47, 728, DateTimeKind.Local).AddTicks(2500), "https://dotnetmasteryimages.com/bluevillaimages/villa2.jpg" });
        }
    }
}
