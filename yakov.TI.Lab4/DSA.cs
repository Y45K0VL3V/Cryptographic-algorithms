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

        public static byte[] ToSign(byte[] input, Params @params, out BigInteger openKey)
        {
            BigInteger inputHash = SimpleHash.ToHash(input, @params.q);

            BigInteger g = BigInteger.ModPow(@params.h, (@params.p - 1) / @params.q, @params.p);

            BigInteger rKey = BigInteger.ModPow(g, @params.k, @params.p) % @params.q;
            BigInteger sKey = ((inputHash + @params.x*rKey)/@params.k) % @params.q;

            if (rKey == 0 || sKey == 0)
            {
                throw new Exception("Enter other k param.");
            }

            openKey = BigInteger.ModPow(g, @params.x, @params.p);
            return (byte[])input.Concat(Encoding.UTF8.GetBytes($", {rKey}, {sKey}"));
        }

        public static bool IsSignCorrect(byte[] input, Params @params, BigInteger openKey, out BigInteger rawTextHash)
        {
            string fullInputText = Encoding.UTF8.GetString(input);
            
            int signStartIndex = 0;
            bool isFind = false;
            for (int i = fullInputText.Length - 1; i >= 1; i--)
            {
                if(fullInputText[i] == ',')
                {
                    if(!isFind)
                        isFind = true;
                    else
                    {
                        signStartIndex = i;
                        break;
                    }
                }
            }

            rawTextHash = 0;
            if (signStartIndex == 0)
                return false;

            #region Extract sign data from input.

            byte[] rawTextBytes = Encoding.UTF8.GetBytes(fullInputText.Substring(0, signStartIndex));
            rawTextHash = SimpleHash.ToHash(rawTextBytes, @params.q);

            string signData = fullInputText.Substring(signStartIndex);
            BigInteger rKey = BigInteger.Parse(signData.Substring(2, signData.IndexOf(',', 1) - 2));
            BigInteger sKey = BigInteger.Parse(signData.Substring(signData.IndexOf(',', 1)));
            
            #endregion

            BigInteger w = BigInteger.ModPow(sKey, @params.q - 2, @params.q);
            BigInteger u1 = rawTextHash * w % @params.q;
            BigInteger u2 = rKey * w % @params.q;
            BigInteger g = BigInteger.ModPow(@params.h, (@params.p - 1) / @params.q, @params.p);
            BigInteger v = BigInteger.Pow(g, (int)u1) * BigInteger.Pow(openKey, (int)u2) % @params.p % @params.q;

            return rKey == v ? true : false;
        }
    }
}
