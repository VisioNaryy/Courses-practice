using System;
using System.Collections.Generic;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee[] developers = new Employee[]
            {
                new Employee{Id=1, Name="Scott" },
                new Employee{Id=2, Name="Chriss" }
            };
            List<Employee> sales = new List<Employee>()
            {
                new Employee { Id=3, Name="Alex"}
            };


            Console.WriteLine(sales.Count());

            IEnumerator<Employee> enumerator = sales.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Name);
            }

            Func<int, int, int> add = (x, y) => x + y;
            Console.WriteLine(add(3,4));
        }
    }
}
