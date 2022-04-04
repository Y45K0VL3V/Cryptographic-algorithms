using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace yakov.TI.Lab3
{
    public static class RabinHelpMath
    {
        public static bool IsNumberPrime(BigInteger num)
        {
            if (num == 0)
                return false;

            for (int i = 2; i <= ((num > 100) ? 100 : (num - 1)); i++)
            {
                var n = BigInteger.ModPow(i, num - 1, num);
                if (n != 1) 
                {
                    return false;
                }

            }
            return true;         
        }

        public static BigInteger ExtendedEuclidean(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (b < a)
            {
                (b, a) = (a, b);
            }

            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            BigInteger gcd = ExtendedEuclidean(b % a, a, out x, out y);

            BigInteger newY = x;
            BigInteger newX = y - (b / a) * x;

            x = newX;
            y = newY;
            return gcd;
        }
    }
}
