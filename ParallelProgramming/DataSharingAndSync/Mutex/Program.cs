using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Mutex_
{
    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new List<Task>();
            Mutex mutex = new Mutex();
            Mutex mutex2 = new Mutex();

            var ba = new BankAccount();
            var ba2 = new BankAccount();


            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool haveLock = mutex.WaitOne();
                        try
                        {
                            ba.Deposit(1);
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine($"{ex.Message}");
                        }
                        finally
                        {
                            if (haveLock)
                                mutex.ReleaseMutex();
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool haveLock = mutex2.WaitOne();
                        try
                        {
                            ba2.Deposit(1);
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine($"{ex.Message}");
                        }
                        finally
                        {
                            if (haveLock)
                                mutex2.ReleaseMutex();
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool haveLock = Mutex.WaitAll(new[] { mutex, mutex2 });
                        try
                        {
                            ba.Transfer(ba2, 1);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex.ReleaseMutex();
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());


            Console.WriteLine($"Final balance of ba is {ba.Balance}.");
            Console.WriteLine($"Final balance of ba2 is {ba2.Balance}.");
        }
    }

    public class BankAccount
    {
        private int balance;

        public int Balance { get => balance; private set => balance = value; }
        public void Deposit(int amount)
        {
            //now only one thread can lock padlock
            balance += amount;

        }
        public void Witdraw(int amount)
        {
            balance -= amount;
        }
        public void Transfer(BankAccount where, int amount)
        {
            Balance -= amount;
            where.Balance += amount;
        }
    }
}
