using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.ARBOL_B_DISCO
{
    public interface TextoFabrica<T> where T : TextoFijo
    {
        T Fabricar(string textoTamañoFijo);
        T FabricarNulo();
    }
}
