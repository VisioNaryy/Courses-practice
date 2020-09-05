using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLocks
{
    class Program
    {
        static ReaderWriterLockSlim padlock = new ReaderWriterLockSlim();
        //to support recursion
        //static ReaderWriterLockSlim padlock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        static Random random = new Random();
        static void Main(string[] args)
        {
            int x = 0;

            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    padlock.EnterReadLock();
                    
                    Console.WriteLine($"Entered read lock, x = {x}.");
                    Thread.Sleep(5000);

                    padlock.ExitReadLock();
                    Console.WriteLine($"Exited read lock, x = {x}.");
                }));
            }
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    Console.WriteLine(e);
                    return true;
                });
            }
            while (true)
            {
                Console.ReadKey();
                padlock.EnterWriteLock();
                Console.WriteLine($"Write lock acquired");
                int newValue = random.Next(10);
                x = newValue;
                Console.WriteLine($"Set x = {x}");
                padlock.ExitWriteLock();
                Console.WriteLine("Write lock released");
            }





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
        public void Transfer(BankAccount where, int amount)
        {
            Balance -= amount;
            where.Balance += amount;
        }
    }
}
