using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace yakov.TI.Lab4
{
    public static class SimpleHash
    {
        public const int DEFAULT_HASH = 100;

        public static BigInteger ToHash(byte[] input, BigInteger modNum)
        {
            try
           {
                BigInteger currHash = Convert.ToInt32(Math.Pow(DEFAULT_HASH + input[0], 2)) % modNum;

                for (int i = 1; i < input.Length; i++)
                    currHash = Convert.ToInt32(Math.Pow((double)currHash + input[i], 2)) % modNum;

                return currHash;
            }
            catch
            {
                return DEFAULT_HASH;
            }
        }
    }
}
