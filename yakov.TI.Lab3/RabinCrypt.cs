using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace yakov.TI.Lab3
{
    public class RabinCrypt
    {
        public byte[] Encrypt(BigInteger privateKeyQ, BigInteger privateKeyP, out BigInteger publicKey, byte[] inputData)
        {
            if (!RabinHelpMath.IsNumberPrime(privateKeyP) || !RabinHelpMath.IsNumberPrime(privateKeyQ))
                throw new Exception("Not all keys are prime.");

            publicKey = privateKeyP * privateKeyQ;
            return Encrypt(publicKey);
        }

        public byte[] Encrypt(BigInteger publicKey, byte[] inputData)
        {


            //return
        }
    }
}
