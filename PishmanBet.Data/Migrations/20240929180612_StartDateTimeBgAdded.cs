using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PishmanBet.Data.Migrations
{
    public partial class StartDateTimeBgAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Matches",
                newName: "StartDateUtc");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateBg",
                table: "Matches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDateBg",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "StartDateUtc",
                table: "Matches",
                newName: "StartDate");
        }
    }
}
