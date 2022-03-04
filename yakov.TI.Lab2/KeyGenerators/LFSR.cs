using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.TI.Lab2.KeyGenerators
{
    public class LFSR
    {
        public LFSR(byte registerLength, string polynom)
        {
            RegisterLength = registerLength;
            Polynom = polynom;
        }

        public byte RegisterLength { get; set; }
        public string Polynom { get; set; }
    }
}
