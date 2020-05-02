using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.ARBOL_B_DISCO
{
    public abstract class ArbolBusqueda<TLlave, T> where TLlave : IComparable
    {
        public int Tamaño { get; protected set; }
        public abstract void Agregar(TLlave llave, T dato, string llaveAux);
        public abstract void Eliminar(TLlave llave);
        public abstract T Obtener(TLlave llave);
        public abstract bool Contiene(TLlave llave);
        public abstract string RecorrerInOrden();
        public abstract int ObtenerAltura();
        public abstract void Cerrar();
    }
}
