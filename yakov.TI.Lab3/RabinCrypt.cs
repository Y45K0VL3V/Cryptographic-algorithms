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
        public byte[] Encrypt(BigInteger privateKeyQ, BigInteger privateKeyP, out BigInteger publicKeyN, BigInteger publicKeyB,  byte[] inputData)
        {
            if (!RabinHelpMath.IsNumberPrime(privateKeyP) || !RabinHelpMath.IsNumberPrime(privateKeyQ))
                throw new Exception("Not all keys are prime.");

            if (privateKeyP % 4 != 3 || privateKeyQ % 4 != 3)
                throw new Exception("Not all private keys %4 == 3");

            publicKeyN = privateKeyP * privateKeyQ;
            return Encrypt(publicKeyN, publicKeyB, inputData);
        }

        public byte[] Encrypt(BigInteger publicKeyN, BigInteger publicKeyB, byte[] inputData)
        {
            if (publicKeyN >= publicKeyB)
                throw new Exception("Public key B must be lower, than N");

            byte[] outputData = new byte[publicKeyN.GetByteCount() * inputData.Length];

            for(int i = 0; i <= inputData.Length - 1; i++)
            {
                byte currByte = inputData[i];
                (currByte * (currByte + publicKeyB) % publicKeyN).ToByteArray().CopyTo(outputData, i * publicKeyN.GetByteCount());
            }

            return outputData;
        }
    }
}
