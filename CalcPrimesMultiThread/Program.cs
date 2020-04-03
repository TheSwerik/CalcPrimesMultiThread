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

        private static void Main(string[] args)
        {
            // INFO: Don't forget to set the working directory to the "out" folder in the run-config!"
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                var max = BigInteger.Parse(Console.ReadLine() ?? throw new NullReferenceException());
                if (max <= MaxSieveValue)
                    ThreadMaster.StartSieve(max);
                else
                    ThreadMaster.Start();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("Please enter a real number.");
            }

            // TODO create TaskMaster
        }
    }
}