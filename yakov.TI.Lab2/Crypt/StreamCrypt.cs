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
        public static string Crypt(LFSR keyGenerator, string text, out string usedKeyBinary)
        {
            var encryptedBytes = new List<byte>();
            StringBuilder cryptResult = new StringBuilder();
            StringBuilder sbUsedKeyBinary = new StringBuilder();
            foreach (byte currByte in text)
            {
                byte rndByte = keyGenerator.GetRandomByte();
                sbUsedKeyBinary.Append(Convert.ToString(rndByte, 2).PadLeft(8, '0') + " ");
                cryptResult.Append((char)(currByte ^ rndByte));
            }

            usedKeyBinary = sbUsedKeyBinary.ToString();
            return cryptResult.ToString();
        }

    }
}
