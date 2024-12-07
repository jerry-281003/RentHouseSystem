using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentHouseSystem.Migrations
{
    /// <inheritdoc />
    public partial class addressHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "House",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "House",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ward",
                table: "House",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "District",
                table: "House");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "House");

            migrationBuilder.DropColumn(
                name: "Ward",
                table: "House");
        }
    }
}
