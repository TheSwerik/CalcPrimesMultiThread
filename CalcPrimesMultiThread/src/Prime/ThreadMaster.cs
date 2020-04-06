using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace CalcPrimesMultiThread.Prime
{
    public static class ThreadMaster
    {
        private const string Filename = "Primes.txt";
        private static int _n = 1;
        public static BigInteger Max { get; set; }

        public static void Start(int threadCount = -1)
        {
            _n = threadCount < 1 ? Environment.ProcessorCount : threadCount;
            _n = threadCount > 64 ? 64 : threadCount;

            var watch = new Stopwatch();

            // one Event will be used for every Prime Object
            var events = new WaitHandle[_n];
            var primes = new Prime[_n];

            // config and start ThreadPool
            Console.Write("Starting {0} Threads...", _n);

            for (var i = 0; i < _n; i++)
            {
                events[i] = new ManualResetEvent(false);
                primes[i] = new Prime(int.MaxValue - 57, (ManualResetEvent) events[i]);
            }

            Console.Write("\r" + new string(' ', 50) + "\r");
            Console.WriteLine("{0} Threads Started.", _n);
            
            Console.Write("Picking up, where we left off...");
            watch.Start();
            var x = BigInteger.Parse(File.ReadLines(Filename).Last());
            Console.Write("\r" + new string(' ', 50) + "\r");
            Console.WriteLine("Starting at {0}.", x);

            while (x <= Max)
            {
                if(watch.Elapsed.Milliseconds % 100_000 == 0) Console.Write("\rChecking from {0} to {1}...", x, x + threadCount * 2);
                for (var i = 0; i < _n; i++)
                {
                    primes[i].N = x += 2;
                    // give Threads to Pool
                    ThreadPool.QueueUserWorkItem(primes[i].SeeIfNIsPrime, i);
                }

                // Wait for all Threads
                WaitHandle.WaitAll(events);

                using var sw = File.AppendText("Test.txt");
                foreach (var prime in primes)
                    if (prime.IsPrime)
                        sw.WriteLine(prime.N);
            }

            watch.Stop();
            Console.WriteLine("\rCalculation finished in {0}.", watch.Elapsed.ToString());
        }

        public static void StartSieve()
        {
            Console.WriteLine("Calculating Primes till {0} with 1 Thread and PrimeSieve...", Max);
            var watch = new Stopwatch();

            watch.Start();
            var primes = new Prime(Max).PrimeSieve((int) Max);
            watch.Stop();
            var elapsed = watch.Elapsed.ToString();
            Console.WriteLine("Calculation finished in {0} Seconds.",
                double.Parse(elapsed.Substring(elapsed.LastIndexOf(":", StringComparison.Ordinal) + 1)));

            watch.Reset();

            Console.WriteLine("Writing to file...");
            watch.Start();
            using var sw = File.CreateText(Filename);
            foreach (var prime in primes) sw.WriteLine(prime);
            watch.Stop();
            elapsed = watch.Elapsed.ToString();
            Console.WriteLine("Finished in {0} Seconds. \n",
                double.Parse(elapsed.Substring(elapsed.LastIndexOf(":", StringComparison.Ordinal) + 1)));
        }
    }
}