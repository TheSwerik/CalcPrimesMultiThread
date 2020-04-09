using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace CalcPrimesMultiThread.Prime.Task
{
    public static class StaticPrime
    {
        public static bool IsPrime(BigInteger n)
        {
            if ((n & 1) == 0) return false;

            var root = Math.Sqrt((long) n);
            for (var i = 3; i <= root; i += 2)
            {
                if (n % i != 0) continue;
                return false;
            }

            return true;
        }

        public static IEnumerable<int> PrimeSieve(int n, CancellationToken? token)
        {
            var sieve = new bool[n + 1];
            var primes = new List<int> {2};
            var root = (int) Math.Sqrt(n);
            for (long i = 3; i <= root && !(token?.IsCancellationRequested ?? false); i += 2)
                if (!sieve[i])
                {
                    primes.Add((int) i);
                    var add = i << 1;
                    for (var j = i * i; j < sieve.Length; j += add) sieve[j] = true;
                }

            for (var i = (root & 1) == 0 ? root + 1 : root + 2;
                 i < n && !(token?.IsCancellationRequested ?? false);
                 i += 2)
                if (!sieve[i])
                    primes.Add(i);
            return primes;
        }
    }
}