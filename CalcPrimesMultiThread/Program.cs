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

        private static void Main()
        {
            // INFO: Don't forget to set the working directory to the "out" folder in the run-config!"
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                Console.WriteLine("Until what number do you want to calculate?");
                var max = BigInteger.Parse(Console.ReadLine() ?? throw new NullReferenceException());
                
                Console.WriteLine("How Many Threads do you want to use?");
                var input = Console.ReadLine();
                var threadCount = string.IsNullOrEmpty(input) ? -1 : int.Parse(input);
                
                Console.WriteLine("Do you want to override any existing file?");
                var shouldOverride = false;
                input = Console.ReadLine() ?? throw new NullReferenceException();
                if (input.ToLowerInvariant().Contains("y")) shouldOverride = true;
                else if (input.ToLowerInvariant().Equals("true")) shouldOverride = true;
                
                if (shouldOverride)
                {
                    ThreadMaster.Max = MaxSieveValue;
                    ThreadMaster.StartSieve();
                    if (max <= MaxSieveValue) return;
                    ThreadMaster.Max = max;
                    ThreadMaster.Start(threadCount);
                }
                else
                {
                    ThreadMaster.Max = max;
                    if (max <= MaxSieveValue) ThreadMaster.StartSieve();
                    else ThreadMaster.Start(threadCount);
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