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
            for (int i = 2; i <= 100; i++)
            {
                var n = BigInteger.ModPow(i, num - 1, num);
                if (n != 1) 
                {
                    return false;
                }

            }
            return true;         
        }
    }
}
