using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class AddNameColoumnToTheWalksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("19d9c703-3006-46d4-ba42-e60abbf02f39"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("3709febd-2a69-4660-a647-683622e4d455"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("6954edef-a248-4112-88d3-0b4d6229427f"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Walks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("483ac3ee-7a54-4878-8684-002a9b4b9dcf"), "Hard" },
                    { new Guid("779e48fa-9615-40de-b3e5-ec29de1a8619"), "Medium" },
                    { new Guid("ae928d54-8f51-49bd-b3e1-b72b6ee9b4ba"), "Easy" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("483ac3ee-7a54-4878-8684-002a9b4b9dcf"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("779e48fa-9615-40de-b3e5-ec29de1a8619"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("ae928d54-8f51-49bd-b3e1-b72b6ee9b4ba"));

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Walks");

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("19d9c703-3006-46d4-ba42-e60abbf02f39"), "Easy" },
                    { new Guid("3709febd-2a69-4660-a647-683622e4d455"), "Medium" },
                    { new Guid("6954edef-a248-4112-88d3-0b4d6229427f"), "Hard" }
                });
        }
    }
}
