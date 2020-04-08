using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using CalcPrimesMultiThread.Prime.util;

namespace CalcPrimesMultiThread.Prime.Task
{
    public static class TaskMaster
    {
        private const long Step = 1_000_000;
        public static BigInteger Max { get; set; }

        public static void Start(CancellationToken? token)
        {
            var watch = new Stopwatch();


            CustomConsole.ReplaceLine("Picking up, where we left off...");
            var lastPrime = FileHelper.FindLastPrime();
            CustomConsole.ReplaceLine($"Starting at {lastPrime}.");

            System.Threading.Thread.Sleep(2000);

            CustomConsole.WriteLine("Starting calculation...");
            watch.Start();

            var current = lastPrime;
            for (BigInteger i = 0;
                 current < Max && !(token?.IsCancellationRequested ?? false);
                 current = lastPrime + ++i * Step)
            {
                CustomConsole.ReplaceLine($"Calculating till {current + Step}...");
                var bag = new ConcurrentBag<BigInteger>();

                ParallelFor(current, current + Step, n =>
                                                     {
                                                         if (StaticPrime.IsPrime(n)) bag.Add(n);
                                                     });

                var list = bag.ToList();
                list.Sort();
                FileHelper.WriteFile(list);
            }

            watch.Stop();
            FileHelper.Dispose();
            CustomConsole.NewLine();
            CustomConsole.WriteLine(
                $"Calculated for: {watch.Elapsed.ToString().Substring(0, watch.Elapsed.ToString().LastIndexOf('.'))}. Biggest Prime found: {FileHelper.FindLastPrime()}"
            );
        }

        private static IEnumerable<BigInteger> Range(BigInteger fromInclusive, BigInteger toExclusive)
        {
            for (var i = fromInclusive; i < toExclusive; i++) yield return i;
        }

        private static void ParallelFor(BigInteger fromInclusive, BigInteger toExclusive, Action<BigInteger> body)
        {
            Parallel.ForEach(Range(fromInclusive, toExclusive), body);
        }

        public static void StartSieve(BigInteger max, CancellationToken? token)
        {
            var help = Max;
            Max = max;
            CustomConsole.WriteLine($"Calculating Primes till {Max} with 1 Thread and PrimeSieve...");
            var watch = new Stopwatch();

            watch.Start();
            var primes = StaticPrime.PrimeSieve((int) Max, token);
            watch.Stop();
            var elapsed = watch.Elapsed.ToString();
            CustomConsole.WriteLine(
                $"Calculation finished in {double.Parse(elapsed.Substring(elapsed.LastIndexOf(":", StringComparison.Ordinal) + 1))} Seconds.");
            CustomConsole.NewLine();

            watch.Reset();

            CustomConsole.WriteLine("Writing to file...");
            watch.Start();
            FileHelper.Restart();
            FileHelper.WriteFile(primes);
            FileHelper.Dispose();
            watch.Stop();
            CustomConsole.WriteLine($"Finished in {watch.Elapsed.ToString()}.");
            CustomConsole.NewLine();
            Max = help;
        }
    }
}