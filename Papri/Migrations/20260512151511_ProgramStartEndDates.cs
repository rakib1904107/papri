using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papri.Migrations
{
    /// <inheritdoc />
    public partial class ProgramStartEndDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Programs");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "Programs",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "Programs",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Programs");

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Programs",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
