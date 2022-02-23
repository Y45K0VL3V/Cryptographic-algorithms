using System.Text;
using System.Text.RegularExpressions;

namespace yakov.TI.Lab1.Crypt
{
    public class DecimationEncryption
    {
        static DecimationEncryption()
        {
            SetAlphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        }

        public static string Alphabet { get; set; }

        public static void SetAlphabet(string newAlphabet)
        {
            Alphabet = newAlphabet;
        }

        public static string GetValidKey(string key)
        {
            StringBuilder sbKey = new();
            foreach(Match validPart in Regex.Matches(key, "\\d+"))
            {
                sbKey.Append(validPart.Value);
            }
            if (int.TryParse(sbKey.ToString(), out int tmp))
            {
                if (tmp == 0 || GCD(tmp, Alphabet.Length) != 1) sbKey.Clear();
            }
            return sbKey.ToString();
        }

        static int GCD(int a, int b)
        {
            if (a == 0)
            {
                return b;
            }
            else
            {
                while (b != 0)
                {
                    if (a > b)
                    {
                        a -= b;
                    }
                    else
                    {
                        b -= a;
                    }
                }

                return a;
            }
        }

        private static string GetValidChars(string inputText)
        {
            StringBuilder sbBuffer = new();
            string validPattern = $"[{Alphabet}]+";

            foreach (Match validPart in Regex.Matches(inputText, validPattern))
            {
                sbBuffer.Append(validPart.Value);
            }

            return sbBuffer.ToString() ?? "";
        }

        public static string Encrypt(string inputText, string key)
        {
            key = GetValidKey(key?.ToUpper()??"1");
            char[] inputTextArr = GetValidChars(inputText?.ToUpper()??"").ToCharArray();

            for(int i = 0; i <= inputTextArr.Length - 1; i++)
            {
                if (Alphabet.IndexOf(inputTextArr[i]) != -1)
                {
                    inputTextArr[i] = Alphabet[(Alphabet.IndexOf(inputTextArr[i]) * int.Parse(key)) % Alphabet.Length];
                }
            }

            return new string(inputTextArr);
        }

        public static string Decrypt(string encryptedText, string key)
        {
            key = GetValidKey(key?.ToUpper() ?? "1");
            char[] encryptedTextArr = GetValidChars(encryptedText?.ToUpper() ?? "").ToCharArray();

            char[] encryptedAlphabetArr = new char[Alphabet.Length];
            for(int i = 0; i <= Alphabet.Length - 1; i++)
            {
                encryptedAlphabetArr[i] = Alphabet[i * int.Parse(key) % Alphabet.Length];
            }
            string encryptedAlphabet = new string(encryptedAlphabetArr);

            for (int i = 0; i <= encryptedTextArr.Length - 1; i++)
            {
                if (Alphabet.IndexOf(encryptedTextArr[i]) != -1)
                {
                    encryptedTextArr[i] = Alphabet[encryptedAlphabet.IndexOf(encryptedTextArr[i])];
                }
            }

            return new string(encryptedTextArr);
        }
    }
}
