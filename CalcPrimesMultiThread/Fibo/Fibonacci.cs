using System;
using System.Threading;

namespace CalcPrimesMultiThread
{
    public class Fibonacci
    {
        private readonly int _n;
        private long _fibOfN;
        private readonly ManualResetEvent _doneEvent;

        public Fibonacci(int n, ManualResetEvent doneEvent)
        {
            _n = n;
            _doneEvent = doneEvent;
        }

        // wrapper-method for threadpool:
        public void ThreadPoolCallback(object threadContext)
        {
            int threadIndex = (int) threadContext;
            Console.WriteLine("Thread {0} starts calculating all fibos till " + _n + " ...", threadIndex);
            _fibOfN = Calculate(_n);
            Console.WriteLine("Thread {0} finished...", threadIndex);

            // notify that calculation finished:
            _doneEvent.Set();
        }

        private long Calculate(int n)
        {
            if (n <= 1) return n;
            return Calculate(n - 1) + Calculate(n - 2);
        }

        public int N => _n;
        public long FibOfN => _fibOfN;
    }
}