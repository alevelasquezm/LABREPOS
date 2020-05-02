using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.ARBOL_B_DISCO
{
    public class NodoB<T>
    {
        #region Definiciones
        public int index;
        public int Father;
        public int cant;
        public List<int> Sons = new List<int>();
        public List<T> Values = new List<T>();
        static int lenght = 500;
        #endregion
        public NodoB(int father_)
        {
            if (father_ != 0)
            {
                cant = ArbolBusqueda.Instance.grade - 1;
            }
            else
            {
                cant = (4 * (ArbolBusqueda.Instance.grade - 1) / 3);
            }
            this.Father = father_;
        }
        static string bufferToString(byte[] line)
        {
            var auxiliar = string.Empty;
            foreach (var character in line)
            {
                auxiliar += (char)character;
            }
            return auxiliar;
        }
        static byte[] stringToBuffer(string line2)
        {
            var auxiliar2 = new List<byte>();
            foreach (var character2 in line2)
            {
                auxiliar2.Add((byte)character2);
            }
            return auxiliar2.ToArray();
        }
        //Nos permite convertir la informacion de string a un nodo normal.
        public static NodoB<T> StringToNodo(int position)
        {
            var cant_sons = ((4 * (ArbolBusqueda.Instance.grade - 1)) / 3) + 1;
            var cant_characters = 8 + (4 * cant_sons) + (lenght * (cant_sons - 1));

            //Leer la linea de archivo de texto que contiene el nodo
            var buffer_l = new byte[cant_characters];
            using (var files = new FileStream(ArbolBusqueda.Instance.path, FileMode.OpenOrCreate))
            {
                files.Seek((position - 1) * cant_characters + 15, SeekOrigin.Begin);
                files.Read(buffer_l, 0, cant_characters);
            }
            var nodeString = bufferToString(buffer_l);
            //Dividir los valores para llenar el nodo que se va a utilizar
            var values = new List<string>();
            for (int i = 0; i < cant_sons + 2; i++)
            {
                values.Add(nodeString.Substring(0, 4));
                nodeString = nodeString.Substring(4);
            }
            for (int i = 0; i < cant_sons - 1; i++)
            {
                values.Add(nodeString.Substring(0, lenght));
                nodeString = nodeString.Substring(lenght);
            }
            var node_out = new NodoB<T>(Convert.ToInt32(values[1]));
            node_out.index = Convert.ToInt32(values[0]);
            for (int i = 2; i < (2 + cant_sons); i++)
            {
                if (values[i].Trim() != "-")
                {
                    node_out.Sons.Add(Convert.ToInt32(values[i]));
                }
            }
            for (int i = (2 + cant_sons); i < (1 + (2 * cant_sons)); i++)
            {
                if (values[i].Trim() != "-")
                {
                    node_out.Values.Add((T)ArbolBusqueda.Instance.node.DynamicInvoke(values[i]));
                }
            }
            return node_out;
        }
        //Nos permite convertir la informacion de nodo normal a string.
        public void NodoToString()
        {
            string sons = string.Empty;
            string data = string.Empty;
            var cant_sons2 = ((4 * (ArbolBusqueda.Instance.grade - 1)) / 3) + 1;
            foreach (var item in Sons)
            {
                sons += item.ToString("0000;-0000");
            }
            for (int i = Sons.Count; i < cant_sons2; i++)
            {
                sons += string.Format("{0,-4}", "-");
            }
            foreach (var item in Values)
            {
                data += Convert.ToString(ArbolBusqueda.Instance.string_.DynamicInvoke(item));
            }
            for (int i = Values.Count; i < (cant_sons2 - 1); i++)
            {
                data += string.Format("{0,-500}", "-");
            }
            var p = data.Length;
            var node_char = $"{index.ToString("0000;-0000")}{Father.ToString("0000;-0000")}{sons}{data}";
            var cant_characters2 = 8 + (4 * cant_sons2) + (lenght * (cant_sons2 - 1));
            using (var file_s2 = new FileStream(ArbolBusqueda.Instance.path, FileMode.OpenOrCreate))
            {
                file_s2.Seek((index - 1) * cant_characters2 + 15, SeekOrigin.Begin);
                file_s2.Write(stringToBuffer(node_char), 0, cant_characters2);
            }
        }
    }
}
