using System;
using System.Text;
using System.Text.RegularExpressions;

namespace yakov.TI.Lab1.Crypt
{
    /// <summary>
    /// Class to encrypt/decrypt english text with
    /// columnar improved method. 
    /// </summary>
    public class ColumnarImprovedEncryption
    {
        private static int RowsAmount { get; set; }

        /// <summary>
        /// Encrypt entered text
        /// </summary>
        /// <param name="inputText"> Text to encrypt</param>
        public static string Encrypt(string inputText, string keyWord)
        {
            inputText = GetValidChars(inputText?.ToUpper()??"");
            keyWord = GetValidChars(keyWord?.ToUpper());

            var columnsRead = GetColumnsOrder(keyWord);
            var inputTextTable = FillTextTable(keyWord, inputText, columnsRead);

            StringBuilder encryptedString = new();

            foreach (int j in columnsRead)
            {
                for (int i = 0; i <= RowsAmount - 1; i++)
                {
                    try
                    {
                        if (inputTextTable[i][j-1] != '\0')
                        {
                            encryptedString.Append(inputTextTable[i][j-1]);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return encryptedString.ToString();
        }

        public static string Decrypt(string encryptedText, string keyWord)
        {
            encryptedText = GetValidChars(encryptedText?.ToUpper()??"");
            keyWord = GetValidChars(keyWord?.ToUpper()??"");

            var columnsRead = GetColumnsOrder(keyWord);
            var testTable = FillTextTable(keyWord, new string('a',encryptedText.Length), columnsRead);
            var columnsCharAmount = GetColumnCharAmount(testTable, keyWord.Length);

            int decryptedTextIndex = 0;
            for (int j = 0; j <= keyWord.Length - 1; j++)
            {
                int verticalShift = 0;
                for (int i = 0; i <= columnsCharAmount[columnsRead[j] - 1]-1;)
                {
                    try
                    {
                        testTable[i+verticalShift][columnsRead[j] - 1] = encryptedText[decryptedTextIndex];
                        i++;
                        decryptedTextIndex++;
                    }
                    catch
                    {
                        verticalShift++;
                        continue;
                    }
                }
            }

            StringBuilder decryptedString = new();

            for (int i = 0; i <= RowsAmount - 1; i++)
            {
                foreach(char colChar in testTable[i])
                {
                    if (colChar != '\0')
                    {
                        decryptedString.Append(colChar);
                    }
                    else
                    {
                        return decryptedString.ToString();
                    }
                }
            }

            return decryptedString.ToString();
        }

        private static int[] GetColumnCharAmount(char[][] textTable, int columnsAmount)
        {
            var columnCharAmount = new int[columnsAmount];
            for (int j = 0; j <= columnsAmount - 1; j++)
            {
                for (int i = 0; i <= RowsAmount; i++)
                {
                    try
                    {
                        if (textTable[i][j] != '\0')
                        {
                            columnCharAmount[j]++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return columnCharAmount;
        }

        public static string GetValidChars(string inputText)
        {
            StringBuilder sbBuffer = new();
            string validPattern = "[A-Z]+";

            foreach (Match validPart in Regex.Matches(inputText, validPattern))
            {
                sbBuffer.Append(validPart.Value);
            }

            return sbBuffer.ToString() ?? "";
        }

        private static char[][] FillTextTable(string keyWord, string inputText, int[] columnsRead)
        {
            RowsAmount = (int)(keyWord.Length * Math.Ceiling((double)inputText.Length / (((1 + keyWord.Length) * keyWord.Length) / 2)));
            var inputTextTable = new char[RowsAmount][];
            for (int i = 0; i <= RowsAmount - 1; i++)
            {
                inputTextTable[i] = new char[columnsRead[i % columnsRead.Length]];
            }

            int currCharIndex = 0;
            for (int i = 0; i <= RowsAmount - 1; i++)
            {
                for (int j = 0; j <= columnsRead[i % columnsRead.Length] - 1; j++)
                {
                    inputTextTable[i][j] = inputText[currCharIndex++];
                    if (currCharIndex >= inputText.Length)
                    {
                        return inputTextTable;
                    }
                }
            }

            return inputTextTable;
        }

        private static int[] GetColumnsOrder(string word)
        {
            var columnsRead = new int[word.Length];
            int columnCount = 0;
            while (columnCount <= columnsRead.Length-1)
            {
                for (char currChar = 'A'; currChar <= 'Z'; currChar++)
                {
                    int prevIndex = word.IndexOf(currChar);
                    while (prevIndex != -1)
                    {
                        columnsRead[columnCount++] = prevIndex + 1;
                        prevIndex = word.IndexOf(currChar, prevIndex + 1);
                    }
                }
            }

            return columnsRead;
        }
    }
}
