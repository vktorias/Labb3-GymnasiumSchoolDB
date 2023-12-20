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

namespace Labb3_GymnasiumSchoolDB
{
    internal class StudentGrade
    {
        public static void ViewGradesLatestMonth(GymnasiumSchoolContext dbContext)
        {
            try
            {
                DateTime latestMonth = DateTime.Now.AddMonths (-1);
                DateTime endDate = DateTime.Now;
                Console.WriteLine($"* Grades for the latest month: ({latestMonth:dd MMMM} " +
                    $"- {endDate:dd MMMM}) *\n");


                var recentGrades = dbContext.Enrollments
                    .Where(e => e.GradeDate >= latestMonth)
                    .Include(e => e.Student)
                    .Include(e => e.Course)
                    .ToList();

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

            Console.WriteLine("Press ENTER to return.");
            Console.ReadLine();
            Console.Clear();
            StudentMethods.Students(dbContext);
        }
        public static void ViewCourseGradesStatistics(GymnasiumSchoolContext dbContext)
        {
            try
            {
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
    }
}

