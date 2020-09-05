using System;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Task t1 = Task.Run(() => Console.WriteLine("Task.Run() execution"));
            //Task t = new Task(Write, "hello");
            //t.Start();
            //Task.Factory.StartNew(Write, 1234);
            //
            string text1 = "testing", text2 = "this";
            var task1 = new Task<int>(TextLength, text1);
            task1.Start();
            Task<int> task2 = Task.Factory.StartNew<int>(TextLength, text2);

            Console.WriteLine($"Length of {text1} is {task1.Result}");
            Console.WriteLine($"Length of {text2} is {task2.Result}");

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }

        public static void Write(object o)
        {
            for (int i = 0; i <1000; i++)
            {
                Console.Write(o);
            }
        }
        //num of chars
        public static int TextLength(object o)
        {
            Console.WriteLine($"\nTask with id {Task.CurrentId} processing object {o} ...");
            return o.ToString().Length;
        }
    }
}
