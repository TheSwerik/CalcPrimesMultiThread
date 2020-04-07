using System;
using System.Globalization;
using System.Numerics;
using System.Threading;
using CalcPrimesMultiThread.Prime.Task;
using CalcPrimesMultiThread.Prime.Thread;

namespace CalcPrimesMultiThread
{
    internal static class Program
    {
        private const int MaxSieveValue = int.MaxValue - 57;
        private const bool Task = false;

        private static void Main()
        {
            // INFO: Don't forget to set the working directory to the "out" folder in the run-config!"
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            try
            {
                Console.WriteLine("Until what number do you want to calculate?");
                var max = BigInteger.Parse(Console.ReadLine() ?? "999999999999999999999999999999");

                Console.WriteLine("Do you want to override any existing file?");
                var shouldOverride = false;
                var input = Console.ReadLine() ?? throw new InvalidInputException();
                if (input.ToLowerInvariant().Contains("y")) shouldOverride = true;
                else if (input.ToLowerInvariant().Equals("true")) shouldOverride = true;

                Console.WriteLine("\nStarting...");

                if (shouldOverride)
                {
                    ThreadMaster.Max = max;
                    TaskMaster.Max = max;
                    TaskMaster.StartSieve(max <= MaxSieveValue ? max : MaxSieveValue);
                    if (Task && max > MaxSieveValue) TaskMaster.Start();
                    if (Task) return;
                    Console.WriteLine("How Many Threads do you want to use?");
                    input = Console.ReadLine();
                    var threadCount = string.IsNullOrEmpty(input) ? -1 : int.Parse(input);
                    ThreadMaster.Start(threadCount);
                }
                else
                {
                    ThreadMaster.Max = max;
                    TaskMaster.Max = max;
                    if (max <= MaxSieveValue)
                    {
                        TaskMaster.StartSieve(max);
                    }
                    else if (Task)
                    {
                        TaskMaster.Start();
                    }
                    else
                    {
                        Console.WriteLine("How Many Threads do you want to use?");
                        input = Console.ReadLine();
                        var threadCount = string.IsNullOrEmpty(input) ? -1 : int.Parse(input);
                        ThreadMaster.Start(threadCount);
                    }
                }
            }
            catch (InvalidInputException)
            {
                Console.WriteLine("Please enter a real number.");
            }
            finally
            {
                FileHelper.Dispose();
            }
        }
    }
}