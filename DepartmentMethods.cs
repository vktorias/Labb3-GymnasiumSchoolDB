using Labb3_GymnasiumSchoolDB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_GymnasiumSchoolDB
{
    public class DepartmentMethods
    {
       public static void DepartmentsInSchool(GymnasiumSchoolContext dbContext)
        {
            Console.WriteLine("** Departments **");
            Console.WriteLine("[1] View teachars in each department");
            Console.WriteLine("[2] Salary");
            Console.WriteLine("[3] Salary statistics");
            Console.WriteLine("[4] Exit");

            string departmentChoice = Console.ReadLine();

            Console.Clear();

            switch (departmentChoice)
            {
                case "1":
                    ViewTeachersInDepartments(dbContext);
                    break;
                case "2":
                    Salary(dbContext);
                    break;
                case "3":
                    SalaryStatistics(dbContext);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid selection. Enter a valid number (1-4)");
                    DepartmentsInSchool(dbContext);
                    Console.Clear();
                    break;
            }
        } 
        public static void ViewTeachersInDepartments(GymnasiumSchoolContext dbContext)
        {
            // Display information about teachers in various departments
            Console.WriteLine("* Teachers who work in the various departments *");
            var departments = dbContext.Departments.ToList();

            foreach(var Departments in departments)
            {
                // Get teachers in the current department with the role "Teacher"
                var teachers = dbContext.DepartmentInfo
                    .Where(DepartmentInfo => DepartmentInfo.DepartmentId == Departments.DepartmentId &&
                    DepartmentInfo.Employee.Role == "Teacher").ToList();
                Console.WriteLine($"Department: {Departments.DepartmentName}\nTeacher: {teachers.Count}\n");
            }
            Console.WriteLine("Press ENTER to return...");
            Console.ReadLine();
            Console.Clear();
            DepartmentsInSchool(dbContext);
        }

        public static void Salary(GymnasiumSchoolContext dbContext)
        {
            Console.WriteLine("* Salaries for each department *");
            {
                // Get a list of all departments from the database
                var departments = dbContext.Departments.ToList();

                foreach (var department in departments)
                {
                    // Calculate the total salary for employees in the current department
                    var totalSalary = dbContext.DepartmentInfo
                           .Where(info => info.DepartmentId == department.DepartmentId)
                           .Sum(info => info.Salary);

                    Console.WriteLine($"Department: {department.DepartmentName}, Total Salary: {totalSalary}");

                }
                Console.WriteLine("Press enter to return.");
                Console.ReadLine();
                Console.Clear();
                DepartmentsInSchool(dbContext);
            }
        }
        public static void SalaryStatistics(GymnasiumSchoolContext dbContext)
        {
            Console.WriteLine("* Average salary for each department * ");
            var departments = dbContext.Departments.ToList();

            foreach (var department in departments)
            {
                // Get department information from the database
                var departmentInfo = dbContext.DepartmentInfo
                    .Where(info => info.DepartmentId == department.DepartmentId)
                    .ToList();
                // Calculate the average salary for employees in the current department
                var averageSalary = departmentInfo.Sum(info => info.Salary) / departmentInfo.Count();

                Console.WriteLine($"Department: {department.DepartmentName}. Average: {averageSalary} ");
            }
            Console.WriteLine("Press enter to return.");
            Console.ReadLine();
            Console.Clear();
            DepartmentsInSchool(dbContext);
        }
    }
}
