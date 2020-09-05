using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cancelling
{
    class Program
    {
        static void Main(string[] args)
        {
            //var cts = new CancellationTokenSource();
            //var token = cts.Token;
            //token.Register(() => { Console.WriteLine("Cancellation token has been requested"); });

            //var t = new Task(() =>
            //   {
            //       int i = 0;
            //       while (true)
            //       {
            //           if (token.IsCancellationRequested)
            //           {
            //               throw new OperationCanceledException();
            //           }

            //           else
            //           Console.WriteLine($"{i++}");
            //       }
            //   }, token);
            //t.Start();

            //Task.Factory.StartNew(() =>
            //{
            //    token.WaitHandle.WaitOne();
            //    Console.WriteLine("Wait handle released, cancellation was requested");
            //});



            //Console.ReadKey();
            //cts.Cancel();

            var planned = new CancellationTokenSource();
            var preventative = new CancellationTokenSource();
            var emergency = new CancellationTokenSource();

            var paranoid = CancellationTokenSource.CreateLinkedTokenSource(
                planned.Token, preventative.Token, emergency.Token);

            Task.Factory.StartNew(() =>
            {
                int i = 0;
                while (true)
                {
                    paranoid.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++}\t");
                    Thread.Sleep(1000);
                }
            }, paranoid.Token);

            Console.ReadKey();
            emergency.Cancel(); //or preventative.Cancel(), or planned.Cancel()
            

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }

    }
}
