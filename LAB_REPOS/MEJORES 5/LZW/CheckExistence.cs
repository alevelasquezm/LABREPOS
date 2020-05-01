using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.LZW
{
    public class CheckExistence
    {
        private static CheckExistence exists = null;
        public static CheckExistence Exist
        {
            get
            {
                if (exists == null) exists = new CheckExistence();
                return exists;
            }
        }

        public List<TablaComprimir> tabla = new List<TablaComprimir>();
    }
}
