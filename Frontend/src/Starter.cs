using System;
using System.Numerics;
using System.Threading;
using CalcPrimesMultiThread;
using CalcPrimesMultiThread.Prime.Task;
using CalcPrimesMultiThread.Prime.Thread;
using CalcPrimesMultiThread.Prime.util;

namespace Frontend
{
    internal static class Starter
    {
        private const int MaxSieveValue = int.MaxValue - 57;
        public static bool Task { get; set; }
        public static bool ShouldOverride { get; set; }
        public static int? ThreadCount { get; set; }
        public static BigInteger? MaxN { get; set; }

        internal static void Start(object obj)
        {
            // INFO: Don't forget to set the working directory to the "out" folder in the run-config!"
            var token = (CancellationToken) obj;
            try
            {
                CustomConsole.WriteLine("Starting..." + Environment.NewLine);
                var max = MaxN ?? BigInteger.Parse("999999999999999999999999999999");
                ThreadMaster.Max = max;
                TaskMaster.Max = max;
                if (ShouldOverride)
                {
                    TaskMaster.StartSieve(max <= MaxSieveValue ? max : MaxSieveValue, token);
                    if (Task && max > MaxSieveValue) TaskMaster.Start(token);
                    else if (!Task) ThreadMaster.Start(ThreadCount ?? 0);
                }
                else
                {
                    // CustomConsole.WriteLine("bbbbbb");
                    // if (MaxN.HasValue && FileHelper.FindLastPrime() <= max) return;
                    // if (MaxN.HasValue && max <= MaxSieveValue) TaskMaster.StartSieve(max, token);
                    // else
                    // {
                        // if (FileHelper.FindLastPrime() <= MaxSieveValue) TaskMaster.StartSieve(MaxSieveValue, token);
                        // CustomConsole.WriteLine("assss" + Environment.NewLine);
                        if (Task) TaskMaster.Start(token);
                        else ThreadMaster.Start(ThreadCount ?? 0);
                    // }
                }
            }
            finally
            {
                FileHelper.Dispose();
                MainWindow.Finished = true;
            }
        }
    }
}