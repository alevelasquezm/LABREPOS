using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using LAB_REPOS.MEJORES_5.RSA;

namespace LAB_REPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CifradoRSAController : ControllerBase
    {
        // GET: api/CifradoRSA
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/RSA
        [Route("CifradoRSA")]
        public void PostCifradoRSA([FromBody] string file1, string file2, string file)
        {
            var path1 = "";
            var path2 = "";
            var fileName = "";



            path1 = Path.GetFullPath("Archivo");
            fileName = Path.GetFullPath(file1);


            path2 = Path.GetFullPath("Archivo");
            fileName = Path.GetFullPath(file2);

            var lecture = Path.GetFullPath("Archivo");
            var lecture2 = Path.GetFullPath(file);
            RSA_Encryption rsa = new RSA_Encryption();
            rsa.read_text(path1, path2, lecture, fileName);


        }

        // POST api/RSA
        [HttpGet("getPublicKey")]
        public void GenerarLlaves([FromBody] Keys RSA, string file)
        {
            var primo1 = RSA.numeroPrimo(RSA.ValorP, 2);
            var primo2 = RSA.numeroPrimo(RSA.ValorQ, 2);

            RSA_Encryption rsa = new RSA_Encryption();
            var lecture = Path.GetFullPath("Archivo");
            var lecture2 = Path.GetFullPath(file);
            rsa.Keys(RSA.ValorP, RSA.ValorQ, lecture);
        }
        // POST api/RSA
        [Route("DescifradoRSA")]
        public void PostDescifradoRSA([FromBody] string file1, string file2, string file)
        {
            var path1 = "";
            var path2 = "";
            var fileName = "";



            path1 = Path.GetFullPath("Archivo");
            fileName = Path.GetFullPath(file1);


            path2 = Path.GetFullPath("Archivo");
            fileName = Path.GetFullPath(file2);

            var lecture = Path.GetFullPath("Archivo");
            var lecture2 = Path.GetFullPath(file);
            RSA_Encryption rsa = new RSA_Encryption();
            rsa.read_encryption(path1, path2, lecture, fileName);


        }

    }
}
