using System;
using System.Threading;

namespace CalcPrimesMultiThread
{
    public class Fibonacci
    {
        private readonly ManualResetEvent _doneEvent;

        public Fibonacci(int n, ManualResetEvent doneEvent)
        {
            N = n;
            _doneEvent = doneEvent;
        }

        public int N { get; }

        public long FibOfN { get; private set; }

        // wrapper-method for threadpool:
        public void ThreadPoolCallback(object threadContext)
        {
            var threadIndex = (int) threadContext;
            Console.WriteLine("Thread {0} starts calculating all fibos till " + N + " ...", threadIndex);
            FibOfN = Calculate(N);
            Console.WriteLine("Thread {0} finished...", threadIndex);

            // notify that calculation finished:
            _doneEvent.Set();
        }

        private long Calculate(int n)
        {
            if (n <= 1) return n;
            return Calculate(n - 1) + Calculate(n - 2);
        }
    }
}