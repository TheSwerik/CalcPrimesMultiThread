using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace CalcPrimesMultiThread.Prime
{
    public class Prime
    {
        private readonly ManualResetEvent _doneEvent;

        public Prime(int n, ManualResetEvent doneEvent)
        {
            N = n;
            _doneEvent = doneEvent;
        }

        public Prime(BigInteger n)
        {
            N = n;
            _doneEvent = null;
        }

        public BigInteger N { get; set; }
        public bool IsPrime { get; private set; }

        // wrapper-method for threadpool:
        public void SeeIfNIsPrime(object threadContext)
        {
            var root = Math.Sqrt((double) N);
            var primes = PrimeSieve((int) N);
            foreach (var prime in primes)
            {
                if (N % prime != 0) continue;
                IsPrime = false;
                break;
            }
            IsPrime = true;

            // notify that calculation finished:
            _doneEvent.Set();
        }

        public int[] PrimeSieve(int n)
        {
            var sieve = new bool[n + 1];
            var primes = new List<int> {2};
            var root = (int) Math.Sqrt(n);
            for (long i = 3; i <= root; i += 2)
                // System.Console.WriteLine(1);
                if (!sieve[i])
                {
                    // System.Console.WriteLine(2);
                    primes.Add((int) i);
                    for (var j = i * i; j < sieve.Length; j += i << 1) sieve[j] = true;
                    // System.Console.WriteLine(3);
                }

            for (var i = (root & 1) == 0 ? root + 1 : root + 2; i < n; i += 2)
                if (!sieve[i])
                    primes.Add(i);
            return primes.ToArray();
        }
    }
}