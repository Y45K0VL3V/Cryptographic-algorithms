using System;
using System.Collections.Generic;
using System.Text;

namespace yakov.TI.Lab1.Encryption
{
    /// <summary>
    /// Encrypt english text. 
    /// </summary>
    public static class ColumnarImprovedMethod
    {
        /// <summary>
        /// Encrypt entered text
        /// </summary>
        /// <param name="inputText"> Text to encrypt</param>
        public static string Encrypt(string inputText, string keyWord)
        {
            inputText = inputText.ToUpper();
            var columnsRead = GetColumnsOrder(keyWord);

            var inputTextTable = new char[keyWord.Length][];
            for (int i = 0; i <= keyWord.Length; i++)
            {
                inputTextTable[i] = new char[columnsRead[i]];
            }

            int currCharIndex = 0;
            for (int i = 0; i <= keyWord.Length; i++)
            {
                for (int j = 0; j <= columnsRead[i] - 1; j++)
                {
                    inputTextTable[i][j] = inputText[currCharIndex++];
                    if (currCharIndex >= keyWord.Length)
                    {
                        break;
                    }
                }
            }

            StringBuilder resultString = new();

            foreach (int j in columnsRead)
            {
                for (int i = 0; i <= keyWord.Length; i++)
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(inputTextTable[i][j].ToString()))
                        {
                            resultString.Append(inputTextTable[i][j]);
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
            }

            return resultString.ToString();
        }

        private static int[] GetColumnsOrder(string word)
        {
            var columnsRead = new int[word.Length];
            int columnCount = 0;
            while (columnCount <= columnsRead.Length)
            {
                for (char currChar = 'A'; currChar <= 'Z'; currChar++)
                {
                    int prevIndex = word.IndexOf(currChar);
                    while (prevIndex != -1)
                    {
                        prevIndex = word.IndexOf(currChar, prevIndex + 1);
                        columnsRead[columnCount++] = prevIndex + 1;
                    }
                }
            }

            return columnsRead;
        }
    }
}
