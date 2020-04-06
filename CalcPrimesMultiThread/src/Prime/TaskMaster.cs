using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace CalcPrimesMultiThread.Prime
{
    public class TaskMaster
    {
        private const string Filename = "Primes.txt";
        public static BigInteger Max { get; set; }

        public static void Start()
        {
            var watch = new Stopwatch();

            Console.Write("Picking up, where we left off...");
            var lastPrime = BigInteger.Parse(File.ReadLines(Filename).Last());
            Console.Write("\r" + new string(' ', 50) + "\r");
            Console.WriteLine("Starting at {0}.", lastPrime);

            System.Console.WriteLine("\nStarting with calculaton...");
            watch.Start();

            var current = lastPrime;
            for (BigInteger i = 0; current < Max; i++)
            {
                current = lastPrime + i * 10_000_000;
                var bag = new ConcurrentBag<BigInteger>();

                ParallelFor(current, current + 10_000_000, n =>
                {
                    if (StaticPrime.IsPrime(n)) bag.Add(n);
                });

                var list = bag.ToList();
                list.Sort();
                using var sw = File.AppendText(Filename);
                foreach (var prime in list) sw.WriteLine(prime);
                Console.WriteLine("\rWritten till {0}.", current + 10_000_000);
            }

            watch.Stop();
            Console.WriteLine("Finished in {0} . \n", watch.Elapsed.ToString());
        }

        private static IEnumerable<BigInteger> Range(BigInteger fromInclusive, BigInteger toExclusive)
        {
            for (var i = fromInclusive; i < toExclusive; i++) yield return i;
        }

        private static void ParallelFor(BigInteger fromInclusive, BigInteger toExclusive, Action<BigInteger> body)
        {
            Parallel.ForEach(Range(fromInclusive, toExclusive), body);
        }

        public static void StartSieve(BigInteger max)
        {
            BigInteger help = Max;
            Max = max;
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
            Max = help;
        }
    }
}