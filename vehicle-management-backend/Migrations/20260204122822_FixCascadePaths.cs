using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vehicle_management_backend.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "ModelType",
            //    table: "Models");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModelType",
                table: "Models",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
