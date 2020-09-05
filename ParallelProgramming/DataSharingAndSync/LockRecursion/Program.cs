using System;
using System.Threading;

namespace LockRecursion
{
    class Program
    {
        static void Main(string[] args)
        {
            LockRecursion(5);
        }

        static SpinLock sl = new SpinLock();
        public static void LockRecursion(int x)
        {
            bool lockTaken = false;
            try
            {
                sl.Enter(ref lockTaken);
            }
            catch (Exception e)
            {

                Console.WriteLine($"Exception: {e.Message}");
            }
            finally
            {
                if (lockTaken)
                {
                    Console.WriteLine($"Took a lock, x = {x}");
                    LockRecursion(x - 1);
                    sl.Exit();
                }
                else
                {
                    Console.WriteLine($"Failed to take a lock, x = {x}");
                }
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
    }
}
