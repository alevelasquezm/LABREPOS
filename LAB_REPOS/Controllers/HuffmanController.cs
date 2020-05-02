using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LAB_REPOS.MEJORES_5.HUFFMAN;
using Microsoft.AspNetCore.Hosting;
using System.IO;
namespace LAB_REPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HuffmanController : ControllerBase
    {
        public static IWebHostEnvironment information;
        public HuffmanController(IWebHostEnvironment info)
        {
            information = info;
        }
        // GET: api/Huffman
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        // POST: api/Huffman
        //Maneja el factor, razon y entre otros de compresion.
        [HttpPost("TodasLasCompresionesHuffman")]
        public void Compressions()
        {
            if (!Directory.Exists(information.WebRootPath + "\\Compresion_Total\\"))
            {
                Directory.CreateDirectory(information.WebRootPath + "\\Compresion_Total\\");
            }
            Methods Writing = new Methods();
            Writing.data_huffman(information.WebRootPath + "\\Compresion_Total\\");
        }
        //Compresion de Huffman.
        [HttpPost("compress_Huffman")]
        public void CompresionHuffman([FromForm] Huffman_A _DataHuffman)
        {
            if (!Directory.Exists(information.WebRootPath + "\\Compresiones_Huffman\\"))
            {
                Directory.CreateDirectory(information.WebRootPath + "\\Compresiones_Huffman\\");
            }
            Methods huff_method = new Methods();
            huff_method.compression_huffman(_DataHuffman.compression, information.WebRootPath + "\\Compresiones_Huffman\\", _DataHuffman.new_name);

        }
        //Decompresion de Huffman.
        [HttpPost("Decompress_Huffman")]
        public void DecompresionHuffman([FromForm]IFormFile _ArchivoHUFF)
        {
            if (!Directory.Exists(information.WebRootPath + "\\Descompresiones_Huffman\\"))
            {
                Directory.CreateDirectory(information.WebRootPath + "\\Descompresiones_Huffman\\");
            }
            Methods huffman_method = new Methods();
            huffman_method.ProcesoDecompresionHuffman(_ArchivoHUFF, information.WebRootPath + "\\Descompresiones_Huffman\\", information.WebRootPath + "\\Descompresiones_Huffman\\");

        }
    }
}
