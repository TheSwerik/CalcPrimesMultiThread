using System;
using System.Collections.Generic;
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

        // wrapper-method for threadpool:
        public void ThreadPoolCallback(object threadContext)
        {
            var threadIndex = (int) threadContext;
            Console.WriteLine("Thread {0} starts calculating all primes till " + N + " ...", threadIndex);
            FibOfN = PrimeSieve(N).LongLength;
            Console.WriteLine("Thread {0} finished...", threadIndex);

            // notify that calculation finished:
            _doneEvent.Set();
        }

        private static int[] PrimeSieve(int n) {
            var sieve = new bool[n + 1];
            var primes = new List<int> {2};
            int root = (int) Math.Sqrt(n);
            for (int i = 3; i <= root; i += 2) {
                if (!sieve[i]) {
                    primes.Add(i);
                    for (int j = i * i; j < sieve.Length; j += i << 1) {
                        sieve[j] = true;
                    }
                }
            }
            for (int i = (root & 1) == 0 ? root + 1 : root + 2; i < n; i += 2) {
                if (!sieve[i]) primes.Add(i);
            }
            return primes.ToArray();
        }

        public int N { get; }
        public long FibOfN { get; private set; }
    }
}