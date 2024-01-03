using Labb3_GymnasiumSchoolDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_GymnasiumSchoolDB
{
    internal class StudentMethods
    {
        public static void Students(GymnasiumSchoolContext dbContext)
        {
            Console.WriteLine("** Students **");
            Console.WriteLine("[1] View all students");
            Console.WriteLine("[2] View students by class");
            Console.WriteLine("[3] Add new student");
            Console.WriteLine("[4] Grades");
            Console.WriteLine("[5] Return to menu");

            string studentChoice = Console.ReadLine();

            Console.Clear();
            switch (studentChoice)
            {
                case "1":
                    ViewAllStudents(dbContext);
                    break;
                case "2":
                    ViewStudentsByClass(dbContext);
                    break;
                case "3":
                    AddNewStudent(dbContext);
                    break;
                case "4":
                    StudentGrade.Grades(dbContext);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Wrong input, try again");
                    break;
            }
            Console.Clear();
        }
        public static void ViewAllStudents(GymnasiumSchoolContext dbContext)
        {
            Console.WriteLine("* Select sorting options *");
            Console.WriteLine("[1] Sort by first name (ascending)");
            Console.WriteLine("[2] Sort by first name (descending)");
            Console.WriteLine("[3] Sort by last name (ascending)");
            Console.WriteLine("[4] Sort by last name (descending)");

            int sortChoice;
            while (!int.TryParse(Console.ReadLine(), out sortChoice) || sortChoice < 1 || sortChoice > 4)
            {
                Console.WriteLine("Invalid input. Please enter a valid number (1-4):");
            }

            Console.Clear();
            // Define the IQueryable variable for sorting students
            IQueryable<Student> students;

            switch (sortChoice)
            {
                case 1:
                    students = dbContext.Students.OrderBy(s => s.FirstName);
                    break;
                case 2:
                    students = dbContext.Students.OrderByDescending(s => s.FirstName);
                    break;
                case 3:
                    students = dbContext.Students.OrderBy(s => s.LastName);
                    break;
                case 4:
                    students = dbContext.Students.OrderByDescending(s => s.LastName);
                    break;
                default:
                    Console.WriteLine("Invalid sorting option. Enter sorting option (1-4)");
                    return;
            }
            Console.Clear();
            Console.WriteLine("* All students *");
            // Display all students based on the sorting choice
            foreach (var student in students)
            {
                Console.WriteLine($"Student Id {student.StudentId}: {student.FirstName} {student.LastName}. Class: {student.Class}");
            }
            Console.WriteLine("Press ENTER to return..."); 
            Console.ReadLine();
            Console.Clear();
            Students(dbContext);
        }
        public static void ViewStudentsByClass(GymnasiumSchoolContext dbContext)
        {
            Console.WriteLine("* Available classes *");
            Console.WriteLine("EK17 - EK18 - NA18 - SA18");

            {
                string classChoice = "";
                bool isValidClass = false;

                while (!isValidClass)
                {
                    Console.WriteLine("* Enter a class to view students *");
                    classChoice = Console.ReadLine();

                    string[] validClassCodes = { "EK17", "EK18", "NA18", "SA18"};

                    isValidClass = validClassCodes.Contains(classChoice);

                    if (!isValidClass)
                    {
                        Console.WriteLine("Invalid class. Enter a class available in the list.");
                    }
                }
                Console.Clear();

                try
                {
                    // Retrieve students in the selected class from the database
                    var studentsInClass = dbContext.Students
                        .Where(predicate: s => s.Class == classChoice)
                        .ToList();
                    if (studentsInClass.Any())
                    {
                        // Display information for each student in the selected class
                        Console.WriteLine($"* Students in Class {classChoice} *");
                        foreach (var student in studentsInClass)
                        {
                            Console.WriteLine($"Student Id {student.StudentId}: {student.FirstName} {student.LastName}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No students found in Class {classChoice}.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occured: {ex.Message}");
                }

                Console.WriteLine("Press ENTER to return...");
                Console.ReadLine();
                Console.Clear();
                Students(dbContext);
            }
        }
        public static void AddNewStudent(GymnasiumSchoolContext dbContext)
        {
            Console.WriteLine("* Enter Student information *");

            Console.Write("First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last Name: ");
            string lastName = Console.ReadLine();

            int personalNumber;
            while (true)
            {
                Console.WriteLine("Enter personalnumber (YYYYMMDD):");
                string personalNumberInput = Console.ReadLine();

                if (personalNumberInput.Length == 8 && int.TryParse(personalNumberInput, out personalNumber))
                {
                    break; 
                }
                else
                {
                    Console.WriteLine("Invalid personalnumber. Please enter 8 digits (YYYYMMDD)");
                }
            }

            bool isValidClass = false;
            string studentClass = "";

            while (!isValidClass)
            {
                Console.Write("Class (EK17, EK18, NA18 or SA18): ");
                studentClass = Console.ReadLine();

                string[] validClassCodes = { "EK17", "EK18", "NA18", "SA18" };
                isValidClass = validClassCodes.Contains(studentClass);

                if (!isValidClass)
                {
                    Console.WriteLine("Invalid class. Enter a class available in the list.");
                }
            }
            Console.Clear();

            var newStudent = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Class = studentClass,
                PersonalNumber = personalNumber
            };

            try
            {
                dbContext.Students.Add(newStudent);
                dbContext.SaveChanges();

                Console.WriteLine("New student added.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
            Console.WriteLine("Press ENTER to return...");
            Console.ReadLine();
            Console.Clear();
            Students(dbContext);

        }
    }
}

