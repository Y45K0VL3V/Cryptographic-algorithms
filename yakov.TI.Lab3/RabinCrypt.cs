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
        public static byte[] Encrypt(BigInteger privateKeyQ, BigInteger privateKeyP, out BigInteger publicKeyN, BigInteger publicKeyB,  byte[] inputData)
        {
            if (!RabinHelpMath.IsNumberPrime(privateKeyP) || !RabinHelpMath.IsNumberPrime(privateKeyQ))
                throw new Exception("Not all keys are prime.");

            if (privateKeyP % 4 != 3 || privateKeyQ % 4 != 3)
                throw new Exception("Not all private keys %4 == 3");

            publicKeyN = privateKeyP * privateKeyQ;
            return Encrypt(publicKeyN, publicKeyB, inputData);
        }

        public static byte[] Encrypt(BigInteger publicKeyN, BigInteger publicKeyB, byte[] inputData)
        {
            if (publicKeyN >= publicKeyB)
                throw new Exception("Public key B must be lower, than N");

            byte[] encryptedData = new byte[publicKeyN.GetByteCount() * inputData.Length];

            for(int i = 0; i <= inputData.Length - 1; i++)
            {
                byte currByte = inputData[i];
                (currByte * (currByte + publicKeyB) % publicKeyN).ToByteArray().CopyTo(encryptedData, i * publicKeyN.GetByteCount());
            }

            return encryptedData;
        }

        private static long s_currEncryptedPos;

        public static byte[] Decrypt(BigInteger privateKeyQ, BigInteger privateKeyP, BigInteger publicKeyB, byte[] encryptedData)
        {
            s_currEncryptedPos = 0;
            BigInteger publicKeyN = privateKeyP * privateKeyQ;

            for (long i = 1; i <= encryptedData.Length/publicKeyN.GetByteCount(); i++)
            {
                var currNumber = new BigInteger(GetNumberBytes(encryptedData, publicKeyN.GetByteCount()));
            }
            
        }

        // For decrypt only.
        private static byte[] GetNumberBytes(byte[] srcArray, int length)
        {
            var bytes = new byte[length];
            long endPos = s_currEncryptedPos + length - 1;

            try
            {
                while (s_currEncryptedPos <= endPos)
                    // Works properly, if length stays the same for full array.
                    bytes[s_currEncryptedPos % length] = srcArray[s_currEncryptedPos++];
            }
            catch { return new byte[length]; }

            return bytes;
        }
    }
}
