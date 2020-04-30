using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.LZW
{
    public class LZW
    {
        private Dictionary<string, int> compress = new Dictionary<string, int>();
        public double bytes_compression, bytes_original;
        //Llenar diccionario.
        public void dictionary_initial(FileStream file)
        {
            //var split = Nombre.Split('.');
            // bytes_original = file.ContentLength;
            List<int> initial_list = new List<int>();
            Dictionary<string, int> Base = new Dictionary<string, int>();
            var length = 0;
            //Leer archivo.
            //Stream stream = file.InputStream;
            length = 1000;
        }
    }
}
