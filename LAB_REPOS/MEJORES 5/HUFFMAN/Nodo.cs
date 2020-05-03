using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.HUFFMAN
{
    public class Nodo : IComparable<Nodo>
    {
        #region Definiciones
        public int id;
        public bool isLetter;
        public byte letter;
        public int frequency;
        public Nodo left1, right1;
        public Nodo() { }
        #endregion
        public Nodo(byte letter1, int frequency1, Nodo lef, Nodo rig, bool isLetter1)
        {
            this.letter = letter1;
            this.frequency = frequency1;
            this.left1 = lef;
            this.right1 = rig;
            this.isLetter = isLetter1;
        }
        public int CompareTo(Nodo other)
        {
            return (this.frequency > other.frequency) ? -1 : ((this.frequency == other.frequency) ? 0 : 1);
        }

    }
}
