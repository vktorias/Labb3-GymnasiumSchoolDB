using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Labb3_GymnasiumSchoolDB.Migrations
{
    public partial class Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Course",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Student",
                table: "Enrollments");

            migrationBuilder.AddForeignKey(
                name: "FK_ENROLLMENT_COURSE_ID",
                table: "Enrollments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ENROLLMENT_STUDENT_ID",
                table: "Enrollments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ENROLLMENT_COURSE_ID",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ENROLLMENT_STUDENT_ID",
                table: "Enrollments");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Course",
                table: "Enrollments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Student",
                table: "Enrollments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId");
        }
    }
}
