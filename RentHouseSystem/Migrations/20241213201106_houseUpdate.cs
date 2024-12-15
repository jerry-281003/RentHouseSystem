using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentHouseSystem.Migrations
{
    /// <inheritdoc />
    public partial class houseUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcceptableVehicles",
                table: "House",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CloseTime",
                table: "House",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "FacilitiesHouse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InventoryEquipment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UtilityCost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AirConditioning = table.Column<bool>(type: "bit", nullable: false),
                    MajorAppliances = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfBedroom = table.Column<int>(type: "int", nullable: false),
                    ToiletAndBathroom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pet = table.Column<bool>(type: "bit", nullable: false),
                    DepositRequired = table.Column<int>(type: "int", nullable: false),
                    ownerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilitiesHouse", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacilitiesHouse");

            migrationBuilder.DropColumn(
                name: "AcceptableVehicles",
                table: "House");

            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "House");
        }
    }
}
