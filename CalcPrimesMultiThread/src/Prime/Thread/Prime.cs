using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace CalcPrimesMultiThread.Prime.Thread
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
            if (N % 2 == 0)
            {
                IsPrime = false;
                _doneEvent.Set();
                return;
            }

            var root = Math.Sqrt((long) N);
            for (var i = 3; i < root; i += 2)
            {
                if (N % i != 0) continue;
                IsPrime = false;
                _doneEvent.Set();
                return;
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
                if (!sieve[i])
                {
                    primes.Add((int) i);
                    var add = i << 1;
                    for (var j = i * i; j < sieve.Length; j += add) sieve[j] = true;
                }

            for (var i = (root & 1) == 0 ? root + 1 : root + 2; i < n; i += 2)
                if (!sieve[i])
                    primes.Add(i);
            return primes.ToArray();
        }
    }
}