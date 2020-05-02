using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.ARBOL_B_DISCO
{
    public class ArbolB<T> where T : IComparable
    {
        static List<T> temporal_pass;
        static int[] Data(int[] data = null)
        {
            var buffer = new byte[14];
            using (var files = new FileStream(ArbolBusqueda.Instance.path, FileMode.OpenOrCreate))
            {
                if (data == null)
                {
                    files.Read(buffer, 0, 14);
                    var values = Encoding.UTF8.GetString(buffer).Split('|');
                    return new int[3] { Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), Convert.ToInt32(values[2]) };
                }
                var line = string.Empty;
                foreach (var item in data)
                {
                    line = $"{line}{item.ToString("0000;-0000")}|";
                }
                files.Write(Encoding.UTF8.GetBytes(line.ToCharArray()), 0, 15);
            }
            return null;
        }
        //Inicializar el arbol
        public static void begin_tree(string name, Delegate oNodo, Delegate oString)
        {
            var grado = 5;
            ArbolBusqueda.Instance.path = $"Datos\\{name}.txt";
            if (!Directory.Exists("Datos"))
            {
                Directory.CreateDirectory("Datos");
            }
            if (!File.Exists(ArbolBusqueda.Instance.path))
            {
                string text = $"{grado.ToString("0000;-0000")}|0000|0001|";
                File.WriteAllText(ArbolBusqueda.Instance.path, text);
            }
            ArbolBusqueda.Instance.node = oNodo;
            ArbolBusqueda.Instance.string_ = oString;
        }
        //Insertar info en el arbol.
        public static void insert_t(T info)
        {
            //1. Grado, 2. Raiz y 3. Siguiente.
            var metaData = Data();
            ArbolBusqueda.Instance.grade = metaData[0];
            if (metaData[1] != 0)
            {
                var carry = false;
                var carryHijo = false;
                var IsFirst = true;
                var Hijo = 0;
                var posHijo = 0;
                insertion(metaData[1], ref info, ref Hijo, ref posHijo, ref carry, ref carryHijo, ref IsFirst);
            }
            else
            {
                metaData[1]++;
                metaData[2]++;
                var nodo = new NodoB<T>(0) { index = 1, Values = new List<T> { info }, Sons = new List<int>() };
                nodo.NodoToString();
                Data(metaData);
            }
        }
        //Insercion como tal.
        static void insertion(int indiceActual, ref T info, ref int hijo, ref int posHijo, ref bool TieneCarry, ref bool TieneHijo, ref bool first)
        {
            var metaData = Data();
            var actual = NodoB<T>.StringToNodo(indiceActual);
            var pos = 0;
            if (actual.Sons.Count != 0)
            {
                while ((pos < actual.Values.Count) && (actual.Values[pos].CompareTo(info) == -1))
                {
                    pos++;
                }
                insertion(actual.Sons[pos], ref info, ref hijo, ref posHijo, ref TieneCarry, ref TieneHijo, ref first);
            }
            else
            {
                while ((pos < actual.Values.Count) && (actual.Values[pos].CompareTo(info) == -1))
                {
                    pos++;
                }
                actual.Values.Insert(pos, info);
            }
            if (TieneCarry)
            {
                pos = 0;
                actual = NodoB<T>.StringToNodo(actual.index);
                while ((pos < actual.Values.Count) && (actual.Values[pos].CompareTo(info) == -1))
                {
                    pos++;
                }
                actual.Values.Insert(pos, info);
                if (TieneHijo)
                {
                    actual.Sons.Insert(posHijo, hijo);
                    TieneHijo = false;
                }
                TieneCarry = false;
            }
            if (actual.Values.Count == actual.cant + 1)
            {
                if ((actual.Sons.Count == 0) && (actual.Father != 0))
                {
                    var padre = NodoB<T>.StringToNodo(actual.Father);
                    var indiceAct = padre.Sons.IndexOf(actual.index);
                    var indiceHer = rotation_node(padre, indiceAct, metaData);
                    T datoTemporal = actual.Values[0];
                    var LlevaCarry = false;
                    if (indiceHer < indiceAct)
                    {
                        var indiceDato = indiceAct == 0 ? 0 : indiceAct - 1;
                        for (int i = indiceAct; i >= indiceHer; i--)
                        {
                            if (padre.Sons[i] != actual.index)
                            {
                                actual = NodoB<T>.StringToNodo(padre.Sons[i]);
                            }
                            if (LlevaCarry)
                            {
                                padre.Values.Insert(indiceDato, datoTemporal);
                                datoTemporal = padre.Values[indiceDato + 1];
                                padre.Values.RemoveAt(indiceDato + 1);
                                padre.NodoToString();
                                actual.Values.Add(datoTemporal);
                                datoTemporal = actual.Values[0];
                                if (i != indiceHer)
                                {
                                    actual.Values.RemoveAt(0);
                                }
                                indiceDato--;
                            }
                            else
                            {
                                datoTemporal = actual.Values[0];
                                actual.Values.RemoveAt(0);
                            }
                            actual.NodoToString();
                            LlevaCarry = true;
                        }
                    }
                    else if (indiceHer > indiceAct)
                    {
                        var indiceDato = indiceAct == padre.Sons.Count() - 1 ? indiceAct - 1 : indiceAct;
                        for (int i = indiceAct; i <= indiceHer; i++)
                        {
                            if (padre.Sons[i] != actual.index)
                            {
                                actual = NodoB<T>.StringToNodo(padre.Sons[i]);
                            }
                            if (LlevaCarry)
                            {
                                padre.Values.Insert(indiceDato, datoTemporal);
                                datoTemporal = padre.Values[indiceDato + 1];
                                padre.Values.RemoveAt(indiceDato + 1);
                                padre.NodoToString();
                                actual.Values.Insert(0, datoTemporal);
                                datoTemporal = actual.Values[actual.Values.Count - 1];
                                if (i != indiceHer)
                                {
                                    actual.Values.RemoveAt(actual.Values.Count - 1);
                                }
                                indiceDato++;
                            }
                            else
                            {
                                datoTemporal = actual.Values[actual.Values.Count - 1];
                               actual.Values.RemoveAt(actual.Values.Count - 1);
                            }
                            actual.NodoToString();
                            LlevaCarry = true;
                        }
                    }
                    else
                    {
                        var indiceDato = indiceAct == 0 ? 0 : indiceAct - 1;
                        indiceHer = indiceAct - 1 >= 0 ? indiceAct - 1 : indiceAct + 1;
                        var hermano = NodoB<T>.StringToNodo(padre.Sons[indiceHer]);
                        var listaTemporal = new List<T>();
                        var temporal = new NodoB<T>(padre.index) { index = metaData[2], Values = new List<T>(), Sons = new List<int>() };
                        if (indiceAct - 1 == indiceHer)
                        {
                            foreach (var item in hermano.Values)
                            {
                                listaTemporal.Add(item);
                            }
                            listaTemporal.Add(padre.Values[indiceDato]);
                            foreach (var item in actual.Values)
                            {
                                listaTemporal.Add(item);
                            }
                            var cantDividida = (listaTemporal.Count - 2) / 3;

                            temporal.Values.AddRange(listaTemporal.GetRange(0, cantDividida));
                            listaTemporal.RemoveRange(0, cantDividida);

                            padre.Values.RemoveAt(indiceDato);
                            padre.Values.Insert(indiceDato, listaTemporal[0]);
                            listaTemporal.RemoveAt(0);

                            hermano.Values.Clear();
                            hermano.Values.AddRange(listaTemporal.GetRange(0, cantDividida));
                            listaTemporal.RemoveRange(0, cantDividida);

                            padre.Values.Insert(indiceDato + 1, listaTemporal[0]);
                            listaTemporal.RemoveAt(0);

                            actual.Values.Clear();
                            actual.Values.AddRange(listaTemporal.GetRange(0, cantDividida));

                            indiceHer = indiceAct - 1 >= 0 ? indiceAct - 1 : indiceAct + 2;
                            padre.Sons.Insert(indiceHer, temporal.index);
                        }
                        else
                        {
                            foreach (var item in actual.Values)
                            {
                                listaTemporal.Add(item);
                            }
                            listaTemporal.Add(padre.Values[indiceDato]);
                            foreach (var item in hermano.Values)
                            {
                                listaTemporal.Add(item);
                            }
                            var cantDividida = (listaTemporal.Count - 2) / 3;

                            actual.Values.Clear();
                            actual.Values.AddRange(listaTemporal.GetRange(0, cantDividida));
                            listaTemporal.RemoveRange(0, cantDividida);

                            padre.Values.RemoveAt(indiceDato);
                            padre.Values.Insert(indiceDato, listaTemporal[0]);
                            listaTemporal.RemoveAt(0);

                            hermano.Values.Clear();
                            hermano.Values.AddRange(listaTemporal.GetRange(0, cantDividida));
                            listaTemporal.RemoveRange(0, cantDividida);

                            padre.Values.Insert(indiceDato + 1, listaTemporal[0]);
                            listaTemporal.RemoveAt(0);

                            temporal.Values.AddRange(listaTemporal.GetRange(0, cantDividida));

                            padre.Sons.Insert(indiceHer + 1, temporal.index);
                        }

                        if (padre.Values.Count > padre.cant)
                        {
                            info = padre.Values[0];
                            padre.Values.RemoveAt(0);
                            posHijo = padre.Sons.IndexOf(temporal.index);
                            padre.Sons.RemoveAt(posHijo);
                            hijo = temporal.index;
                            TieneCarry = true;
                            TieneHijo = true;
                        }
                        temporal.NodoToString();
                        padre.NodoToString();
                        hermano.NodoToString();
                        metaData[2]++;
                        Data(metaData);
                    }
                }
                else
                {
                    metaData = Data();
                    var posMedia = actual.Values.Count % 2 == 0 ? (actual.Values.Count - 1) / 2 : actual.Values.Count / 2;
                    var padreHermano = actual.Father == 0 ? metaData[2] + 1 : actual.Father;
                    var Hermano = new NodoB<T>(padreHermano) { index = metaData[2], Values = actual.Values.GetRange(0, posMedia) };

                    metaData[2]++;

                    if (actual.Sons.Count != 0)
                    {
                        Hermano.Sons = actual.Sons.GetRange(0, posMedia + 1);
                        actual.Sons.RemoveRange(0, posMedia + 1);

                        foreach (var indiceHijo in Hermano.Sons)
                        {
                            var Hijo = NodoB<T>.StringToNodo(indiceHijo);
                            Hijo.Father = Hermano.index;
                            Hijo.NodoToString();
                        }
                    }

                    if (actual.Father == 0)
                    {
                        var padre = new NodoB<T>(0) { Values = new List<T> { actual.Values[posMedia] }, Sons = new List<int> { Hermano.index, actual.index }, index = metaData[2] };
                        metaData[1] = metaData[2];
                        metaData[2]++;
                        padre.NodoToString();
                        Hermano.Father = padre.index;
                        actual.Father = padre.index;
                    }
                    else
                    {
                        var padre = NodoB<T>.StringToNodo(actual.Father);
                        info = actual.Values[posMedia];
                        pos = 0;
                        while (pos < padre.Values.Count && padre.Values[pos].CompareTo(info) == -1)
                        {
                            pos++;
                        }
                        padre.Values.Insert(pos, info);
                        padre.Sons.Insert(padre.Sons.IndexOf(actual.index), Hermano.index);

                        if (padre.Values.Count > padre.cant)
                        {
                            info = padre.Values[0];
                            padre.Values.RemoveAt(0);
                            posHijo = padre.Sons.IndexOf(Hermano.index);
                            padre.Sons.RemoveAt(posHijo);
                            hijo = Hermano.index;
                            TieneCarry = true;
                            TieneHijo = true;
                        }

                        padre.NodoToString();
                    }
                    actual.Values.RemoveRange(0, posMedia + 1);
                    actual.NodoToString();
                    Hermano.NodoToString();
                    Data(metaData);
                }
            }
            if (first)
            {
                actual.NodoToString();
                first = false;
            }
        }
        //Rotacion de nodos.
        static int rotation_node(NodoB<T> Father, int list, int[] metadata)
        {
            var temporal = new NodoB<T>(1);

            if (list - 1 >= 0)
            {
                temporal = NodoB<T>.StringToNodo(Father.Sons[list - 1]);
                if (temporal.Values.Count < temporal.cant)
                {
                    return list - 1;
                }
            }
            if (list + 1 < Father.Sons.Count)
            {
                temporal = NodoB<T>.StringToNodo(Father.Sons[list + 1]);
                if (temporal.Values.Count < temporal.cant)
                {
                    return list + 1;
                }
            }
            if (list - 2 >= 0)
            {
                for (int i = list - 2; i >= 0; i--)
                {
                    temporal = NodoB<T>.StringToNodo(Father.Sons[i]);
                    if (temporal.Values.Count < temporal.cant)
                    {
                        return i;
                    }
                }
            }
            if (list + 2 < Father.Sons.Count)
            {
                for (int i = list + 2; i < Father.Sons.Count; i++)
                {
                    temporal = NodoB<T>.StringToNodo(Father.Sons[i]);
                    if (temporal.Values.Count < temporal.cant)
                    {
                        return i;
                    }
                }
            }
            return list;
        }
        //Reccorrido InOrden.
        static void InOrden(NodoB<T> actual)
        {
            if (actual.Sons.Count != 0)
            {
                var pos_data = 1;
                foreach (var son in actual.Sons)
                {
                    InOrden(NodoB<T>.StringToNodo(son));
                    if (pos_data < actual.Sons.Count)
                    {
                        temporal_pass.Add(actual.Values[pos_data - 1]);
                        pos_data++;
                    }
                }
            }
            else
            {
                foreach (var dato in actual.Values)
                {
                    temporal_pass.Add(dato);
                }
            }
        }
        //Busqueda en el arbol.
        static void finding(NodoB<T> actual, T info, ref bool continue2)
        {
            var pos = 0;
            while (continue2 && pos < actual.Values.Count && (actual.Values[pos].CompareTo(info) == -1 || actual.Values[pos].CompareTo(info) == 0))
            {
                if (actual.Values[pos].CompareTo(info) == 0)
                {
                    continue2 = false;
                    temporal_pass.Add(actual.Values[pos]);
                }
                pos++;
            }
            if (continue2 && actual.Sons.Count != 0)
            {
                finding(NodoB<T>.StringToNodo(actual.Sons[pos]), info, ref continue2);
            }
        }
        //Recorridos.
        public static List<T> pass(T info1, T info2, int type = 0)
        {
            temporal_pass = new List<T>();
            var metaData = Data();
            ArbolBusqueda.Instance.grade = metaData[0];
            if (metaData[1] != 0)
            {
                var raiz = NodoB<T>.StringToNodo(metaData[1]);
                var continue_ = true;

                switch (type)
                {
                    case 0:
                        InOrden(raiz);
                        break;
                    case 1:
                        finding(raiz, info1, ref continue_);
                        break;
                    case 2:
                        finding(raiz, info1, ref continue_);
                        continue_ = true;
                        finding(raiz, info2, ref continue_);
                        break;
                }
            }
            return temporal_pass;
        }
    }
}
