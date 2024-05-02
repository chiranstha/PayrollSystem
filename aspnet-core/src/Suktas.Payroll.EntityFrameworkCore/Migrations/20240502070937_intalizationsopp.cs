using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Suktas.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class intalizationsopp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "tbl_Employee",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "tbl_Employee");
        }
    }
}
