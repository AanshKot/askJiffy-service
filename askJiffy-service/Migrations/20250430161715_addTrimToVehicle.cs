using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace askJiffy_service.Migrations
{
    /// <inheritdoc />
    public partial class addTrimToVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Trim",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Trim",
                table: "Vehicles");
        }
    }
}
