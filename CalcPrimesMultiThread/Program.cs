using System;
using System.Globalization;
using System.Threading;
using CalcPrimesMultiThread.Prime;

namespace CalcPrimesMultiThread
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            var threadMaster = new ThreadMaster();
            threadMaster.Start();
            // TODO create TaskMaster
        }
    }
}