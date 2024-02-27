using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace authentication.Migrations
{
    /// <inheritdoc />
    public partial class initialdbmigratenew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Employees",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "EmpName",
                table: "Employees",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "EmpEmail",
                table: "Employees",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "EmpAddress",
                table: "Employees",
                newName: "Address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Employees",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Employees",
                newName: "EmpName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Employees",
                newName: "EmpEmail");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Employees",
                newName: "EmpAddress");
        }
    }
}
