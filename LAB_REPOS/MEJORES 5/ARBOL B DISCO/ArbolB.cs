using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.ARBOL_B_DISCO
{
    public class ArbolB<T> : ArbolBusqueda<string, T> where T : TextoFijo
    {
        #region Atributos 
        // Tamaño total del encabezado 
        private const int _tamañoEncabezadoBinario = 5 * Cambios.EnteroYEnterBinarioTamaño;
        // Atributos en el encabezado del archivo 
        private int _raiz;
        private int _ultimaPosicionLibre;
        // Otras variables para acceso al archivo 
        private FileStream _archivo = null;
        private string _archivoNombre = "";
        private TextoFabrica<T> _fabrica = null;
        // El grado del árbol, este es asignado en el momento de la creación del árbol y no puede cambiarse posteriormente 
        public int Orden { get; private set; }
        public int Altura { get; private set; }

        public List<string> datos = new List<string>();
        #endregion
        public ArbolB(int orden, string nombreArchivo, TextoFabrica<T> fabrica)
        {
            // Se guardan los parámetros recibidos 
            _archivoNombre = nombreArchivo;
            _fabrica = fabrica;
            // Se abre la conexión al archivo 
            _archivo = new FileStream(_archivoNombre, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            // Se obtienen los valores del encabezado del archivo 
            _raiz = Cambios.LeerEntero(_archivo, 0);
            _ultimaPosicionLibre = Cambios.LeerEntero(_archivo, 1);
            Tamaño = Cambios.LeerEntero(_archivo, 2);
            Orden = Cambios.LeerEntero(_archivo, 3);
            Altura = Cambios.LeerEntero(_archivo, 4);
            // Se corrigen los valores del encabezado cuando el archivos no existe previamente 
            if (_ultimaPosicionLibre == Cambios.ApuntadorVacio)
            {
                _ultimaPosicionLibre = 0;
            }
            if (Tamaño == Cambios.ApuntadorVacio)
            {
                Tamaño = 0;
            }
            if (Orden == Cambios.ApuntadorVacio)
            {
                Orden = orden;
            }
            if (Altura == Cambios.ApuntadorVacio)
            {
                Altura = 1;
            }
            if (_raiz == Cambios.ApuntadorVacio)
            {
                // Se crea la cabeza del árbol vacía para evitar futuros errores 
                NodoB<T> nodoCabeza = new NodoB<T>(Orden, _ultimaPosicionLibre, Cambios.ApuntadorVacio, _fabrica);
                _ultimaPosicionLibre++;
                _raiz = nodoCabeza.Posicion;
                nodoCabeza.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
            }
            // Si el archivo existe solamente se actualizan los encabezados, sino se crea y luego se almacenan los valores iniciales 
            GuardarEncabezado();
        }
        private void GuardarEncabezado()
        {
            // Se escribe a disco 
            Cambios.EscribirEntero(_archivo, 0, _raiz);
            Cambios.EscribirEntero(_archivo, 1, _ultimaPosicionLibre);
            Cambios.EscribirEntero(_archivo, 2, Tamaño);
            Cambios.EscribirEntero(_archivo, 3, Orden);
            Cambios.EscribirEntero(_archivo, 4, Altura);
            _archivo.Flush();
        }
        private void AgregarRecursivo(int posicionNodoActual, string llave, T dato)
        {
            NodoB<T> nodoActual = NodoB<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, posicionNodoActual, _fabrica);
            if (nodoActual.PosicionExactaEnNodo(llave) != -1)
            {
                throw new InvalidOperationException("La llave indicada ya está contenida en el árbol.");
            }
            if (nodoActual.EsHoja)
            {
                // Se debe insertar en este nodo, por lo que se hace la llamada al método encargado de insertar y ajustar el árbol si es necesario 
                Subir(nodoActual, llave, dato, Cambios.ApuntadorVacio);
                GuardarEncabezado();
            }
            else
            {
                // Se hace una llamada recursiva, bajando en el subarbol correspondiente según la posición aproximada de la llave 
                AgregarRecursivo(nodoActual.Hijos[nodoActual.PosicionAproximadaEnNodo(llave)], llave, dato);
            }
        }
        private void Subir(NodoB<T> nodoActual, string llave, T dato, int hijoDerecho)
        {
            // Si el nodo no está lleno, se agrega la información al nodo y el método termina 
            if (!nodoActual.Lleno)
            {
                nodoActual.AgregarDato(llave, dato, hijoDerecho);
                nodoActual.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                return;
            }
            // Creo un nuevo nodo hermano 
            NodoB<T> nuevoHermano = new NodoB<T>(Orden, _ultimaPosicionLibre, nodoActual.Padre, _fabrica);
            _ultimaPosicionLibre++;
            // Datos a subir al padre luego de la separación 
            string llavePorSubir = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            T datoPorSubir = _fabrica.FabricarNulo();
            // Se llama al método que hace la separación  
            nodoActual.SepararNodo(llave, dato, hijoDerecho, nuevoHermano, ref llavePorSubir, ref datoPorSubir);
            // Actualizar el apuntador en todos los hijos 
            NodoB<T> nodoHijo = null;
            for (int i = 0; i < nuevoHermano.Hijos.Count; i++)
            {
                if (nuevoHermano.Hijos[i] != Cambios.ApuntadorVacio)
                {
                    // Se carga el hijo para modificar su apuntador al padre 
                    nodoHijo = NodoB<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, nuevoHermano.Hijos[i], _fabrica);
                    nodoHijo.Padre = nuevoHermano.Posicion;
                    nodoHijo.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                }
                else
                {
                    break;
                }
            }
            // Evaluo el caso del padre 
            if (nodoActual.Padre == Cambios.ApuntadorVacio) // Es la raiz 
            {
                // Creo un nuevo nodo Raiz 
                NodoB<T> nuevaRaiz = new NodoB<T>(Orden, _ultimaPosicionLibre, Cambios.ApuntadorVacio, _fabrica);
                _ultimaPosicionLibre++;
                Altura++;
                // Agrego la información 
                nuevaRaiz.Hijos[0] = nodoActual.Posicion;
                nuevaRaiz.AgregarDato(llavePorSubir, datoPorSubir, nuevoHermano.Posicion);
                // Actualizo los apuntadores al padre 
                nodoActual.Padre = nuevaRaiz.Posicion;
                nuevoHermano.Padre = nuevaRaiz.Posicion;
                _raiz = nuevaRaiz.Posicion;
                // Guardo los cambios 
                nuevaRaiz.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                nodoActual.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                nuevoHermano.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
            }
            else // No es la raiz 
            {
                nodoActual.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                nuevoHermano.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                // Cargar el nodo padre 
                NodoB<T> nodoPadre = NodoB<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, nodoActual.Padre, _fabrica);
                Subir(nodoPadre, llavePorSubir, datoPorSubir, nuevoHermano.Posicion);
            }
        }
        private NodoB<T> ObtenerRecursivo(int posicionNodoActual, string llave, out int posicion)
        {
            NodoB<T> nodoActual = NodoB<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, posicionNodoActual, _fabrica);
            posicion = nodoActual.PosicionExactaEnNodo(llave);
            if (posicion != -1)
            {
                return nodoActual;
            }
            else
            {
                if (nodoActual.EsHoja)
                {
                    return null;
                }
                else
                {
                    int posicionAproximada = nodoActual.PosicionAproximadaEnNodo(llave);
                    return ObtenerRecursivo(nodoActual.Hijos[posicionAproximada], llave, out posicion);
                }
            }
        }
        public override void Agregar(string llave, T dato, string llaveAux)
        {
            try
            {
                if (llave == "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")
                {
                    throw new ArgumentOutOfRangeException("llave");
                }

                llave = llave + llaveAux;
                AgregarRecursivo(_raiz, llave, dato);
                Tamaño++;
            }
            catch (Exception)
            {

            }
        }
    }
}
