using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace yakov.TI.Lab2.KeyGenerators
{
    public class LFSR
    {
        public LFSR(byte registerLength, string polynom)
        {
            RegisterLength = registerLength;
            _registerMask = Convert.ToInt64(new string('1', registerLength), 2);

            _xorBitsIndex = ParsePolynom(polynom);
        }

        private const string _polynomParsingPattern = "(?<=\\^)\\d+(?=\\+)";

        // Get bits indexes, that will xor for key generating.
        private List<byte> ParsePolynom(string polynom)
        {
            List<byte> bitsIndex = new List<byte>();
            foreach (Match match in Regex.Matches(polynom, _polynomParsingPattern))
            {
                bitsIndex.Add(Convert.ToByte(match.Value));
            }

            return bitsIndex;
        }

        public byte RegisterLength { get; }
        public long RegisterState { get; set; }
        
        private readonly long _registerMask;
        private readonly List<byte> _xorBitsIndex;

        public void SetRegisterState(long state)
        {
            RegisterState = state;
        }

        public byte GetRandomByte()
        {
            byte result = 0;
            for(int i = 1; i <= 8; i++)
            {
                result <<= 1;
                result += Convert.ToByte((RegisterState >> RegisterLength - 1) & 1);
                byte nextBit = GetNextBit();
                RegisterState = (RegisterState << 1) & _registerMask;
                RegisterState += nextBit;
            }
            return result;
        }

        private byte GetNextBit()
        {
            byte nextBit = Convert.ToByte((RegisterState >> (_xorBitsIndex.FirstOrDefault() - 1)) & 1);
            for (int i = 1; i <= _xorBitsIndex.Count - 1; i++)
            {
                nextBit ^= Convert.ToByte((RegisterState >> (_xorBitsIndex[i] - 1)) & 1);
            }
            return nextBit;
        }
    }
}
