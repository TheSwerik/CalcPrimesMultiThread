using System;
using System.Globalization;
using System.Numerics;
using System.Threading;
using System.Windows.Controls.Ribbon;
using CalcPrimesMultiThread;
using CalcPrimesMultiThread.Prime.Thread;
using Frontend.Master;

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
            //TODO change out folder in UI
            // INFO: Don't forget to set the working directory to the "out" folder in the run-config!"
            var token = (CancellationToken) obj;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                Console.Write("Starting...");
                var max = MaxN ?? BigInteger.Parse("999999999999999999999999999999");
                ThreadMaster.Max = max;
                TaskMaster.Max = max;
                if (ShouldOverride)
                {
                    TaskMaster.StartSieve(max <= MaxSieveValue ? max : MaxSieveValue, token);
                    if (Task && max > MaxSieveValue) TaskMaster.Start(token);
                    if (!Task) ThreadMaster.Start(ThreadCount ?? 0);
                }
                else
                {
                    if (MaxN.HasValue && FileHelper.FindLastPrime() <= max) return;
                    if (MaxN.HasValue && max <= MaxSieveValue) TaskMaster.StartSieve(max, token);
                    else if (Task) TaskMaster.Start(token);
                    else ThreadMaster.Start(ThreadCount ?? 0);
                }
            }
            finally
            {
                FileHelper.Dispose();
            }
        }
    }
}