using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InterlockedOperations
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


            Console.WriteLine($"Final balance is {ba.Balance}.");
        }
    }

    public class BankAccount
    {
        private int balance;

        public int Balance { get => balance; private set => balance = value; }
        public void Deposit(int amount)
        {
            //now only one thread can lock padlock
            Interlocked.Add(ref balance, amount);

        }
        public void Witdraw(int amount)
        {
            Interlocked.Add(ref balance, -amount);
        }
    }
}
