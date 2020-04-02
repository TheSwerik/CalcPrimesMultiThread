using System;
using System.Globalization;
using System.Threading;

namespace CalcPrimesMultiThread
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            ThreadMaster threadMaster = new ThreadMaster();
            threadMaster.Start();
        }
    }
}