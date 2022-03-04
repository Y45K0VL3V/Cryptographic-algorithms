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
        public static string Crypt(LFSR keyGenerator, string text)
        {
            var encryptedBytes = new List<byte>();
            foreach (byte currByte in Encoding.UTF8.GetBytes(text))
            {
                encryptedBytes.Add((byte)(currByte ^ keyGenerator.GetRandomByte()));
            }

            return Encoding.UTF8.GetString(encryptedBytes.ToArray());
        }

    }
}
