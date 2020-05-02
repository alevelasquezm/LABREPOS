﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        private void LimpiarNodo(TextoFabrica<T> fabrica)
        {
            Hijos = new List<int>();

            for (int i = 0; i < Orden; i++)
            {
                Hijos.Add(Cambios.ApuntadorVacio);
            }

            Llaves = new List<string>();

            for (int i = 0; i < Orden - 1; i++)
            {
                Llaves.Add("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            }

            Datos = new List<T>();

            for (int i = 0; i < Orden - 1; i++)
            {
                Datos.Add(fabrica.FabricarNulo());
            }
        }
        internal NodoB(int orden, int posicion, int padre, TextoFabrica<T> fabrica)
        {
            if ((orden < Minimo) || (orden > Maximo)) { throw new ArgumentOutOfRangeException("orden"); }
            if (posicion < 0) { throw new ArgumentOutOfRangeException("posicion"); }
            Orden = orden; Posicion = posicion; Padre = padre;
            LimpiarNodo(fabrica);
        }
        private int CalcularPosicionEnDisco(int tamañoEncabezado)
        {
            return tamañoEncabezado + (Posicion * TamañoEnBytes);
        }
        private string ConvertirATextoTamañoFijo()
        {
            StringBuilder datosCadena = new StringBuilder();
            datosCadena.Append(Cambios.FormatearEntero(Posicion));
            datosCadena.Append(Cambios.TextoSeparador);
            datosCadena.Append(Cambios.FormatearEntero(Padre));
            datosCadena.Append(Cambios.TextoSeparador);
            datosCadena.Append(Cambios.TextoSeparador);
            datosCadena.Append(Cambios.TextoSeparador);

            for (int i = 0; i < Hijos.Count; i++)
            {
                datosCadena.Append(Cambios.FormatearEntero(Hijos[i]));
                datosCadena.Append(Cambios.TextoSeparador);
            }

            datosCadena.Append(Cambios.TextoSeparador);
            datosCadena.Append(Cambios.TextoSeparador);

            for (int i = 0; i < Llaves.Count; i++)
            {
                datosCadena.Append(Cambios.FormatearLlave(Llaves[i]));
                datosCadena.Append(Cambios.TextoSeparador);
            }
            datosCadena.Append(Cambios.TextoSeparador);
            datosCadena.Append(Cambios.TextoSeparador);

            for (int i = 0; i < Datos.Count; i++)
            {
                datosCadena.Append(Datos[i].ToFixedSizeString().Replace(Cambios.TextoSeparador, Cambios.TextoSustitutoSeparador));
                datosCadena.Append(Cambios.TextoSeparador);
            }
            datosCadena.Append(Cambios.TextoNuevaLinea);
            return datosCadena.ToString();
        }
        private byte[] ObtenerBytes()
        {
            byte[] datosBinarios = null;
            datosBinarios = Cambios.ConvertirBinarioYTexto(ConvertirATextoTamañoFijo());
            return datosBinarios;
        }
        internal static NodoB<T> LeerNodoDesdeDisco(FileStream archivo, int tamañoEncabezado, int orden, int posicion, TextoFabrica<T> fabrica)
        {
            if (archivo == null)
            {
                throw new ArgumentNullException("archivo");
            }
            if (tamañoEncabezado < 0)
            {
                throw new ArgumentOutOfRangeException("tamañoEncabezado");
            }
            if ((orden < Minimo) || (orden > Maximo))
            {
                throw new ArgumentOutOfRangeException("orden");
            }
            if (posicion < 0)
            {
                throw new ArgumentOutOfRangeException("posicion");
            }
            if (fabrica == null)
            {
                throw new ArgumentNullException("fabrica");
            }
            // Se crea un nodo nulo para poder acceder a las propiedades de tamaño calculadas sobre la instancia 

            // Dato de la instancia del nodo
            NodoB<T> nuevoNodo = new NodoB<T>(orden, posicion, 0, fabrica);
            // Se crea un buffer donde se almacenarán los bytes leidos 
            byte[] datosBinario = new byte[nuevoNodo.TamañoEnBytes];
            // Variables a ser utilizadas luego de que el archivo sea leido 
            string datosCadena = "";
            string[] datosSeparados = null;
            int PosicionEnDatosCadena = 1;
            // Se ubica la posición donde deberá estar el nodo y se lee desde el archivo 
            archivo.Seek(nuevoNodo.CalcularPosicionEnDisco(tamañoEncabezado), SeekOrigin.Begin);
            archivo.Read(datosBinario, 0, nuevoNodo.TamañoEnBytes);
            // Se convierten los bytes leidos del archivo a una cadena 
            datosCadena = Cambios.ConvertirBinarioYTexto(datosBinario);
            // Se quitan los saltos de línea y se separa en secciones 
            datosCadena = datosCadena.Replace(Cambios.TextoNuevaLinea, "");
            datosCadena = datosCadena.Replace("".PadRight(3, Cambios.TextoSeparador), Cambios.TextoSeparador.ToString());
            datosSeparados = datosCadena.Split(Cambios.TextoSeparador);
            // Se obtiene la posición del Padre 
            nuevoNodo.Padre = Convert.ToInt32(datosSeparados[PosicionEnDatosCadena]);
            PosicionEnDatosCadena++;
            // Se asignan al nodo vacío los hijos desde la cadena separada 
            for (int i = 0; i < nuevoNodo.Hijos.Count; i++)
            {
                nuevoNodo.Hijos[i] = Convert.ToInt32(datosSeparados[PosicionEnDatosCadena]);
                PosicionEnDatosCadena++;
            }
            // Se asignan al nodo vacío las llaves desde la cadena separada 
            for (int i = 0; i < nuevoNodo.Llaves.Count; i++)
            {
                nuevoNodo.Llaves[i] = datosSeparados[PosicionEnDatosCadena];
                PosicionEnDatosCadena++;
            }
            // Se asignan al nodo vacío los datos la cadena separada 
            for (int i = 0; i < nuevoNodo.Datos.Count; i++)
            {
                datosSeparados[PosicionEnDatosCadena] = datosSeparados[PosicionEnDatosCadena].Replace(Cambios.TextoSustitutoSeparador, Cambios.TextoSeparador);
                nuevoNodo.Datos[i] = fabrica.Fabricar(datosSeparados[PosicionEnDatosCadena]);
                PosicionEnDatosCadena++;
            }
            // Se retorna el nodo luego de agregar toda la información 
            return nuevoNodo;
        }
        internal void GuardarNodoEnDisco(FileStream archivo, int tamañoEncabezado)
        {
            // Se ubica la posición donde se debe escribir 
            archivo.Seek(CalcularPosicionEnDisco(tamañoEncabezado), SeekOrigin.Begin);
            // Se escribe al archivo y se fuerza a vaciar el buffer 
            archivo.Write(ObtenerBytes(), 0, TamañoEnBytes);
            archivo.Flush();
        }
        internal void LimpiarNodoEnDisco(FileStream archivo, int tamañoEncabezado, TextoFabrica<T> fabrica)
        {
            // Se limpia el contenido del nodo 
            LimpiarNodo(fabrica);
            // Se guarda en disco el objeto que ha sido limpiado 
            GuardarNodoEnDisco(archivo, tamañoEncabezado);
        }

    }
}