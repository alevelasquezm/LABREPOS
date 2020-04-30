using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using LAB_REPOS.MEJORES_5.CIFRADOS;

namespace LAB_REPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {

            return "value";
        }

        
        //-----------------------------------------------------ENDPOINTS PARA LAB DE CIFRADOS ZIGZAG, CESAR, DE RUTA------------------------------------------------------
        // POST api/Cesar
        [Route("CifradoCesar")]
        [HttpPost]
        public void PostCifradoCesar([FromBody] string file, string key)
        {
            Cesar_Encryption CESAR = new Cesar_Encryption();
            var lecture = Path.GetFullPath("Cifrado");
            var lecture2 = Path.GetFullPath(file);
            CESAR.message(lecture, file, key);
        }

        [Route("DescifradoCesar")]
        [HttpPost]
        public void PostDescifradoCesar([FromBody]string file, string key)
        {
            Cesar_Encryption CESAR = new Cesar_Encryption();
            var lecture = Path.GetFullPath("Descifrado");
            var lecture2 = Path.GetFullPath(file);
            CESAR.message_d(file, lecture, key);

        }

    }
}
