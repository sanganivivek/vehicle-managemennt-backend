using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vehicle_management_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDealerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MobileNo",
                table: "Dealers",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "EmailId",
                table: "Dealers",
                newName: "GSTNo");

            migrationBuilder.AddColumn<string>(
                name: "ContactNo",
                table: "Dealers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Dealers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Dealers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Dealers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactNo",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Dealers");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Dealers",
                newName: "MobileNo");

            migrationBuilder.RenameColumn(
                name: "GSTNo",
                table: "Dealers",
                newName: "EmailId");
        }
    }
}
