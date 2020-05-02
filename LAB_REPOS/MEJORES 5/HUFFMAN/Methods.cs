using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LAB_REPOS.MEJORES_5.HUFFMAN
{
    public class Methods
    {
        #region Definiciones
        private static string file_compression;
        private static string route;
        private static string route_container;
        private static string file_decompression;
        private static double size_de;
        private static double size_com;
        public DateTime time = DateTime.Now;
        private string separations(IFormFile file_)
        {
            string temporal = string.Empty;
            string result_text = string.Empty;
            char[] remover = null;
            string[] receptor = null;
            using (var lecture = new StreamReader(file_.OpenReadStream()))
            {
                var capture_file = new StringBuilder();
                while (lecture.Peek() >= 0)
                {
                    capture_file.AppendLine(lecture.ReadLine());
                }
                temporal = capture_file.ToString();
                if (temporal.Contains("\r\n"))
                {
                    result_text = temporal.Replace("\r\n", "\n");
                }
                lecture.Close();
            }
            return result_text;
        }
        #endregion
        //Archivo a comprimir en controlador, ruta a escribir en el archivo y nuevo nombre del archivo.
        public void compression_huffman(IFormFile file_entry, string path1, string new_name)
        {
            string file_string = separations(file_entry);
            byte[] file_byte = Encoding.ASCII.GetBytes(file_string);
            byte[] file_huff = Compression.finish_compression(file_byte);
            var route_file = Path.Combine(path1, new_name + ".huff");
            //Nombre del archivo antes de ser comprimido.
            file_compression = file_entry.FileName.ToString();
            File.WriteAllBytes(route_file, file_huff);
        }
        //Archivo compreso en el controlador, ruta del archivo y ruta donde estara la descompresion
        public void ProcesoDecompresionHuffman(IFormFile file_entry2, string path_container, string path_write)
        {
            string file_ = string.Empty;
            byte[] bytes_ = File.ReadAllBytes((path_container + file_entry2.FileName));
            byte[] ArchivoDecompresoHuff = Decompression.finish_descompression(bytes_);
            var RutaArchivo = Path.Combine(path_write, file_compression);
            route_container = path_container;
            route = path_write;
            size_com = bytes_.Length;
            size_de = ArchivoDecompresoHuff.Length;
            //Nombre original
            file_decompression = file_entry2.FileName.ToString();
            File.WriteAllBytes(RutaArchivo, ArchivoDecompresoHuff);
        }
        //Ruta del archivo comprimido y todos sus datos.
        public void data_huffman(string path_compressions)
        {
            double reason = 0;
            double factor = 0;
            double percentage = 0;
            var Path1 = Path.Combine(path_compressions, +time.Minute + "CompressionsHuffman" + ".txt");
            using (StreamWriter Escritura = new StreamWriter(Path1))
            {
                for (int i = 0; i <= 5; i++)
                {

                    if (i == 0)
                    {
                        Escritura.Write("Nombre Original: " + file_compression);
                    }
                    if (i == 1)
                    {
                        Escritura.Write(Environment.NewLine + "Nombre Archivo Compreso: " + file_decompression + ", Ruta Contenedora: " + route_container);
                    }
                    if (i == 2)
                    {
                        reason = Math.Round(Convert.ToDouble(size_com / size_de), 2);
                        Escritura.Write(Environment.NewLine + "Razon de compresion: " + reason.ToString());
                    }
                    if (i == 3)
                    {
                        factor = Math.Round(Convert.ToDouble(size_de / size_com), 2);
                        Escritura.Write(Environment.NewLine + "Factor de compresion: " + factor.ToString());
                    }
                    if (i == 4)
                    {
                        percentage = Math.Round(((factor / reason) * 100), 2);
                        Escritura.Write(Environment.NewLine + "Porcentaje de compresion: " + percentage.ToString() + "%");
                    }
                    if (i == 5)
                    {
                        Escritura.Write(Environment.NewLine + "Archivo Compreso y decompreso con: Huffman");
                    }

                }
            }
        }
    }
}
