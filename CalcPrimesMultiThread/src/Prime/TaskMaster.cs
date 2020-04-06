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

            var bag = new ConcurrentBag<BigInteger>() {2};

            ParallelFor(3, Max, i =>
            {
                if (StaticPrime.IsPrime(i))
                {
                    bag.Add(i);
                    //Console.WriteLine(i + " is Prime!");
                }
            });

            var list = bag.ToList();
            list.Sort();

            watch.Stop();
            Console.WriteLine("\rCalculation finished in {0}.", watch.Elapsed.ToString());
            //Console.WriteLine(string.Join(" ", list));
        }

        private static IEnumerable<BigInteger> Range(BigInteger fromInclusive, BigInteger toExclusive)
        {
            for (var i = fromInclusive; i < toExclusive; i++) yield return i;
        }

        private static void ParallelFor(BigInteger fromInclusive, BigInteger toExclusive, Action<BigInteger> body)
        {
            Parallel.ForEach(Range(fromInclusive, toExclusive), body);
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