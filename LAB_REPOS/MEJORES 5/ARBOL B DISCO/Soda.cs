using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.ARBOL_B_DISCO
{
    public class Soda : IComparable
    {
        #region Definiciones
        public string Name { get; set; }
        public string Flavor { get; set; }
        public string Volume { get; set; }
        public string Price { get; set; }
        public string Producer_House { get; set; }
        #endregion
        public int CompareTo(object objeto)
        {
            return this.Name.CompareTo(((Soda)objeto).Name);
        }
        public static string SodaToString(object info)
        {
            var actual = (Soda)info;
            actual.Name = actual.Name == null ? "" : actual.Name;
            actual.Flavor = actual.Flavor == null ? "" : actual.Flavor;
            actual.Producer_House = actual.Producer_House == null ? "" : actual.Producer_House;
            return $"{string.Format("{0,-100}", actual.Name)}{string.Format("{0,-100}", actual.Flavor)}{string.Format("{0,-100}", actual.Volume.ToString())}{string.Format("{0,-100}", actual.Price.ToString())}{string.Format("{0,-100}", actual.Producer_House)}";
        }
        public static Soda StringToSoda(string info)
        {
            var separation_info = new List<string>();
            for (int x = 0; x < 5; x++)
            {
                separation_info.Add(info.Substring(0, 100));
                info = info.Substring(100);
            }
            return new Soda() { Name = separation_info[0].Trim(), Flavor = separation_info[1].Trim(), Volume = separation_info[2], Price = separation_info[3], Producer_House = separation_info[4].Trim() };
        }
    }
}
