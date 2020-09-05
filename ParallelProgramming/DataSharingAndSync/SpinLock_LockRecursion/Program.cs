using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpinLock_LockRecursion
{
    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new List<Task>();
            BankAccount ba = new BankAccount();

            SpinLock sl = new SpinLock();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            ba.Deposit(100);
                        }
                        finally
                        {
                            if (lockTaken)
                                sl.Exit();
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            ba.Witdraw(100);
                        }
                        finally
                        {
                            if (lockTaken)
                                sl.Exit();
                        }
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
            balance += amount;

        }
        public void Witdraw(int amount)
        {
            balance -= amount;
        }
    }
}
