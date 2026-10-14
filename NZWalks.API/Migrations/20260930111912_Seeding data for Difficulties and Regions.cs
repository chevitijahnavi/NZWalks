using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingdataforDifficultiesandRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("43787b3a-7fde-44d0-8ef4-4d7f465c346f"), "Hard" },
                    { new Guid("77ef4588-cd0d-4101-afd5-a46f2456d6c8"), "Medium" },
                    { new Guid("e1ba8fab-b57e-43d1-94d7-365cff9a25d8"), "Easy" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("113b638e-bb66-43fa-96bc-64263e64d8fa"), "STL", "SouthLand", "https://share.google/NGQhxYhv3iscmp3Ok" },
                    { new Guid("17537ad4-edad-429c-a0ec-836e7fbd29ed"), "AKL", "Auckland", "https://share.google/NGQhxYhv3iscmp3Ok" },
                    { new Guid("d520dd3e-d8fa-4315-9af9-e0ef2c441e54"), "NSN", "Nelson", "https://share.google/NGQhxYhv3iscmp3Ok" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("43787b3a-7fde-44d0-8ef4-4d7f465c346f"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("77ef4588-cd0d-4101-afd5-a46f2456d6c8"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("e1ba8fab-b57e-43d1-94d7-365cff9a25d8"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("113b638e-bb66-43fa-96bc-64263e64d8fa"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("17537ad4-edad-429c-a0ec-836e7fbd29ed"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("d520dd3e-d8fa-4315-9af9-e0ef2c441e54"));
        }
    }
}
