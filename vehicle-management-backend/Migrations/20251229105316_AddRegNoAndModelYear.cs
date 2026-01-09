using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace vehicle_management_backend.Migrations
{
    public partial class AddRegNoAndModelYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Vehicles",
                newName: "VehicleId");
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);
            migrationBuilder.AddColumn<int>(
                name: "ModelYear",
                table: "Vehicles",
                type: "int",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "RegNo",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Vehicles");
            migrationBuilder.DropColumn(
                name: "ModelYear",
                table: "Vehicles");
            migrationBuilder.DropColumn(
                name: "RegNo",
                table: "Vehicles");
            migrationBuilder.RenameColumn(
                name: "VehicleId",
                table: "Vehicles",
                newName: "Id");
        }
    }
}