using System;
using System.Numerics;
using System.Threading;

namespace CalcPrimesMultiThread.Prime.Thread
{
    public class Prime
    {
        private readonly ManualResetEvent _doneEvent;

        public Prime(ManualResetEvent doneEvent)
        {
            _doneEvent = doneEvent;
        }

        public Prime(BigInteger n)
        {
            N = n;
        }

        public BigInteger N { get; set; }
        public bool IsPrime { get; private set; }

        // wrapper-method for threadpool:
        public void CheckNPrime(object threadContext)
        {
            if ((N & 1) == 0)
            {
                IsPrime = false;
                _doneEvent.Set();
                return;
            }

            var root = Math.Sqrt((long) N);
            for (var i = 3; i <= root; i += 2)
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
    }
}