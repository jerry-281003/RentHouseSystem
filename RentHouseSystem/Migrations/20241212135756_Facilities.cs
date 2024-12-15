using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentHouseSystem.Migrations
{
    /// <inheritdoc />
    public partial class Facilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facilities",
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
                    DepositRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facilities");
        }
    }
}
