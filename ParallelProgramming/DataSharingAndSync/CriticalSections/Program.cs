using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CriticalSections
{
    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new List<Task>();
            BankAccount ba = new BankAccount();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        ba.Deposit(100);
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        ba.Witdraw(100);
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            //methods in BankAccount are not atomic

            Console.WriteLine($"Final balance is {ba.Balance}.");

        }
    }

    public class BankAccount
    {
        public object padlock = new object();
        public int Balance { get; private set; }
        public void Deposit(int amount)
        {
            //now only one thread can lock padlock
            lock (padlock)
            {
                Balance += amount;
            }
        }
        public void Witdraw(int amount)
        {
            lock (padlock)
            {
                Balance -= amount;
            }
        }
    }
}
