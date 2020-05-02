using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using LAB_REPOS.MEJORES_5.LZW;
namespace LAB_REPOS.Controllers.LZWController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LZWController : ControllerBase
    {
        // GET: api/LZW
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/LZW
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
