using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
namespace LAB_REPOS.MEJORES_5.HUFFMAN
{
    public class Compression
    {
        //Comprimir bytes.
        private static void compression_bytes(Nodo use_node, List<Nodo> list_node)
        {
            if (use_node == null)
            {
                return;
            }
            list_node.Add(use_node);
            compression_bytes(use_node.left1, list_node);
            compression_bytes(use_node.right1, list_node);
        }
        //Comprimir arbol.
        private static N_minimo[] tree_compression(Nodo node_root)
        {
            List<Nodo> ListadoDeNodos = new List<Nodo>();
            compression_bytes(node_root, ListadoDeNodos);
            for (int x = 0; x < ListadoDeNodos.Count; x++)
            {
                ListadoDeNodos[x].id = x;
            }
            N_minimo[] N_Result = new N_minimo[ListadoDeNodos.Count];
            for (int y = 0; y < N_Result.Length; y++)
            {
                if (ListadoDeNodos[y].frequency == 0)
                {
                    int LadoIzquierdo = -2;
                    int LadoDerecho = -2;
                    N_Result[y] = new N_minimo(ListadoDeNodos[y].letter, LadoIzquierdo, LadoDerecho);
                }
                else
                {
                    int left = (ListadoDeNodos[y].left1 == null) ? -1 : ListadoDeNodos[y].left1.id;
                    int right = (ListadoDeNodos[y].right1 == null) ? -1 : ListadoDeNodos[y].right1.id;
                    N_Result[y] = new N_minimo(ListadoDeNodos[y].letter, left, right);
                }
            }
            return N_Result;
        }
        //Conversion del arbol.
        private static byte[] tree_conversion(Nodo NodoRaizArbol)
        {
            List<byte> ListadoDeBytes = new List<byte>();

            N_minimo[] minimoNodos = tree_compression(NodoRaizArbol);

            byte[] CapacidadDelByte = BitConverter.GetBytes(minimoNodos.Length);

            ListadoDeBytes.AddRange(CapacidadDelByte);

            foreach (N_minimo value in minimoNodos)
            {
                byte LetraABytes = value.letter;
                byte[] BytesLadoIzquierdo = BitConverter.GetBytes(value.left);
                byte[] BytesLadoDerecho = BitConverter.GetBytes(value.right);
                {
                    Array.Reverse(BytesLadoIzquierdo);
                    Array.Reverse(BytesLadoDerecho);
                }
                ListadoDeBytes.Add(LetraABytes);
                ListadoDeBytes.AddRange(BytesLadoIzquierdo);
                ListadoDeBytes.AddRange(BytesLadoDerecho);
            }
            return ListadoDeBytes.ToArray();
        }
        //Ver las frecuencias.
        private static Dictionary<byte, int> frequencies_(byte[] file_original)
        {
            Dictionary<byte, int> frequencies = new Dictionary<byte, int>();
            //Recorre archivo, encuentra el caracter y visualiza su frecuencia.
            foreach (byte caracter in file_original)
            {
                if (frequencies.ContainsKey(caracter))
                {
                    frequencies[caracter] += 1;
                }
                else
                {
                    frequencies.Add(caracter, 1);
                }
            }
            return frequencies;
        }
        //Listado de letras.
        private static List<Nodo> list_letters(Dictionary<byte, int> Frecuencias)
        {
            List<Nodo> letter_obtain = new List<Nodo>();
            foreach (KeyValuePair<byte, int> caracter in Frecuencias)
            {
                Nodo new_n = new Nodo(caracter.Key, caracter.Value, null, null, true);
                letter_obtain.Add(new_n);
            }
            return letter_obtain;
        }
        //Manejo de prioridades.
        private static Priority<Nodo> create_priority(List<Nodo> ListadoDeLetras)
        {
            Priority<Nodo> letter_priority = new Priority<Nodo>();
            foreach (Nodo caracter in ListadoDeLetras)
            {
                letter_priority.push(caracter);
            }
            return letter_priority;
        }
        //Crear arbol.
        private static Nodo creation_tree(Priority<Nodo> colaletra_priority)
        {
            colaletra_priority.push(new Nodo(System.Byte.MinValue, 0, null, null, true));
            while (colaletra_priority.counter != 1)
            {
                Nodo FirstNodoCola = colaletra_priority.pop();
                Nodo SecondNodoCola = colaletra_priority.pop();
                Nodo NewNodoArbol = new Nodo(System.Byte.MinValue, FirstNodoCola.frequency + SecondNodoCola.frequency, FirstNodoCola, SecondNodoCola, false);
                colaletra_priority.push(NewNodoArbol);
            }
            Nodo NodeRaizCola = colaletra_priority.pop();
            return NodeRaizCola;
        }
        //Asignar nodo.
        private static void code_assignation(Dictionary<byte, string> DiccionarioNuevoCaracteres, Nodo NodoACodificar, string CodigoAUtilizar, List<string> ListadoDeCaracteres)
        {
            if (NodoACodificar == null)
            {
                return;
            }
            string CodigoHijoIzquierdo = CodigoAUtilizar + "0";
            string CodigoHijoDerecho = CodigoAUtilizar + "1";
            code_assignation(DiccionarioNuevoCaracteres, NodoACodificar.left1, CodigoHijoIzquierdo, ListadoDeCaracteres);
            if (NodoACodificar.isLetter)
            {

                if (NodoACodificar.frequency == 0)
                {
                    ListadoDeCaracteres.Add(CodigoAUtilizar);
                }
                else
                {
                    DiccionarioNuevoCaracteres.Add(NodoACodificar.letter, CodigoAUtilizar);
                }
            }
            code_assignation(DiccionarioNuevoCaracteres, NodoACodificar.right1, CodigoHijoDerecho, ListadoDeCaracteres);
        }
        //Crear codigo.
        private static Dictionary<byte, string> code_character(Nodo NodoRaizArbolHFF, out string CodigoCaracter)
        {
            List<string> list_c = new List<string>();
            Dictionary<byte, string> Dictionary = new Dictionary<byte, string>();
            code_assignation(Dictionary, NodoRaizArbolHFF, "", list_c);
            CodigoCaracter = list_c[0];
            return Dictionary;
        }
        //Obtener la compresion del archivo.
        private static byte[] file_compressionObtention(byte[] DatosCompletos, Dictionary<byte, string> DiccionarioDeDatos, string NuevoCodigoCaracter)
        {
            List<byte> result_list = new List<byte>();
            List<bool> Data_ = new List<bool>();
            int UltimoByteEnDisco = 0;
            int DatoIncrementador = 1;
            foreach (byte BValor in DatosCompletos)
            {
                string CadenaCodigo = DiccionarioDeDatos[BValor];
                foreach (char CValor in CadenaCodigo)
                {
                    Data_.Add(CValor == '1' ? true : false);
                    if (Data_.Count == 8)
                    {
                        int DatosConvertidosBooleanos = Convert.ToByte(Data_[0]) * 1 + Convert.ToByte(Data_[1]) * 2 + Convert.ToByte(Data_[2]) * 4 + Convert.ToByte(Data_[3]) * 8 + Convert.ToByte(Data_[4]) * 16 + Convert.ToByte(Data_[5]) * 32 + Convert.ToByte(Data_[6]) * 64 + Convert.ToByte(Data_[7]) * 128;
                        result_list.Add((byte)DatosConvertidosBooleanos);
                        Data_.Clear();
                    }
                }
            }
            foreach (char CRValor in NuevoCodigoCaracter)
            {
                Data_.Add(CRValor == '1' ? true : false);
                if (Data_.Count == 8)
                {
                    int DatosConvertidosBooleanos = Convert.ToByte(Data_[0]) * 1 + Convert.ToByte(Data_[1]) * 2 + Convert.ToByte(Data_[2]) * 4 + Convert.ToByte(Data_[3]) * 8 + Convert.ToByte(Data_[4]) * 16 + Convert.ToByte(Data_[5]) * 32 + Convert.ToByte(Data_[6]) * 64 + Convert.ToByte(Data_[7]) * 128;
                    result_list.Add((byte)DatosConvertidosBooleanos);
                    Data_.Clear();
                }
            }
            for (int i = 0; i < Data_.Count; i++)
            {
                if (Data_[i])
                {
                    UltimoByteEnDisco += DatoIncrementador;
                }
                DatoIncrementador *= 2;
            }
            result_list.Add((byte)UltimoByteEnDisco);
            return result_list.ToArray();
        }
        //Se finaliza la compresion.
        public static byte[] finish_compression(byte[] ArchivoOriginal)
        {
            string NuevoCodigoDeCaracter;
            Dictionary<byte, int> Frequencies = frequencies_(ArchivoOriginal);
            List<Nodo> list_letter = list_letters(Frequencies);
            Priority<Nodo> colaletra_priority = create_priority(list_letter);
            Nodo Node_root = creation_tree(colaletra_priority);
            Dictionary<byte, string> CodigoCaracter = code_character(Node_root, out NuevoCodigoDeCaracter);
            MemoryStream memory_space = new MemoryStream();
            BinaryWriter EscrituraBinaria = new BinaryWriter(memory_space);
            byte[] ArbolDeBytes = tree_conversion(Node_root);
            EscrituraBinaria.Write(ArbolDeBytes);
            byte[] ComprimirDatos = file_compressionObtention(ArchivoOriginal, CodigoCaracter, NuevoCodigoDeCaracter);
            EscrituraBinaria.Write(ComprimirDatos);
            EscrituraBinaria.Flush();
            return memory_space.ToArray();

        }
    }
}
