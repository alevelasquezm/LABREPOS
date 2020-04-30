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
            List<int> initial_list = new List<int>();
            Dictionary<string, int> Base = new Dictionary<string, int>();
            var length = 0;
            //Leer archivo.
            length = 1000;
            //Lee numeros binarios.
            using (var reader = new BinaryReader(file))
            {
                var bytes = new byte[length];
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    //Leer bytes.
                    bytes = reader.ReadBytes(length);
                    for (int x = 0; x < bytes.Length; x++)
                    {
                        //Lista inicial contiene la informacion.
                        if (!initial_list.Contains(bytes[x]))
                        {
                            initial_list.Add(bytes[x]);
                            bytes_compression++;
                        }
                    }
                }
            }
            initial_list.Sort();
            var position = 0;
            //Chequeo de la posicion.
            foreach (var item in initial_list)
            {
                Base.Add(item.ToString(), position);
                position++;
            }
            foreach (var item in compress)
            {
                compress.Add(item.Key, item.Value);
            }
        }
    }
}
