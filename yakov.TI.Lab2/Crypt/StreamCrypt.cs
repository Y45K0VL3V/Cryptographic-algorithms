using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yakov.TI.Lab2.KeyGenerators;

namespace yakov.TI.Lab2.Crypt
{
    public static class StreamCrypt
    {
        // Input bytes changes to output after crypting
        public static string CryptBinary(LFSR keyGenerator, ref byte[] inputBytes, out string usedKeyBinary)
        {
            StringBuilder sbCryptResultBinary = new StringBuilder();
            StringBuilder sbUsedKeyBinary = new StringBuilder();
            List<byte> outBytes = new List<byte>();
            foreach (byte currByte in inputBytes)
            {
                byte rndByte = keyGenerator.GetRandomByte();
                byte resByte;
                outBytes.Add(resByte = (byte)(currByte ^ rndByte));

                sbUsedKeyBinary.Append(Convert.ToString(rndByte, 2).PadLeft(8, '0') + " ");
                sbCryptResultBinary.Append(Convert.ToString(resByte, 2).PadLeft(8, '0') + " ");
            }

            usedKeyBinary = sbUsedKeyBinary.ToString();
            inputBytes = outBytes.ToArray();

            return sbCryptResultBinary.ToString();
        }

    }
}
