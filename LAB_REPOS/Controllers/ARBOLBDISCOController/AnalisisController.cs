using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LAB_REPOS.MEJORES_5.ARBOL_B_DISCO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LAB_REPOS.Controllers.ARBOLBDISCOController
{
    delegate string ObjectToString(object o);
    delegate string StringToObject(string s);
    [Route("[controller]")]
    public class AnalisisController : Controller
    {
        [HttpGet]
        public List<Soda> RecorridoInOrder()
        {
            ArbolB<Soda>.begin_tree("Sodas", new StringToObject(Soda.StringToSoda), new ObjectToString(Soda.SodaToString));
            return ArbolB<Soda>.pass(null, null);
        }
        [HttpGet, Route("busqueda")]
        public List<Soda> Busqueda([FromForm]Soda dato)
        {
            ArbolB<Soda>.begin_tree("Sodas", new StringToObject(Soda.StringToSoda), new ObjectToString(Soda.SodaToString));
            return ArbolB<Soda>.pass(dato, null, 1);
        }

        [HttpPost]
        public void Agregar([FromForm]Soda dato)
        {
            ArbolB<Soda>.begin_tree("Sodas", new StringToObject(Soda.StringToSoda), new ObjectToString(Soda.SodaToString));
            ArbolB<Soda>.insert_t(dato);
        }
    }
}
