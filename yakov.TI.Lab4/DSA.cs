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

            throw new NotImplementedException();
        }
    }
}
