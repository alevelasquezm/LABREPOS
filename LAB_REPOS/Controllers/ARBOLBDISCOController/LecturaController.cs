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
    [Route("api/[controller]")]
    [ApiController]
    public class LecturaController : ControllerBase
    {
        [HttpGet]
        [Route("api/Registrar")]
        public ActionResult<string> Registro()
        {
        }
        
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }
        
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
