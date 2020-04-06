using System;
using System.Globalization;
using System.Numerics;
using System.Threading;
using CalcPrimesMultiThread.Prime;

namespace CalcPrimesMultiThread
{
    internal static class Program
    {
        private const int MaxSieveValue = int.MaxValue - 57;
        private const bool Task = true;

        private static void Main()
        {
            // INFO: Don't forget to set the working directory to the "out" folder in the run-config!"
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            try
            {
                Console.WriteLine("Until what number do you want to calculate?\n");
                var max = BigInteger.Parse(Console.ReadLine() ?? throw new NullReferenceException());

                Console.WriteLine("Do you want to override any existing file?");
                var shouldOverride = false;
                var input = Console.ReadLine() ?? throw new NullReferenceException();
                if (input.ToLowerInvariant().Contains("y")) shouldOverride = true;
                else if (input.ToLowerInvariant().Equals("true")) shouldOverride = true;

                Console.WriteLine("\nStarting...");

                if (shouldOverride)
                {
                    ThreadMaster.Max = MaxSieveValue;
                    TaskMaster.Max = MaxSieveValue;
                    TaskMaster.StartSieve();
                    if (max <= MaxSieveValue) return;
                    ThreadMaster.Max = max;
                    TaskMaster.Max = max;
                    if (Task) TaskMaster.Start();
                    else
                    {
                        Console.WriteLine("How Many Threads do you want to use?");
                        input = Console.ReadLine();
                        var threadCount = string.IsNullOrEmpty(input) ? -1 : int.Parse(input);
                        ThreadMaster.Start(threadCount);
                    }
                }
                else
                {
                    ThreadMaster.Max = max;
                    TaskMaster.Max = max;
                    if (max <= MaxSieveValue) TaskMaster.StartSieve();
                    else if (Task) TaskMaster.Start();
                    else
                    {
                        Console.WriteLine("How Many Threads do you want to use?");
                        input = Console.ReadLine();
                        var threadCount = string.IsNullOrEmpty(input) ? -1 : int.Parse(input);
                        ThreadMaster.Start(threadCount);
                    }
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Please enter a real number.");
            }

            // TODO create TaskMaster
        }
    }
}