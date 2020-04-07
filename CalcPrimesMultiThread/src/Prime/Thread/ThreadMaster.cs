using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;

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
            Console.Write("Starting {0} Threads...", _threadCount);

            for (var i = 0; i < _threadCount; i++)
            {
                events[i] = new ManualResetEvent(false);
                primes[i] = new Prime((ManualResetEvent) events[i]);
            }

            Console.Write("\r" + new string(' ', 50) + "\r");
            Console.WriteLine("{0} Threads Started.", _threadCount);

            Console.Write("Picking up, where we left off...");
            watch.Start();
            var lastPrime = FileHelper.FindLastPrime();
            Console.Write("\r" + new string(' ', 50) + "\r");
            Console.WriteLine("Starting at {0}.", lastPrime);

            while (lastPrime <= Max)
            {
                if (watch.Elapsed.Milliseconds % 100_000 < 100)
                    Console.Write("\rChecking from {0} to {1}...", lastPrime, lastPrime + threadCount * 2);

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
            Console.WriteLine("\rCalculation finished in {0}.", watch.Elapsed.ToString());
        }
    }
}