using System;
using System.Threading;

namespace CalcPrimesMultiThread.Fibo
{
    public class ThreadMaster
    {
        private const int N = 16;

        public void Start()
        {
            // one Event will be used for every Fibonacci Object
            var doneEvents = new ManualResetEvent[N];
            var fibArray = new Fibonacci[N];
            var r = new Random();

            // config and start ThreadPool
            Console.WriteLine("Starting {0} Tasks...", N);

            for (var i = 0; i < N; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                var f = new Fibonacci(r.Next(5, 46), doneEvents[i]);
                fibArray[i] = f;

                // give Tasks to Pool
                ThreadPool.QueueUserWorkItem(f.ThreadPoolCallback, i);
            }

            // Wait for all Threads
            WaitHandle.WaitAll(doneEvents);
            Console.WriteLine("All calculations finished.");

            // Zeige die Ergebnisse an
            for (var i = 0; i < N; i++)
            {
                var f = fibArray[i];
                Console.WriteLine("Fibonacci({0}) = {1}", f.N, f.FibOfN);
            }
        }
    }
}