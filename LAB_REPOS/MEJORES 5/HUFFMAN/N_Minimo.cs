using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.HUFFMAN
{
    public class N_Minimo
    {
        public byte letter;
        public int left, right;
        public N_Minimo() { }
        public N_Minimo(byte l, int le, int ri)
        {
            this.letter = l;
            this.left = le;
            this.right = ri;
        }
    }
}
