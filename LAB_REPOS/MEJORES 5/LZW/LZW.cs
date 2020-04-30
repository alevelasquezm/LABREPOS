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
        // Comprimir informacion.
        public string compact(int value, byte[] bytes, ref int position_counter, string last_position, dynamic writing)
        {
            while (value < bytes.Length)
            {
                // Posicion actual de la informacion.
                var actual_position = last_position + bytes[value].ToString();
                var c = 0;
                var p = compress.TryGetValue(actual_position, out c);
                if (p)
                {
                    // Buscar ultima posicion.
                    last_position += bytes[value].ToString();
                }
                else
                {
                    // Informacion comprimida.
                    compress.Add(actual_position, position_counter);
                    position_counter++;
                    var text = compress[last_position];
                    writing.Write($"{text}");
                    last_position = bytes[value].ToString();
                }
                value++;
            }
            return last_position;
        }
        // Proceso para la compresion.
        public void compression_process(FileStream stream, string t)
        {
            var length = 1000;
            var stream1 = new FileStream(t, FileMode.Open);
            var path = Path.GetFullPath("Comprimidos");
            using (var reading = new BinaryReader(stream1))
            {
                //  Abrir o crear archivo
                using (var writeStream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    // Escribir los bytes.
                    using (var writing = new BinaryWriter(writeStream))
                    {
                        var bytes = new byte[length];
                        var position_counter = compress.Count;
                        foreach (var item in compress)
                        {
                            writing.Write($"{item.Key}|{item.Value.ToString()}|");
                        }
                        writing.Write($"|");
                        var last = string.Empty;
                        while (reading.BaseStream.Position != reading.BaseStream.Length)
                        {
                            //Lectura de bytes.
                            bytes = reading.ReadBytes(length);
                            var a = 0;
                            var last_position = string.Empty;
                            if (last == string.Empty)
                            {
                                last_position = compact(a, bytes, ref position_counter, last_position, writing);
                            }
                            else
                            {
                                //  Informacion vacia.
                                last_position = last;
                                last = string.Empty;
                                last_position = compact(a, bytes, ref position_counter, last_position, writing);
                            }
                            //Detecta ultima posicion.
                            if (last_position != string.Empty)
                            {
                                last = last_position;
                            }
                        }
                        if (last != string.Empty)
                        {
                            //Dato a comprimir.
                            var v_write = compress[last];
                            writing.Write($"{v_write}");
                        }
                    }
                }
            }
        }
        //Codificador.
        public string coder(byte[] bytes, ref int x, ref bool r2, ref string r)
        {
            string code = string.Empty;
            if (r2)
            {
                code += r;
            }
            r2 = false;
            x++;
            for (; x < bytes.Length; x++)
            {
                if (int.TryParse(((char)bytes[x]).ToString(), out int n))
                {
                    code += (char)bytes[x];
                }
                else if (x == bytes.Length - 1)
                {
                    r2 = true;
                    r = code;
                }
                break;
            }
            return code;
        }
        //Descomprimir la informacion.
        public void descompression(string n)
        {
            int length = 1000;
            string n2 = "";
            for (int x = 0; x < n.Length; x++)
            {
                if (n[x] != '.')
                {
                    n2 += n[x];
                }
                else
                {
                    break;
                }
            }
            //Detectar el archivo comprimido.
            string f = n2 + ".lzw";
            string path = Path.GetFullPath(n);
            string p2 = Path.GetFullPath("Descomprimidos");
            var stream1 = new FileStream(n, FileMode.Open);
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reading = new BinaryReader(stream1))
                {
                    //Crear o abrir archivo.
                    using (var writeStream = new FileStream(p2, FileMode.OpenOrCreate))
                    {
                        using (var writing = new BinaryWriter(writeStream))
                        {
                            {
                                var bytes = new byte[length];
                                var dic = false;
                                // Manejar todos los prefijos.
                                Dictionary<int, string> prefixes = new Dictionary<int, string>();

                                while (!dic)
                                {
                                    // Posicion actual del diccionario.
                                    var actual_position = "";
                                    actual_position = reading.ReadString();
                                    if (actual_position[0] == '|')
                                    {
                                        dic = true;
                                    }
                                    else
                                    {
                                        //  Contenido de la informacion
                                        var content = actual_position.Split('|');
                                        prefixes.Add(Convert.ToInt32(content[1]), ((char)Convert.ToInt32(content[0])).ToString());
                                    }
                                }
                                string old, newest, val, r;
                                old = newest = val = r = string.Empty;
                                char first_val = ' ';
                                bool r2 = false;
                                bool first = true;
                                int index = prefixes.Count();
                                var index_b = 0;
                                string outer = string.Empty;

                                while (!r2)
                                {
                                    //  Largo de la cadena de bytes.
                                    if (index_b == bytes.Length - 1)
                                    {
                                        bytes = reading.ReadBytes(length);
                                    }
                                    if (first)
                                    {
                                        // Lee los bytes.
                                        bytes = reading.ReadBytes(length);
                                        old = coder(bytes, ref index, ref r2, ref r);
                                        first_val = Convert.ToChar(prefixes[(Convert.ToInt32(old))]);
                                        outer += first_val;
                                        writing.Write(Convert.ToChar((int)first_val));
                                        first = false;
                                    }
                                    else
                                    {
                                        //Obtener descodificacion.
                                        newest = coder(bytes, ref index, ref r2, ref r);
                                        if (newest == string.Empty)
                                        {
                                            break;
                                        }
                                        string outTryGet = string.Empty;
                                        var in_dic = prefixes.TryGetValue(Convert.ToInt32(newest), out outTryGet);
                                        //  Si se encuentra el valor en el diccionario.
                                        if (in_dic)
                                        {
                                            val = prefixes[(Convert.ToInt32(newest))];
                                        }
                                        else
                                        {
                                            val = prefixes[(Convert.ToInt32(old))];
                                            val += first_val;
                                        }
                                        outer += val;
                                        for (int a = 0; a < val.Length; a++)
                                        {
                                            writing.Write(Convert.ToChar((int)val[a]));
                                        }
                                        first_val = Convert.ToChar(val.Substring(0, 1));
                                        prefixes.Add(index, (prefixes[(Convert.ToInt32(old))]) + first_val);
                                        old = newest;
                                        index++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
