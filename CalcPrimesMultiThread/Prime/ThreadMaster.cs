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
        private const string Filename = "Test.txt";
        private static int _n = 1;

        public static void Start(int threadCount = -1)
        {
            _n = threadCount < 0 ? Environment.ProcessorCount : threadCount;
                
            // one Event will be used for every Prime Object
            var events = new WaitHandle[_n];
            var primes = new Prime[_n];

            // config and start ThreadPool
            Console.WriteLine("Starting {0} Threads...", _n);

            for (var i = 0; i < _n; i++)
            {
                events[i] = new ManualResetEvent(false);
                primes[i] = new Prime(int.MaxValue - 57, (ManualResetEvent) events[i]);
            }

            BigInteger x = 0;
            x = BigInteger.Parse(File.ReadAllLines(Filename).Last(s => !s.Equals("")));
            while (x <= Max)
            {
                for (var i = 0; i < _n; i++)
                {
                    primes[i].N = (x += 2);
                    // give Threads to Pool
                    ThreadPool.QueueUserWorkItem(primes[i].SeeIfNIsPrime, i);
                }

                // Wait for all Threads
                WaitHandle.WaitAll(events);
                Console.Write("\rAll Threads finished.");

                using var sw = File.AppendText("Test.txt");
                foreach (var prime in primes)
                {
                    if (prime.IsPrime) sw.WriteLine(prime.N);
                }
            }
        }
        public static BigInteger Max { get; set; }

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
            using var sw = File.CreateText("Test.txt");
            foreach (var prime in primes)
            {
                sw.WriteLine(prime);
            }
            watch.Stop();
            elapsed = watch.Elapsed.ToString();
            Console.WriteLine("Finished in {0} Seconds.",
                double.Parse(elapsed.Substring(elapsed.LastIndexOf(":", StringComparison.Ordinal) + 1)));
        }
    }
}