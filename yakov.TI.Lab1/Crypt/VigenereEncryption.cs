using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace yakov.TI.Lab1.Crypt
{
    public class VigenereEncryption
    {
        static VigenereEncryption()
        {
            SetAlphabet("АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ");
        }

        public static string Alphabet { get; set; }

        public static void SetAlphabet(string newAlphabet)
        {
            Alphabet = newAlphabet;
        }

        public static string GetValidChars(string inputText)
        {
            StringBuilder sbBuffer = new();
            string validPattern = $"[{Alphabet}]+";

            foreach (Match validPart in Regex.Matches(inputText, validPattern))
            {
                sbBuffer.Append(validPart.Value);
            }

            return sbBuffer.ToString() ?? "";
        }

        public static string Encrypt(string inputText, string keyWord)
        {
            keyWord = GetValidChars(keyWord?.ToUpper() ?? "");
            char[] inputTextArr = GetValidChars(inputText?.ToUpper() ?? "").ToCharArray();

            int keyWordIndex = 0;
            for (int i = 0; i <= inputTextArr.Length - 1; i++)
            {
                if (Alphabet.IndexOf(inputTextArr[i]) != -1)
                {
                    inputTextArr[i] = GetNewChar(inputTextArr[i], keyWord[keyWordIndex++ % keyWord.Length]);
                }
            }

            return new string(inputTextArr);
        }

        public static string Decrypt(string encryptedText, string keyWord)
        {
            keyWord = GetValidChars(keyWord?.ToUpper() ?? "");
            char[] encryptedTextArr = GetValidChars(encryptedText?.ToUpper() ?? "").ToCharArray();

            int keyWordIndex = 0;
            for (int i = 0; i <= encryptedTextArr.Length - 1; i++)
            {
                if (Alphabet.IndexOf(encryptedTextArr[i]) != -1)
                {
                    encryptedTextArr[i] = GetDecryptedChar(encryptedTextArr[i], keyWord[keyWordIndex++ % keyWord.Length]);
                }
            }

            return new string(encryptedTextArr);
        }

        private static char GetNewChar(char oldChar, char keyChar)
        {
            return Alphabet[(Alphabet.IndexOf(oldChar) + Alphabet.IndexOf(keyChar)) % Alphabet.Length];
        }

        private static char GetDecryptedChar(char encryptedChar, char keyChar)
        {
            var index = (Alphabet.IndexOf(encryptedChar) - Alphabet.IndexOf(keyChar)) % Alphabet.Length;
            // TODO: Need this?
            if (index < 0)
            {
                index = Alphabet.Length + index;
            }
            return Alphabet[index];
        }
    }
}
