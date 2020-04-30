using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using LAB_REPOS.MEJORES_5.CIFRADOS;
using Microsoft.AspNetCore.Http;
using LAB_REPOS.MEJORES_5.LZW;

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
                                                                          // C E S A R      
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
                                                                           //ZIGZAG
        // POST api/ZigZag
        [Route("CifradoZigZag")]
        [HttpPost]
        public void PostCifradoZigZag([FromBody] string file, int level)
        {
            ZigZag_Encryption ZigZag = new ZigZag_Encryption();
            var lecture = Path.GetFullPath("Cifrado");
            var lecture2 = Path.GetFullPath(file);
            var BytesList = ZigZag.encryption(file, level);
            var AddExtraC = 0;
            var Matrix = ZigZag.matrix(BytesList.Count(), level, ref AddExtraC);
            var ExtraCharacter = new byte();
            if (AddExtraC > BytesList.Count())
            {
                BytesList = ZigZag.add_extra_c(BytesList, AddExtraC, ref ExtraCharacter);
            }
            ZigZag.message(Matrix, level, lecture, BytesList, ExtraCharacter);

        }
        [Route("DescifradoZigZag")]
        [HttpPost]
        public void PostDescifradoZigZag([FromBody] string file, int level)
        {
            ZigZag_Encryption ZigZag = new ZigZag_Encryption();
            var lecture = Path.GetFullPath("Descifrado");
            var lecture2 = Path.GetFullPath(file);
            var ExtraC = new byte();
            var BytesList = ZigZag.decryption(file, level, ref ExtraC);
            var AddExtraC = 0;
            var Matrix = ZigZag.matrix_dec(BytesList.Count(), level, ref AddExtraC);
            var AddCharacter2 = new byte();
            if (AddExtraC > BytesList.Count())
            {
                BytesList = ZigZag.add_extra_c(BytesList, AddExtraC, ref AddCharacter2);
            }

            ZigZag.message_dec(lecture, level, BytesList, Matrix, ExtraC);

        }
        //DE RUTA 
        // POST api/Spiral
        [Route("CifradoRuta")]
        [HttpPost]
        public void PostCifradoRuta([FromBody] string file, int l, bool direction)
        {
            Route_Encryption ruta = new Route_Encryption();
            var lecture = Path.GetFullPath("Cifrado");
            var archivoleido2 = Path.GetFullPath(file);
            ruta.message(lecture, file, l, direction);

        }
        [Route("DescifradoRuta")]
        [HttpPost]
        public void PostDescifradoRuta([FromBody] string file, int l, bool direction)
        {
            Route_Encryption ruta = new Route_Encryption();
            var lecture = Path.GetFullPath("Descifrado");
            var archivoleido2 = Path.GetFullPath(file);
            ruta.message_d(lecture, file, l, direction);
        }
        //-----------------------------------------------------ENDPOINT PARA LAB DE LZW------------------------------------------------------
                                                                     // L Z W
        // POST: api/LZW
        [Route("Compresionlzw")]
        [HttpPost]
        public void PostCompresionLZW([FromForm] IFormFile Nombre)
        {
            LZW LZW = new LZW();
            var arch = Path.GetFullPath(Nombre.FileName);
            var arch1 = new FileStream(arch, FileMode.Open);
            LZW.dictionary_initial(arch1);
            LZW.compression_process(arch1, arch);
        }
        [Route("Descompresionlzw")]
        [HttpPost]
        public void DescomprimirLZW([FromForm] IFormFile Nombre)
        {
            LZW LZW = new LZW();
            var arch = Path.GetFullPath(Nombre.FileName);
            var arch1 = new FileStream(arch, FileMode.Open);
            LZW.descompression(Nombre.FileName);
        }
    }
}
