using System;
using System.Threading;

namespace CalcPrimesMultiThread.Prime
{
    public class ThreadMaster
    {
        private const int N = 1;
        public void Start()
        {
            // one Event will be used for every Fibonacci Object
            ManualResetEvent[] doneEvents = new ManualResetEvent[N];
            Prime[] fibArray = new Prime[N];
            Random r = new Random();
 
            // config and start ThreadPool
            Console.WriteLine("Starting {0} Tasks...", N);
 
            for (int i = 0; i < N; i++) {
                doneEvents[i] = new ManualResetEvent(false);
                Prime f = new Prime(r.Next(10000000, 1000000000), doneEvents[i]);
                fibArray[i] = f;
 
                // give Tasks to Pool
                ThreadPool.QueueUserWorkItem(f.ThreadPoolCallback, i);
            }
 
            // Wait for all Threads
            WaitHandle.WaitAll(doneEvents);
            Console.WriteLine("All calculations finished.");
 
            // Show results
            for (int i = 0; i < N; i++) {
                var f = fibArray[i];
                Console.WriteLine("PrimeSieve({0}) = {1}", f.N, f.FibOfN);
            }
            Thread.Sleep(500);
        }
    }
}