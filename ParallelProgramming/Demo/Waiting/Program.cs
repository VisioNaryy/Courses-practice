using System;
using System.Threading;
using System.Threading.Tasks;

namespace Waiting
{
    class Program
    {
        static void Main(string[] args)
        {
            //var cts = new CancellationTokenSource();
            //var token = cts.Token;
            //var t = new Task(() =>
            //{
            //    //Thread.Sleep(1000);
            //    //Thread.SpinWait();
            //    //SpinWait.SpinUntil(); it's more efficient if we need our thread to wait for a not long time
            //    Console.WriteLine("Press any key to disarm. You have 5 seconds.");
            //    bool cancelled = token.WaitHandle.WaitOne(5000);
            //    Console.WriteLine(cancelled? "Bomb disarmed.":"BOOM!");

            //}, token);
            //t.Start();

            //Console.ReadKey();
            //cts.Cancel();
            //
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var t = new Task(() =>
            {
                Console.WriteLine("I take 5 seconds.");
                for (int i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }
                Console.WriteLine("I'm done.");
            }, token);
            t.Start();

            Task t2 = Task.Factory.StartNew(() => Thread.Sleep(3000), token);

            Task.WaitAll(new[] { t, t2 }, 4000, token);

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }
    }
}
