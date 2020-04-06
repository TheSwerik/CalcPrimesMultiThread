using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace CalcPrimesMultiThread.Prime
{
    public static class StaticPrime
    {
        public static bool IsPrime(BigInteger n)
        {
            if (n % 2 == 0) return false;

            var root = Math.Sqrt((long) n);
            for (var i = 3; i <= root; i += 2)
            {
                if (n % i != 0) continue;
                return false;
            }

            return true;
        }
    }
}