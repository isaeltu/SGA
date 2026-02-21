using SGA.Domain.ValueObjects.Users;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var Emp1 = new EmployeeCode();
            Console.WriteLine(Emp1.Value);
        }
    }
}
