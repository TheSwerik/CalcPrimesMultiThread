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
                Console.WriteLine("Do you want to override any existing file?");
                var shouldOverride = false;
                var boolString = Console.ReadLine() ?? throw new NullReferenceException();
                if (boolString.ToLowerInvariant().Equals("y")) shouldOverride = true;
                else if (boolString.ToLowerInvariant().Equals("yes")) shouldOverride = true;
                else if (boolString.ToLowerInvariant().Equals("true")) shouldOverride = true;
                else if (boolString.ToLowerInvariant().Equals("n")) shouldOverride = false;
                else if (boolString.ToLowerInvariant().Equals("no")) shouldOverride = false;
                else if (boolString.ToLowerInvariant().Equals("false")) shouldOverride = false;
                if (shouldOverride)
                {
                    ThreadMaster.Max = MaxSieveValue;
                    ThreadMaster.StartSieve();
                    ThreadMaster.Max = max;
                    ThreadMaster.Start();
                }
                else
                {
                    ThreadMaster.Max = max;
                    if (max <= MaxSieveValue)
                        ThreadMaster.StartSieve();
                    else
                        ThreadMaster.Start();
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