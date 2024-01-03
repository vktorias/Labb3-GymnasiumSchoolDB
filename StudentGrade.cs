using Labb3_GymnasiumSchoolDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Design;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Data;

namespace Labb3_GymnasiumSchoolDB
{
    internal class StudentGrade
    {
        public static void Grades(GymnasiumSchoolContext dbContext)
        {
            Console.WriteLine("** Grades **");
            Console.WriteLine("[1] View all grades");
            Console.WriteLine("[2] Add grade for student");
            Console.WriteLine("[3] View grades recent month");
            Console.WriteLine("[4] View grades statistics");
            Console.WriteLine("[5] Return to menu");

            string gradeChoice = Console.ReadLine();
            Console.Clear();

            switch (gradeChoice)
            {
                case "1":
                    ViewAllGrades(dbContext);
                    break;
                case "2":
                    AddGrades(dbContext);
                    break;
                case "3":
                    ViewGradesLatestMonth(dbContext);
                    break;
                case "4":
                    ViewCourseGradesStatistics(dbContext);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Wrong input, try again");
                    break;
            }
            Console.Clear();
        }

        public static void ViewAllGrades(GymnasiumSchoolContext dbContext)
        {
            Console.WriteLine("** All grades information **");

            {
                try
                {
                    // Retrieve all grades from the database including related student, course, and teacher information
                    var allGrades = dbContext.Enrollments
                        .Include(e => e.Student)
                        .Include(e => e.Course)
                        .ThenInclude(c => c.Teacher)
                        .ToList();

                    // Check if there are any grades available
                    if (allGrades.Any())
                    {
                        Console.WriteLine("All grades:");

                        // Display information for each grade
                        foreach (var grade in allGrades)
                        {
                            Console.WriteLine($"Student: {grade.Student.FirstName}");
                            Console.WriteLine($"Course: {grade.Course.CourseName}");
                            Console.WriteLine($"Grade: {grade.Grade}");
                            Console.WriteLine($"Grade Date: {grade.GradeDate}");
                            Console.WriteLine($"Teacher: {grade.Course.Teacher.FirstName} {grade.Course.Teacher.LastName}");
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No grades available.");
                    }
                }
                // Handle any errors that may occur during database operations
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred: {ex.Message}");
                }
                Console.WriteLine("Press Enter to return...");
                Console.ReadLine();
                Console.Clear();
                Grades(dbContext);
            }

        }
        public static void ViewGradesLatestMonth(GymnasiumSchoolContext dbContext)
        {
            try
            {
                // Calculate the date range for the latest month
                DateTime latestMonth = DateTime.Now.AddMonths (-1);
                DateTime endDate = DateTime.Now;
                Console.WriteLine($"* Grades for the latest month: ({latestMonth:dd MMMM} " +
                    $"- {endDate:dd MMMM}) *\n");

                // Retrieve grades from the latest month including related student and course information
                var recentGrades = dbContext.Enrollments
                    .Where(e => e.GradeDate >= latestMonth)
                    .Include(e => e.Student)
                    .Include(e => e.Course)
                    .ToList();

                // Check if there are any grades available for the latest month
                if (recentGrades.Any())
                {

                    foreach (var grade in recentGrades)
                    {
                        Console.WriteLine($"Student: {grade.Student.FirstName} {grade.Student.LastName}");
                        Console.WriteLine($"Course: {grade.Course.CourseName}");
                        Console.WriteLine($"Grade: {grade.Grade}");
                        Console.WriteLine($"Grade Date: {grade.GradeDate}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No grades available for the latest month.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }

            Console.WriteLine("Press ENTER to return...");
            Console.ReadLine();
            Console.Clear();
            StudentMethods.Students(dbContext);
        }
        public static void ViewCourseGradesStatistics(GymnasiumSchoolContext dbContext)
        {
            try
            {   // Retrieve course statistics, including average, max, and min grades
                var courseStatistics = dbContext.Courses
                    .Select(course => new
                    {
                        CoursesName = course.CourseName,
                        AverageGrade = dbContext.Enrollments
                            .Where(enrollment => enrollment.CourseId == course.CourseId)
                            .Average(enrollment => enrollment.Grade),
                        MaxGrade = dbContext.Enrollments
                            .Where(enrollment => enrollment.CourseId == course.CourseId)
                            .Max(enrollment => enrollment.Grade),
                        MinGrade = dbContext.Enrollments
                            .Where(enrollment => enrollment.CourseId == course.CourseId)
                            .Min(enrollment => enrollment.Grade),
                    })
                    .ToList();
                // Check if there are any courses available
                if (courseStatistics.Any())
                {
                    Console.WriteLine("* Course Statistics * \n");
                    foreach (var stats in courseStatistics)
                    {
                        Console.WriteLine($"Course: {stats.CoursesName}");
                        Console.WriteLine($"Average Grade: {stats.AverageGrade}");
                        Console.WriteLine($"Max Grade: {stats.MaxGrade}");
                        Console.WriteLine($"Min Grade: {stats.MinGrade}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No courses available.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
            Console.WriteLine("Press ENTER to return.");
            Console.ReadLine();
            Console.Clear();
            StudentMethods.Students(dbContext);
        }
        public static void AddGrades(GymnasiumSchoolContext dbContext)
        {
            // Display a list of students for the user to select from
            Console.WriteLine("Select student:");
            var students = dbContext.Students.ToList();
            foreach (var student in students)
            {
                Console.WriteLine($"{student.StudentId}. {student.FirstName} {student.LastName}");
            }
            // Read and validate the selected student ID from the user
            int selectedStudentId;
            while (true)
            {
                Console.Write("Enter student ID: ");
                
                if (int.TryParse(Console.ReadLine(), out selectedStudentId) && students.Any(s=>s.StudentId == selectedStudentId))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid selection. Enter a valid Student ID in the list.");
                }
            }

            Console.WriteLine("Select a course:");
            var courses = dbContext.Courses.ToList();
            foreach (var course in courses)
            {
                Console.WriteLine($"{course.CourseId}. {course.CourseName}");
            }

            // Read and validate the selected course ID from the user
            int selectedCourseId;

            while (true)
            {
                Console.Write("Enter course ID: ");

                if (int.TryParse(Console.ReadLine(), out selectedCourseId) && courses.Any(s => s.CourseId == selectedCourseId))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid selection. Enter a valid Course ID in the list.");
                }
            }
            // Display a list of teachers for the user to select from
            Console.WriteLine("Select teacher:");
            var teachers = dbContext.Employees
                .Where(teacher => teacher.Role == "Teacher")
                .ToList();
            foreach (var teacher in teachers)
            {
                Console.WriteLine($"{teacher.EmployeeId}. {teacher.FirstName} {teacher.LastName}");
            }
            // Read and validate the selected teacher ID from the user
            int selectedTeacherId;
            while (true)
            {
                Console.Write("Enter teacher ID: ");

                if (int.TryParse(Console.ReadLine(), out selectedTeacherId) && teachers.Any(t => t.EmployeeId == selectedTeacherId))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid selection. Enter a valid Teacher ID in the list.");
                }
            }
            // Read and validate the entered grade from the user
            int grade;
            while (true)
            {
                Console.Write("Enter Grade (1-5): ");
                if(int.TryParse(Console.ReadLine(), out grade) && grade >= 1 && grade <= 5)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid selection (1-5)");
                }
            }
            // Create a new grade entry and save it to the database
            var newGrade = new Enrollment
            {
                StudentId = selectedStudentId,
                CourseId = selectedCourseId,
                Grade = grade,
                GradeDate = DateTime.Now
            };

                dbContext.Enrollments.Add(newGrade);
                dbContext.SaveChanges();
                // Inform the user that the grade has been added and saved
                Console.WriteLine("The grade has been added and saved.");

            Console.WriteLine("Press ENTER to return...");
            Console.ReadLine();
            Console.Clear();
            StudentMethods.Students(dbContext);
        }
    }
}