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
            var token = (CancellationToken) obj;
            try
            {
                CustomConsole.WriteLine("Starting..." + Environment.NewLine);
                var max = MaxN ?? BigInteger.Parse("999999999999999999999999999999");
                ThreadMaster.Max = TaskMaster.Max = max;
                if (ShouldOverride) TaskMaster.StartSieve(max <= MaxSieveValue ? max : MaxSieveValue, token);
                if (Task) TaskMaster.Start(token);
                else ThreadMaster.Start(ThreadCount ?? 0, token);
            }
            finally
            {
                FileHelper.Dispose();
                MainWindow.Finished = true;
            }
        }
    }
}