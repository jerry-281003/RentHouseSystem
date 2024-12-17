using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentHouseSystem.Migrations
{
    /// <inheritdoc />
    public partial class updateComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcceptableVehicles",
                table: "FacilitiesHouse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CloseTime",
                table: "FacilitiesHouse",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "ReplineId",
                table: "Comment",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptableVehicles",
                table: "FacilitiesHouse");

            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "FacilitiesHouse");

            migrationBuilder.DropColumn(
                name: "ReplineId",
                table: "Comment");
        }
    }
}
