using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Threading;

namespace CalcPrimesMultiThread.Prime
{
    public class TaskMaster
    {
        private const string Filename = "Primes.txt";
        public static BigInteger Max { get; set; }

        public static void Start(int threadCount = -1)
        {
            var watch = new Stopwatch();

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