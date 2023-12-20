using Labb3_GymnasiumSchoolDB.Models;

namespace Labb3_GymnasiumSchoolDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=GymnasiumSchool;Trusted_Connection=True;MultipleActiveResultSets=true";

            using (GymnasiumSchoolContext dbContext = new GymnasiumSchoolContext())
            {
                while (true)
                {
                    Console.WriteLine("** Welcome **\n" +
                        "Select a function:");
                    Console.WriteLine("[1] Employees ");
                    Console.WriteLine("[2] Students");
                    Console.WriteLine("[3] Exit");
                    int choice;
                    while (!int.TryParse(Console.ReadLine(), out choice))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number in the menu");
                    }
                    Console.Clear();
                    switch (choice)
                    {
                        case 1:
                            EmployeeMethods.Employees(dbContext);
                            break;
                        case 2:
                            StudentMethods.Students(dbContext);
                            break;
                        case 3:
                            Console.WriteLine("The program ends.\nWelcome back!");
                            Environment.Exit(0);
                            break;
                    }

                }
            }
        }
    }
}