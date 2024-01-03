using Labb3_GymnasiumSchoolDB.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Labb3_GymnasiumSchoolDB
{
    internal class EmployeeMethods
    {
        public static void Employees(GymnasiumSchoolContext dbContext)
        {
            Console.Clear();
            Console.WriteLine("** Employees **");
            Console.WriteLine("[1] View all employees");
            Console.WriteLine("[2] View all teachers");
            Console.WriteLine("[3] Add new employee");
            Console.WriteLine("[4] Active courses");
            Console.WriteLine("[5] Return to menu");

            string choice = Console.ReadLine();

            Console.Clear();

            switch (choice)
            {
                case "1":
                    ViewAllEmployees(dbContext);
                    break;
                case "2":
                    ViewAllTeachers(dbContext);
                    break;
                case "3":
                    AddNewEmployee(dbContext);
                    break;
                case "4":
                    ActiveCourses(dbContext);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid selection. Enter a valid number (1-4)");
                    Employees(dbContext);
                    Console.Clear();
                    break;
            }
            
            
        }
        public static void ViewAllEmployees(GymnasiumSchoolContext dbContext)
        {
            // Retrieve a list of all employees from the database
            var listOfEmployees = dbContext.Employees.ToList();
            // Loop through each employee and print information
            foreach(var employee in listOfEmployees)
            {
                // Calculate the number of years an employee has worked
                int yearsWorked = DateTime.Now.Year - employee.EmploymentYear.Year;

                // Display employee information
                Console.WriteLine($"Name: {employee.FirstName} {employee.LastName}\nRole: {employee.Role}\n" +
                    $"Years worked: {yearsWorked}\n");
            }

                // Wait for the user to press ENTER before returning to the menu
                Console.WriteLine("Press ENTER to return...");
                Console.ReadLine();
                Console.Clear();
                Employees(dbContext);

        }
        public static void ViewAllTeachers(GymnasiumSchoolContext dbContext)
        {
            var listOfTeachers = dbContext.Employees.Where(t => t.Role == "Teacher");
            foreach (var teacher in listOfTeachers)
            {
                int yearsWorked = DateTime.Now.Year - teacher.EmploymentYear.Year;

                Console.WriteLine($"Name: {teacher.FirstName} {teacher.LastName}\nRole: {teacher.Role}\n" +
                    $"Years worked: {yearsWorked}\n");
            }
            
                Console.WriteLine("Press ENTER to return...");
                Console.ReadLine();
                Console.Clear();
                Employees(dbContext);
        }
        
        public static void AddNewEmployee(GymnasiumSchoolContext dbContext)
        {
            Console.WriteLine("* Enter employee information: *\n");

            // Read first name and last name from the user
            Console.Write("First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last Name: ");
            string lastName = Console.ReadLine();

            // Validate user input for role (Principal, Teacher, Admin)
            bool isValidRole = false;
            string employeeRole = "";

            while (!isValidRole)
            {
                Console.Write("Role (Pricipal, Teacher or Admin): ");
                employeeRole = Console.ReadLine();

                // Define valid roles
                string[] validRoleCodes = { "Principal", "Teacher", "Admin" };
                isValidRole = validRoleCodes.Contains(employeeRole);

                if (!isValidRole)
                {
                    Console.WriteLine("Invalid role. Enter a role available in the list.");
                }
            }

            bool isValidSubject = false;
            string teacherSubject = "";
            if (employeeRole == "Teacher")
            {
                Console.WriteLine("Which subject should the teacher teach?\n" +
                    "Programming C#, Math, English, Science, Art or Sport.");

                while (!isValidSubject)
                {
                    teacherSubject = Console.ReadLine();

                    string[] validSubjects = { "Programming C#", "Math", "English", "Science", "Art", "Sport" };
                    isValidSubject = validSubjects.Contains(teacherSubject);

                    if (!isValidSubject)
                    {
                        Console.WriteLine("Invalid subject. Enter a subject available in the list.");
                    }
                }
            }
            Console.Clear();

            // Read information about the new employee and add to the database
            var newEmployee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Role = employeeRole,
                EmploymentYear = DateTime.Now 
            };

            try
            {
                // Add the new employee to the database and save the changes
                dbContext.Employees.Add(newEmployee);
                dbContext.SaveChanges();

                Console.WriteLine($"New {employeeRole} added!");
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur during database operations
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
            Console.WriteLine("Press ENTER to return...");
            Console.ReadLine();
            Console.Clear();
            Employees(dbContext);

        }
        public static void ActiveCourses(GymnasiumSchoolContext dbContext)
        {
            Console.WriteLine("* Active Courses *");
            var listOfCourses = dbContext.Courses
                .Include(c => c.Teacher)
                .ToList();
            foreach (var course in listOfCourses)
            {
                Console.WriteLine($"Course Id {course.CourseId}\nCourse name: {course.CourseName}\n" +
                    $"Teacher: {course.Teacher.FirstName} {course.Teacher.LastName}\n");
            }

            Console.WriteLine("Press ENTER to return...");
            Console.ReadLine();
            Console.Clear();
            Employees(dbContext);
        }
    }
}