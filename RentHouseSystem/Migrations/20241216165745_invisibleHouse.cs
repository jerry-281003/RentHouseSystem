using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentHouseSystem.Migrations
{
    /// <inheritdoc />
    public partial class invisibleHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Invisible",
                table: "House",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Invisible",
                table: "House");
        }
    }
}
