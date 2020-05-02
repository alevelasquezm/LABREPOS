using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.ARBOL_B_DISCO
{
    public class NodoB<T> where T : TextoFijo
    {
        #region Definiciones
        public const int Minimo = 3;
        public const int Maximo = 99;
        internal int Orden { get; private set; }
        internal int Posicion { get; private set; }
        internal int Padre { get; set; }
        internal List<int> Hijos { get; set; }
        internal List<string> Llaves { get; set; }
        internal List<T> Datos { get; set; }
        #endregion
        internal int CantidadDatos
        {
            get
            {
                int i = 0;
                while (i < Llaves.Count && Llaves[i] != "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")
                {
                    i++;
                }
                return i;
            }
        }
        internal bool Underflow
        {
            get
            {
                return (CantidadDatos < ((Orden / 2) - 1));
            }
        }
        internal bool Lleno
        {
            get
            {
                return (CantidadDatos >= Orden - 1);
            }
        }
        internal bool EsHoja
        {
            get
            {
                bool EsHoja = true;
                for (int i = 0; i < Hijos.Count; i++)
                {
                    if (Hijos[i] != Cambios.ApuntadorVacio)
                    {
                        EsHoja = false;
                        break;
                    }
                }
                return EsHoja;
            }
        }
        internal int TamañoEnTexto
        {
            get
            {
                int tamañoEnTexto = 0;
                tamañoEnTexto += Cambios.TextoEnteroTamaño + 1; // Tamaño del indicador de posición 
                tamañoEnTexto += Cambios.TextoEnteroTamaño + 1; // Tamaño del apuntador al padre 
                tamañoEnTexto += 2; // Separadores extras
                tamañoEnTexto += (Cambios.TextoEnteroTamaño + 1) * Orden; // Tamaño de los hijos 
                tamañoEnTexto += 2; // Separadores extras 
                tamañoEnTexto += (Cambios.TextoLlaveTamaño + 1) * (Orden - 1); // Tamaño de las llaves 
                tamañoEnTexto += 2; // Separadores extras
                tamañoEnTexto += (Datos[0].FixedSizeText + 1) * (Orden - 1); // Tamaño de los datos 
                tamañoEnTexto += Cambios.TextoNuevaLineaTamaño; // Tamaño del enter 
                return tamañoEnTexto;
            }
        }
        internal int TamañoEnBytes
        {
            get
            {
                return TamañoEnTexto * Cambios.BinarioCaracterTamaño;
            }
        }
        internal NodoB(int orden, int posicion, int padre, TextoFabrica<T> fabrica)
        {
            if ((orden < Minimo) || (orden > Maximo)) { throw new ArgumentOutOfRangeException("orden"); }
            if (posicion < 0) { throw new ArgumentOutOfRangeException("posicion"); }
            Orden = orden; Posicion = posicion; Padre = padre;
            LimpiarNodo(fabrica);
        }
    }
}
