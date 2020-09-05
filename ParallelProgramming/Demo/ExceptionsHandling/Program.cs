using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExceptionsHandling
{
    class Program
    {
        static void Main(string[] args)
        {
            // Handling all exceptions
            //var t = Task.Factory.StartNew(() =>
            //{
            //    throw new InvalidOperationException("Can't do this!") { Source = "t" };
            //});
            // var t2 = Task.Factory.StartNew(() =>
            // {
            //     throw new AccessViolationException("Can't acces this!") { Source = "t2" };
            // });

            // try
            // {
            //     Task.WaitAll(t, t2);
            // }
            // catch (AggregateException ae)
            // {
            //     foreach (var e in ae.InnerExceptions)
            //     {
            //         Console.WriteLine($"Exception {e.GetType()} from {e.Source}");
            //     }
            // }

            // Handling separate exceptions in different program areas

            try
            {
                Test();
            }
            catch (AggregateException ae)
            {
                //handling all other types of exceptions
                foreach (var e in ae.InnerExceptions)
                {
                    Console.WriteLine($"Handled elsewhere: {e.GetType()}");
                }
            }

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }
        public static void Test()
        {
            var t = Task.Factory.StartNew(() =>
            {
                throw new InvalidOperationException("Can't do this!") { Source = "t" };
            });
            var t2 = Task.Factory.StartNew(() =>
            {
                throw new AccessViolationException("Can't acces this!") { Source = "t2" };
            });

            try
            {
                Task.WaitAll(t, t2);
            }
            catch (AggregateException ae)
            {
                //we explicitly specified only InvalidOperationException exception
                ae.Handle(e =>
                {
                    if (e is InvalidOperationException)
                    {
                        Console.WriteLine("Invalid op!");
                        return true;
                    }
                    else return false;
                });
            }
        }
    }
}
