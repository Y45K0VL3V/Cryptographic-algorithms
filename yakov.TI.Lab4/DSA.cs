using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace yakov.TI.Lab4
{
    public struct Params
    {
        public BigInteger q { get; set; }
        public BigInteger p { get; set; }
        public BigInteger h { get; set; }
        public BigInteger x { get; set; }
        public BigInteger k { get; set; }
    }

    public class DSA
    {

        public static byte[] ToSign(byte[] input, Params @params)
        {
            BigInteger inputHash = SimpleHash.ToHash(input, @params.q);

            BigInteger g = BigInteger.ModPow(@params.h, (@params.p - 1) / @params.q, @params.p);

            BigInteger rKey = BigInteger.ModPow(g, @params.k, @params.p) % @params.q;
            BigInteger sKey = ((inputHash + @params.x*rKey)/@params.k) % @params.q;

            if (rKey == 0 || sKey == 0)
            {
                throw new Exception("Enter other k param.");
            }

            return (byte[])input.Concat(Encoding.UTF8.GetBytes($", {rKey}, {sKey}"));
        }
    }
}
