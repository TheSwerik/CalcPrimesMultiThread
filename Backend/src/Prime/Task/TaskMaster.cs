using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace CalcPrimesMultiThread.Prime.Task
{
    public static class TaskMaster
    {
        public static BigInteger Max { get; set; }

        public static void Start()
        {
            var watch = new Stopwatch();

            Console.Write("Picking up, where we left off...");
            var lastPrime = FileHelper.FindLastPrime();
            Console.Write("\r" + new string(' ', 50) + "\r");
            Console.WriteLine("Starting at {0}.", lastPrime);

            Console.Write("Starting calculation...");
            watch.Start();

            var current = lastPrime;
            for (BigInteger i = 0; current < Max; current = lastPrime + ++i * 10_000_000)
            {
                Console.Write("\rCalculating till {0}...", current + 10_000_000);
                var bag = new ConcurrentBag<BigInteger>();

                ParallelFor(current, current + 10_000_000, n =>
                {
                    if (StaticPrime.IsPrime(n)) bag.Add(n);
                });

                var list = bag.ToList();
                list.Sort();
                Console.Write("\rWriting from {0} to {1} to File...", current, current + 10_000_000);
                FileHelper.WriteFile(list, null);
            }

            watch.Stop();
            Console.WriteLine("\nFinished in {0} .", watch.Elapsed.ToString());
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
            var help = Max;
            Max = max;
            Console.WriteLine("Calculating Primes till {0} with 1 Thread and PrimeSieve...", Max);
            var watch = new Stopwatch();

            watch.Start();
            var primes = StaticPrime.PrimeSieve((int) Max, null);
            watch.Stop();
            var elapsed = watch.Elapsed.ToString();
            Console.WriteLine("Calculation finished in {0} Seconds.",
                double.Parse(elapsed.Substring(elapsed.LastIndexOf(":", StringComparison.Ordinal) + 1)));

            watch.Reset();

            Console.WriteLine("Writing to file...");
            watch.Start();
            FileHelper.Restart();
            FileHelper.WriteFile(primes, null);
            FileHelper.Dispose();
            watch.Stop();
            Console.WriteLine("Finished in {0}. \n", watch.Elapsed.ToString());
            Max = help;
        }
    }
}