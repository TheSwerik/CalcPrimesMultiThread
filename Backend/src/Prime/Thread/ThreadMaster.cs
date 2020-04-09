using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using CalcPrimesMultiThread.Prime.util;

namespace CalcPrimesMultiThread.Prime.Thread
{
    public static class ThreadMaster
    {
        private static int _threadCount = 1;
        public static BigInteger Max { get; set; }

        public static void Start(int threadCount = -1)
        {
            _threadCount = threadCount < 1 ? Environment.ProcessorCount : threadCount;
            _threadCount = threadCount > 64 ? 64 : threadCount;

            var watch = new Stopwatch();

            // one Event will be used for every Prime Object
            var events = new WaitHandle[_threadCount];
            var primes = new Prime[_threadCount];

            // config and start ThreadPool
            CustomConsole.WriteLine($"Starting {_threadCount} Threads...");

            for (var i = 0; i < _threadCount; i++)
            {
                events[i] = new ManualResetEvent(false);
                primes[i] = new Prime((ManualResetEvent) events[i]);
            }

            CustomConsole.ReplaceLine($"{_threadCount} Threads Started.");

            CustomConsole.WriteLine("Picking up, where we left off...");
            watch.Start();
            var lastPrime = FileHelper.FindLastPrime();
            CustomConsole.ReplaceLine($"Starting at {lastPrime}." + Environment.NewLine);

            while (lastPrime <= Max)
            {
                if (watch.Elapsed.Milliseconds % 100_000 < 100)
                    CustomConsole.ReplaceLine($"Checking from {lastPrime} to {lastPrime + threadCount * 2}...");

                for (var i = 0; i < _threadCount; i++)
                {
                    primes[i].N = lastPrime += 2;
                    // give Threads to Pool
                    ThreadPool.QueueUserWorkItem(primes[i].CheckNPrime, i);
                }

                // Wait for all Threads
                WaitHandle.WaitAll(events);

                FileHelper.WriteFile((from prime in primes where prime.IsPrime select prime.N).ToList());
            }

            watch.Stop();
            CustomConsole.WriteLine($"Calculation finished in {watch.Elapsed.ToString()}.");
        }
    }
}