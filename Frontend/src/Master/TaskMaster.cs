using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using CalcPrimesMultiThread;
using CalcPrimesMultiThread.Prime.Task;

namespace Frontend.Master
{
    public static class TaskMaster
    {
        private const long Step = 1_000_000;
        public static BigInteger Max { get; set; }

        public static void Start(CancellationToken token)
        {
            var watch = new Stopwatch();

            Console.Write("\rPicking up, where we left off...");
            var lastPrime = FileHelper.FindLastPrime();
            Console.Write("\r" + new string(' ', 50) + "\r");
            Console.WriteLine("Starting at {0}.", lastPrime);

            Console.Write("Starting calculation...");
            watch.Start();

            var current = lastPrime;
            for (BigInteger i = 0;
                current < Max && !token.IsCancellationRequested;
                current = lastPrime + ++i * Step)
            {
                Console.Write("\rCalculating till {0}...", current + Step);
                var bag = new ConcurrentBag<BigInteger>();

                ParallelFor(current, current + Step, n =>
                {
                    if (StaticPrime.IsPrime(n)) bag.Add(n);
                });

                var list = bag.ToList();
                list.Sort();
                FileHelper.WriteFile(list, token);
            }

            watch.Stop();
            FileHelper.Dispose();
            Console.WriteLine("\nCalculated for: {0}. Biggest Prime found: {1}",
                watch.Elapsed.ToString().Substring(0, watch.Elapsed.ToString().LastIndexOf('.')), 
                FileHelper.FindLastPrime());
        }

        private static IEnumerable<BigInteger> Range(BigInteger fromInclusive, BigInteger toExclusive)
        {
            for (var i = fromInclusive; i < toExclusive; i++) yield return i;
        }

        private static void ParallelFor(BigInteger fromInclusive, BigInteger toExclusive, Action<BigInteger> body)
        {
            Parallel.ForEach(Range(fromInclusive, toExclusive), body);
        }

        public static void StartSieve(BigInteger max, CancellationToken token)
        {
            var help = Max;
            Max = max;
            Console.WriteLine("Calculating Primes till {0} with 1 Thread and PrimeSieve...", Max);
            var watch = new Stopwatch();

            watch.Start();
            var primes = StaticPrime.PrimeSieve((int) Max, token);
            watch.Stop();
            var elapsed = watch.Elapsed.ToString();
            Console.WriteLine("Calculation finished in {0} Seconds.",
                double.Parse(elapsed.Substring(elapsed.LastIndexOf(":", StringComparison.Ordinal) + 1)));

            watch.Reset();

            Console.WriteLine("Writing to file...");
            watch.Start();
            FileHelper.Restart();
            FileHelper.WriteFile(primes, token);
            FileHelper.Dispose();
            watch.Stop();
            Console.WriteLine("Finished in {0}. \n", watch.Elapsed.ToString());
            Max = help;
        }
    }
}