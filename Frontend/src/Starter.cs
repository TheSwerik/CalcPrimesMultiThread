using System;
using System.Globalization;
using System.Numerics;
using System.Threading;
using CalcPrimesMultiThread;
using CalcPrimesMultiThread.Prime.Thread;
using Frontend.Master;

namespace Frontend
{
    internal static class Starter
    {
        private const int MaxSieveValue = int.MaxValue - 57;

        internal static void Start(bool task, bool shouldOverride, int? threadCount, BigInteger? maxN)
        {
            //TODO change out folder in UI
            // INFO: Don't forget to set the working directory to the "out" folder in the run-config!"
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                Console.WriteLine("\nStarting...");
                var max = maxN ?? BigInteger.Parse("999999999999999999999999999999");
                ThreadMaster.Max = max;
                TaskMaster.Max = max;
                if (shouldOverride)
                {
                    TaskMaster.StartSieve(max <= MaxSieveValue ? max : MaxSieveValue);
                    if (task && max > MaxSieveValue) TaskMaster.Start();
                    if (!task) ThreadMaster.Start(threadCount ?? 0);
                }
                else
                {
                    if (FileHelper.FindLastPrime() <= max) return;
                    Console.WriteLine("3");
                    if (max <= MaxSieveValue) TaskMaster.StartSieve(max);
                    else if (task) TaskMaster.Start();
                    else ThreadMaster.Start(threadCount ?? 0);
                }
            }
            finally
            {
                FileHelper.Dispose();
            }
        }
    }
}