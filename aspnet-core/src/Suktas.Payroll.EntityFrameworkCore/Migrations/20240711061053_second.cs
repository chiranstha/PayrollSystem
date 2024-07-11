using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Suktas.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MunicipalityAmount",
                table: "tbl_Employee",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MunicipalityPercent",
                table: "tbl_Employee",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProvinceAmount",
                table: "tbl_Employee",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProvincePercent",
                table: "tbl_Employee",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "StateAmount",
                table: "tbl_Employee",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "StatePercent",
                table: "tbl_Employee",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MunicipalityAmount",
                table: "tbl_Employee");

            migrationBuilder.DropColumn(
                name: "MunicipalityPercent",
                table: "tbl_Employee");

            migrationBuilder.DropColumn(
                name: "ProvinceAmount",
                table: "tbl_Employee");

            migrationBuilder.DropColumn(
                name: "ProvincePercent",
                table: "tbl_Employee");

            migrationBuilder.DropColumn(
                name: "StateAmount",
                table: "tbl_Employee");

            migrationBuilder.DropColumn(
                name: "StatePercent",
                table: "tbl_Employee");
        }
    }
}
