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
            ManualResetEvent[] doneEvents = new ManualResetEvent[N];
            Fibonacci[] fibArray = new Fibonacci[N];
            Random r = new Random();
 
            // config and start ThreadPool
            Console.WriteLine("Starting {0} Tasks...", N);
 
            for (int i = 0; i < N; i++) {
                doneEvents[i] = new ManualResetEvent(false);
                Fibonacci f = new Fibonacci(r.Next(5, 46), doneEvents[i]);
                fibArray[i] = f;
 
                // give Tasks to Pool
                ThreadPool.QueueUserWorkItem(f.ThreadPoolCallback, i);
            }
 
            // Wait for all Threads
            WaitHandle.WaitAll(doneEvents);
            Console.WriteLine("All calculations finished.");
 
            // Zeige die Ergebnisse an
            for (int i = 0; i < N; i++) {
                Fibonacci f = fibArray[i];
                Console.WriteLine("Fibonacci({0}) = {1}", f.N, f.FibOfN);
            }
        }
    }
}