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
        /// <summary>
        /// Crypt/decrypt input.
        /// </summary>
        /// <param name="keyGenerator">LFSR generator</param>
        /// <param name="text">(en/de)crypted text</param>
        /// <returns>Gives input XORed with key.</returns>
        public static string Crypt(LFSR keyGenerator, string text, out string usedKeyBinary, out string inputBinary, out string outputBinary)
        {
            StringBuilder sbCryptResult = new StringBuilder();
            StringBuilder sbUsedKeyBinary = new StringBuilder();
            StringBuilder sbInputBinary = new StringBuilder();
            StringBuilder sbOutputBinary = new StringBuilder();

            foreach (byte currByte in text)
            {
                byte rndByte = keyGenerator.GetRandomByte();
                byte resByte = (byte)(currByte ^ rndByte);

                sbUsedKeyBinary.Append(Convert.ToString(rndByte, 2).PadLeft(8, '0') + " ");
                sbInputBinary.Append(Convert.ToString(currByte, 2).PadLeft(8, '0') + " ");
                sbOutputBinary.Append(Convert.ToString(resByte, 2).PadLeft(8, '0') + " ");

                sbCryptResult.Append((char)resByte);
            }

            usedKeyBinary = sbUsedKeyBinary.ToString();
            inputBinary = sbInputBinary.ToString();
            outputBinary = sbOutputBinary.ToString();

            return sbCryptResult.ToString();
        }

    }
}
