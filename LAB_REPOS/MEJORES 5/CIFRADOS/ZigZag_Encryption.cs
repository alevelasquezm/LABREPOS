using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;


namespace LAB_REPOS.MEJORES_5.CIFRADOS
{
    public class ZigZag_Encryption
    {
        //Lectura para el cifrado.
        public List<byte> encryption(string file, int length)
        {
            //Lista para bytes.
            var list = new List<byte>();
            using (var stream = new FileStream(file, FileMode.Open))
            {
                //Lectura.
                using (var reading = new BinaryReader(stream))
                {
                    //Almacenamiento.
                    var bytes = new byte[length];
                    while (reading.BaseStream.Position != reading.BaseStream.Length)
                    {
                        //Lee los bytes.
                        bytes = reading.ReadBytes(length);
                        foreach (byte bit in bytes)
                        {
                            //Agregando a la lista.
                            list.Add(bit);
                        }
                    }
                }
            }
            return list;
        }
        //Crear matriz.
        public byte[,] matrix(int counter, int level, ref int caracter)
        {
            caracter = counter;
            var array = new byte[level, counter];
            var return_ = false;
            var x = 0;
            for (int y = 0; y < counter; y++)
            {
                if (return_)
                {
                    //Convertir en byte.
                    array[x, y] = Convert.ToByte('_');
                    x--;
                    if (x < 0)
                    {
                        return_ = false;
                        x += 2;
                    }
                }
                else
                {
                    array[x, y] = Convert.ToByte('_');
                    x++;
                    if (x == level)
                    {
                        return_ = true;
                        x -= 2;
                    }
                }
            }
            //Si convierte a byte, que devuelva el array.
            if (array[0, counter - 1] == Convert.ToByte('_'))
            {
                return array;
            }
            else
            {
                array = matrix(counter + 1, level, ref caracter);
            }
            return array;
        }
        //Añadir cualquier tipo de caracter extra.
        public List<byte> add_extra_c(List<byte> list, int counter, ref byte extra_c)
        {
            bool found = true;
            var x = 1;
            while (!found)
            {
                if (list.Contains(Convert.ToByte(x)))
                {
                    x++;
                }
                else
                {
                    //Encuentra el valor.
                    found = true;
                }
            }
            extra_c = Convert.ToByte(x);
            while (list.Count() != counter)
            {
                //Se añade al listado.
                list.Add(Convert.ToByte(x));
            }
            return list;
        }
        //Cifrar mensaje.
        public void message(byte[,] array, int level, string route, List<byte> list, byte extra_c)
        {
            //Introduce los bytes.
            var list_position = 0;
            var return_ = false;
            var x = 0;
            for (int y = 0; y < list.Count(); y++)
            {
                if (return_)
                {
                    array[x, y] = list[list_position];
                    list_position++;
                    x--;
                    if (x < 0)
                    {
                        return_ = false;
                        x += 2;
                    }
                }
                else
                {
                    array[x, y] = list[list_position];
                    list_position++;
                    x++;
                    if (x == level)
                    {
                        return_ = true;
                        x -= 2;
                    }
                }
            }
            //Obtiene los bytes cifrados.
            var bytes = new byte[list.Count()];
            var position = 0;
            for (x = 0; x < level; x++)
            {
                for (int y = 0; y < list.Count(); y++)
                {
                    if (array[x, y] != 0)
                    {
                        bytes[position] = array[x, y];
                        position++;
                    }
                }
            }
            using (var stream = new FileStream(route + "Cifrados", FileMode.Create))
            {
                using (var writing = new BinaryWriter(stream))
                {
                    writing.Write(extra_c);
                    writing.Seek(0, SeekOrigin.End);
                    writing.Write(bytes);
                }
            }
        }
        //Lectura para el descifrado.
        public List<byte> decryption(string file, int length, ref byte extra_c)
        {
            //Lista para bytes.
            var list = new List<byte>();
            using (var stream = new FileStream(file, FileMode.Open))
            {
                //Lectura.
                using (var reading = new BinaryReader(stream))
                {
                    //Almacenamiento.
                    var counter = 0;
                    var bytes = new byte[length];
                    while (reading.BaseStream.Position != reading.BaseStream.Length)
                    {
                        //Lee los bytes.
                        bytes = reading.ReadBytes(length);
                        foreach (byte bit in bytes)
                        {
                            bytes = reading.ReadBytes(length);
                            if (counter != 0)
                            {
                                //Agregando a la lista.
                                list.Add(bit);
                            }
                            else
                            {
                                extra_c = bit;
                                counter++;
                            }
                        }
                    }
                }
            }
            return list;
        }
        //Crear matriz descifrado.
        public byte[,] matrix_dec(int counter, int level, ref int caracter)
        {
            caracter = counter;
            var array = new byte[level, counter];
            var return_ = false;
            var x = 0;
            for (int y = 0; y < counter; y++)
            {
                if (return_)
                {
                    //Convertir en byte.
                    array[x, y] = Convert.ToByte('_');
                    x--;
                    if (x < 0)
                    {
                        return_ = false;
                        x += 2;
                    }
                }
                else
                {
                    array[x, y] = Convert.ToByte('_');
                    x++;
                    if (x == level)
                    {
                        return_ = true;
                        x -= 2;
                    }
                }
            }
            //Si convierte a byte, que devuelva el array.
            if (array[0, counter - 1] == Convert.ToByte('_'))
            {
                return array;
            }
            else
            {
                array = matrix_dec(counter + 1, level, ref caracter);
            }
            return array;
        }
        //Descifrar mensaje.
        public void message_dec(string route, int level, List<byte> list, byte[,] m, byte extra_c)
        {
            var n = level - 2;
            var m_ = (list.Count() + 2 * n + 1) / (2 + 2 * n);
            var less = m_ - 1;
            var middle = 2 * (m_ - 1);
            var position = 0;
            //Se introducen caracteres superiores.
            for (int x = 0; x < list.Count(); x++)
            {
                if (m[0, x] == Convert.ToByte('_'))
                {
                    m[0, x] = list[position];
                    position++;
                }
            }
            //Se introducen caracteres inferiores.
            var last = string.Empty;
            var y = 0;
            var counter_ = list.Count() - 1;
            while (y != less)
            {
                last = (char)list[counter_] + last;
                counter_--;
                y++;
            }
            var pos = 0;
            for (int x = 0; x < list.Count(); x++)
            {
                if (m[level - 1, x] == Convert.ToByte('_'))
                {
                    m[level - 1, x] = (byte)last[pos];
                    pos++;
                }
            }
            //Se introducen caracteres del medio.
            for (int x = 1; x < level - 1; x++)
            {
                for (int a = 0; a < list.Count(); a++)
                {
                    if ((m[x, a] == Convert.ToByte('_')))
                    {
                        m[x, a] = list[position];
                        position++;
                    }
                }
            }
            //Se recorre la matriz para mostrar los caracteres.
            var return_ = false;
            var z = 0;
            //var text = string.Empty;
            byte[] b = new byte[list.Count()];
            var put = 0;
            for (int x = 0; x < list.Count(); x++)
            {
                if (return_)
                {
                    if (m[z, x] != extra_c)
                    {
                        b[put] = m[z, x];
                        put++;
                    }
                    z--;
                    if (z < 0)
                    {
                        return_ = false;
                        z += 2;
                    }
                }
                else
                {
                    if (m[z, x] != extra_c)
                    {
                        b[put] = m[z, x];
                        put++;
                    }
                    z++;
                    if (z == level)
                    {
                        return_ = true;
                        z -= 2;
                    }
                }
            }
            using (var stream = new FileStream(route + "Descifrado", FileMode.Create))
            {
                using (var writing = new BinaryWriter(stream))
                {
                    writing.Write(b);
                }
            }
        }
    }
}

