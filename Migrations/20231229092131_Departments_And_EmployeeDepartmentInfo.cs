using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Labb3_GymnasiumSchoolDB.Migrations
{
    public partial class Departments_And_EmployeeDepartmentInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmploymentYear",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmploymentYear",
                table: "Employees");
        }
    }
}
