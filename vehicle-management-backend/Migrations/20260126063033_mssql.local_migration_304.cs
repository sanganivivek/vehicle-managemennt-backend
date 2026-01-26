using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vehicle_management_backend.Migrations
{
    /// <inheritdoc />
    public partial class mssqllocal_migration_304 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrandCode",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Brands",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandCode",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Brands");
        }
    }
}
