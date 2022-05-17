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
        public static byte[] Encrypt(BigInteger privateKeyQ, BigInteger privateKeyP, out BigInteger publicKeyN, BigInteger publicKeyB,  byte[] inputData, out string encryptedDec)
        {
            if (!HelpMath.IsNumberPrime(privateKeyP) || !HelpMath.IsNumberPrime(privateKeyQ))
                throw new Exception("Not all keys are prime.");

            if (privateKeyP % 4 != 3 || privateKeyQ % 4 != 3)
                throw new Exception("Not all private keys %4 == 3");

            publicKeyN = privateKeyP * privateKeyQ;
            return Encrypt(publicKeyN, publicKeyB, inputData, out encryptedDec);
        }

        public static byte[] Encrypt(BigInteger publicKeyN, BigInteger publicKeyB, byte[] inputData, out string encryptedDec)
        {
            if (publicKeyN <= publicKeyB)
                throw new Exception("Public key B must be lower, than N");

            int maxEncryptedCapacity = publicKeyN.ToByteArray().Length;
            byte[] encryptedData = new byte[maxEncryptedCapacity * inputData.Length];
            var sbEncryptedDec = new StringBuilder();

            for(int i = 0; i <= inputData.Length - 1; i++)
            {
                byte currByte = inputData[i];
                var encryptedNum = currByte * (currByte + publicKeyB) % publicKeyN;
                sbEncryptedDec.Append(encryptedNum + " ");
                encryptedNum.ToByteArray().CopyTo(encryptedData, i * maxEncryptedCapacity);
            }

            encryptedDec = sbEncryptedDec.ToString();
            return encryptedData;
        }

        private static long s_currEncryptedPos;

        public static byte[] Decrypt(BigInteger privateKeyQ, BigInteger privateKeyP, BigInteger publicKeyB, byte[] encryptedData)
        {
            s_currEncryptedPos = 0;
            BigInteger publicKeyN = privateKeyP * privateKeyQ;

            int maxEncryptedCapacity = publicKeyN.ToByteArray().Length;
            var decryptedBytes = new byte[encryptedData.Length/ maxEncryptedCapacity];

            for (long i = 0; i <= encryptedData.Length/ maxEncryptedCapacity - 1; i++)
            {
                var currNumber = new BigInteger(GetNumberBytes(encryptedData, maxEncryptedCapacity));
                decryptedBytes[i] = DecryptNumber(currNumber, privateKeyQ, privateKeyP, publicKeyN, publicKeyB);
            }
            
            return decryptedBytes;
        }

        private static byte DecryptNumber(BigInteger number, BigInteger privateKeyQ, BigInteger privateKeyP, BigInteger publicKeyN, BigInteger publicKeyB)
        {
            BigInteger discriminant = (publicKeyB * publicKeyB + 4 * number) % publicKeyN;

            var mp = BigInteger.ModPow(discriminant, (privateKeyP + 1) / 4, privateKeyP);
            var mq = BigInteger.ModPow(discriminant, (privateKeyQ + 1) / 4, privateKeyQ);

            HelpMath.ExtendedEuclidean(privateKeyP, privateKeyQ, out BigInteger yp, out BigInteger yq);
           
            var dResults = new BigInteger[4]
            {
                    (yp * privateKeyP * mq + yq * privateKeyQ * mp) % publicKeyN,
                    publicKeyN,
                    (yp * privateKeyP * mq - yq * privateKeyQ * mp) % publicKeyN,
                    publicKeyN
            };
            (dResults[1], dResults[3]) = (publicKeyN - dResults[0], publicKeyN - dResults[2]);

            var mResults = new BigInteger[4];
            for (int i = 0; i < 4; i++)
            {
                mResults[i] = ((dResults[i] - publicKeyB).IsEven) ? (dResults[i] - publicKeyB)/2 % publicKeyN : (dResults[i] + publicKeyN - publicKeyB) / 2 % publicKeyN;
            }

            foreach (BigInteger currRes in mResults)
                if (currRes >= 0 && currRes <= 255)
                    return currRes.ToByteArray()[0];

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
