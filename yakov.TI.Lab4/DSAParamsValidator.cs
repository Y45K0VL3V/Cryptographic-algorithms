using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using yakov.TI.Lab3;

namespace yakov.TI.Lab4
{
    public static class DSAParamsValidator
    {
        public static bool Validate(BigInteger q, BigInteger p, BigInteger h, BigInteger x, BigInteger k)
        {
            if (!HelpMath.IsNumberPrime(q))
                throw new ArgumentException("Q must be prime.");

            if ((p - 1) % q != 0)
                throw new ArgumentException("Q must be divider of p-1.");

            if ((h <= 1) || (h >= p - 1))
                throw new ArgumentException("H must be in range of [2, p-2]");

            if ((x <= 0) || (x >= q))
                throw new ArgumentException("X must be in range of [1, q-1]");

            if ((k <= 0) || (k >= q))
                throw new ArgumentException("K must be in range of [1, q-1]");

            return true;
        }
    }
}
