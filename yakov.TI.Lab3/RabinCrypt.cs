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

            var decryptedBytes = new byte[encryptedData.Length/publicKeyN.GetByteCount()];

            for (long i = 0; i <= encryptedData.Length/publicKeyN.GetByteCount() - 1; i++)
            {
                var currNumber = new BigInteger(GetNumberBytes(encryptedData, publicKeyN.GetByteCount()));
                decryptedBytes[i] = DecryptNumber(currNumber, privateKeyQ, privateKeyP, publicKeyN, publicKeyB);
            }
            
            return decryptedBytes;
        }

        private static byte DecryptNumber(BigInteger number, BigInteger privateKeyQ, BigInteger privateKeyP, BigInteger publicKeyN, BigInteger publicKeyB)
        {
            BigInteger discriminant = (publicKeyB * publicKeyB + 4 * number) % publicKeyN;

            var mp = BigInteger.ModPow(discriminant, (privateKeyP + 1) / 4, privateKeyP);
            var mq = BigInteger.ModPow(discriminant, (privateKeyQ + 1) / 4, privateKeyQ);

            RabinHelpMath.ExtendedEuclidean(privateKeyP, privateKeyQ, out BigInteger yp, out BigInteger yq);
           
            var mResults = new BigInteger[4]
            {
                    (yp * privateKeyP * mq + yq * privateKeyQ * mp) % publicKeyN,
                    publicKeyN,
                    (yp * privateKeyP * mq - yq * privateKeyQ * mp) % publicKeyN,
                    publicKeyN
            };
            (mResults[1], mResults[3]) = (publicKeyN - mResults[0], publicKeyN - mResults[2]);

            foreach (BigInteger currRes in mResults)
                if (currRes.GetByteCount() == 1)
                    return currRes.ToByteArray().First();

            return 0;
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
