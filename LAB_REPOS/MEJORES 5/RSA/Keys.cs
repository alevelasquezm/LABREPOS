using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.RSA
{
    public class Keys
    {
        public int ValorP { get; set; }
        public int ValorQ { get; set; }
        public bool numeroPrimo(int num, int divisor)
        {
            if (num / 2 < divisor)
            {
                return true;
            }
            else
            {
                if (num % divisor == 0)
                {
                    return false;
                }
                else
                {
                    return numeroPrimo(num, divisor + 1);
                }
            }
        }
    }
}
