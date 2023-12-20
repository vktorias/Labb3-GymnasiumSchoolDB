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
            Console.WriteLine("[4] Return to menu");

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
            using (SqlConnection connection = new SqlConnection(dbContext.Database.GetDbConnection().ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Employees";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("** All employees ** ");
                        while (reader.Read())
                        {
                            Console.WriteLine($"Employee Id = {reader["EmployeeId"]}, Role = {reader["Role"]}: " +
                                $"{reader["FirstName"]} {reader["LastName"]}");
                        }
                        Console.WriteLine("Press ENTER to return");
                        Console.ReadLine();
                        Console.Clear();
                        Employees(dbContext);
                    }

                }
            }
        }
        public static void ViewAllTeachers(GymnasiumSchoolContext dbContext)
        {
            using (SqlConnection connection = new SqlConnection(dbContext.Database.GetDbConnection().ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Employees WHERE Role = @Role";

                Console.Clear();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Role", "Teacher");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("** All teachers **");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["FirstName"]} {reader["LastName"]}");

                        }
                        Console.WriteLine("Press ENTER to return");
                        Console.ReadLine();
                        Console.Clear();
                        Employees(dbContext);
                    }
                }
            }
        }
        public static void AddNewEmployee(GymnasiumSchoolContext dbContext)
        {
            Console.WriteLine("* Enter employee information: *\n");

            Console.Write("First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last Name: ");
            string lastName = Console.ReadLine();

            bool isValidRole = false;
            string employeeRole = "";

            while (!isValidRole)
            {
                Console.Write("Role (Pricipal, Teacher or Admin): ");
                employeeRole = Console.ReadLine();

                string[] validRoleCodes = { "Principal", "Teacher", "Admin" };
                isValidRole = validRoleCodes.Contains(employeeRole);

                if (!isValidRole)
                {
                    Console.WriteLine("Invalid role. Enter a role available in the list.");
                }
            }
            Console.Clear();

            var newEmployee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Role = employeeRole
            };

            try
            {
                dbContext.Employees.Add(newEmployee);
                dbContext.SaveChanges();

                Console.WriteLine($"New {employeeRole} added!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
            Console.WriteLine("Press ENTER to return.");
            Console.ReadLine();
            Console.Clear();
            Employees(dbContext);

        }
    }
}