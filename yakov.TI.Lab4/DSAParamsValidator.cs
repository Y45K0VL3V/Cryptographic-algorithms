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
        public static bool Validate(Params @params)
        {
            if (!HelpMath.IsNumberPrime(@params.q))
                throw new ArgumentException("Q must be prime.");

            if ((@params.p - 1) % @params.q != 0)
                throw new ArgumentException("Q must be divider of p-1.");

            if ((@params.h <= 1) || (@params.h >= @params.p - 1))
                throw new ArgumentException("H must be in range of [2, p-2]");

            if ((@params.x <= 0) || (@params.x >= @params.q))
                throw new ArgumentException("X must be in range of [1, q-1]");

            if ((@params.k <= 0) || (@params.k >= @params.q))
                throw new ArgumentException("K must be in range of [1, q-1]");

            return true;
        }
    }
}
