using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.ARBOL_B_DISCO
{
    public class ArbolBusqueda
    {
        #region Definiciones
        public int grade;
        public Delegate node;
        public Delegate string_;
        public string path;
        public int key;
        #endregion
        private static ArbolBusqueda _instance = null;
        public static ArbolBusqueda Instance
        {
            get
            {
                if (_instance == null) _instance = new ArbolBusqueda();
                return _instance;
            }
        }
    }
}
