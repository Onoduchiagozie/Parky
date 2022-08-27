using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parky.Migrations
{
    /// <inheritdoc />
    public partial class AddImageToNationalPark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "NationParks",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "NationParks");
        }
    }
}
