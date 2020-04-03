using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Threading;

namespace CalcPrimesMultiThread.Prime
{
    public static class ThreadMaster
    {
        private const int N = 1;

        public static void Start(int threadCount = -1)
        {
            if (threadCount < 0) threadCount = Environment.ProcessorCount;

            // one Event will be used for every Prime Object
            var events = new WaitHandle[N];
            var primes = new Prime[N];

            // config and start ThreadPool
            Console.WriteLine("Starting {0} Threads...", N);

            for (var i = 0; i < N; i++)
            {
                events[i] = new ManualResetEvent(false);
                var prime = new Prime(int.MaxValue - 57, (ManualResetEvent) events[i]);
                primes[i] = prime;

                // give Threads to Pool
                ThreadPool.QueueUserWorkItem(prime.ThreadPoolCallback, i);
            }

            // Wait for all Threads
            WaitHandle.WaitAll(events);
            Console.WriteLine("All calculations finished.");

            // Show results
            for (var i = 0; i < N; i++)
            {
                var f = primes[i];
                Console.WriteLine("PrimeSieve({0}) = {1}", f.N, f.PrimesTillN);
            }

            Thread.Sleep(500);
        }

        public static void StartSieve(BigInteger max)
        {
            Console.WriteLine("Calculating Primes till {0} with 1 Thread and PrimeSieve...", max);
            var watch = new Stopwatch();

            watch.Start();
            var primes = new Prime(max).PrimeSieve((int) max);
            watch.Stop();
            var elapsed = watch.Elapsed.ToString();
            Console.WriteLine("Calculation finished in {0} Seconds.",
                double.Parse(elapsed.Substring(elapsed.LastIndexOf(":", StringComparison.Ordinal) + 1)));

            watch.Reset();

            Console.WriteLine("Writing to file...");
            watch.Start();
            File.WriteAllText("Test.txt", string.Join("\n", primes));
            watch.Stop();
            elapsed = watch.Elapsed.ToString();
            Console.WriteLine("Finished in {0} Seconds.",
                double.Parse(elapsed.Substring(elapsed.LastIndexOf(":", StringComparison.Ordinal) + 1)));
        }
    }
}