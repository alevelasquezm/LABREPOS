using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace LAB_REPOS.MEJORES_5.CIFRADOS
{
    public class Route_Encryption
    {
        string route = string.Empty;
        string text = string.Empty;
        string text_m = string.Empty;
        //Lectura de archivo.
        public void lecture(string file)
        {
            int length = 100000;
            var bytes = new byte[length];

            using (var stream = new FileStream(file, FileMode.Open))
            {
                using (var reading = new BinaryReader(stream))
                {
                    while (reading.BaseStream.Position != reading.BaseStream.Length)
                    {
                        bytes = reading.ReadBytes(length);
                        foreach (char letter in bytes)
                        {
                            if (letter != 0)
                            {
                                text += letter;
                            }
                        }
                    }
                }
            }
        }
        //Escritura en cifrado.
        public void file_encrypted(string text_)
        {
            //Escribir texto cifrado en el archivo.
            using (var write = new FileStream(route + "Cifrado", FileMode.OpenOrCreate))
            {
                using (var writing = new BinaryWriter(write))
                {
                    writing.Seek(0, SeekOrigin.End);
                    writing.Write(System.Text.Encoding.Unicode.GetBytes(text_));
                }
            }
            text_m = string.Empty;
        }
        //Escritura en descifrado.
        public void file_decrypted(string text_)
        {
            //Escribir texto descifrado en el archivo.
            using (var write = new FileStream(route + "Descifrado", FileMode.OpenOrCreate))
            {
                using (var writing = new BinaryWriter(write))
                {
                    writing.Seek(0, SeekOrigin.End);
                    writing.Write(System.Text.Encoding.Unicode.GetBytes(text_));
                }
            }
        }
        //Recorrido vertical
        public void down_path(int value_m, int value_n, char[,] matrix)
        {
            //Recorrer matriz en espiral
            int x, aux_m = 0, aux_n = 0;
            while (aux_m < value_m && aux_n < value_n)
            {
                for (x = aux_m; x < value_m; ++x)
                {
                    text_m += matrix[x, aux_n];
                }
                aux_n++;
                for (x = aux_n; x < value_n; ++x)
                {
                    text_m += matrix[value_m - 1, x];
                }
                value_m--;
                if (aux_n < value_n)
                {
                    for (x = value_m - 1; x >= aux_m; --x)
                    {
                        text_m += matrix[x, value_n - 1];
                    }
                    value_n--;
                }
                if (aux_m < value_m)
                {
                    for (x = value_n - 1; x >= aux_n; --x)
                    {
                        text_m += matrix[aux_m, x];
                    }
                    aux_m++;
                }
            }
            file_encrypted(text_m);
        }
        //Recorrido horizontal.
        public void right_path(int value_m, int value_n, char[,] matrix)
        {
            //Recorrer matriz en espiral
            int x, aux_m = 0, aux_n = 0;
            while (aux_m < value_m && aux_n < value_n)
            {
                for (x = aux_n; x < value_n; ++x)
                {
                    text_m += matrix[aux_m, x];
                }
                aux_m++;
                for (x = aux_m; x < value_m; ++x)
                {
                    text_m += matrix[x, value_n - 1];
                }
                value_n--;
                if (aux_m < value_m)
                {
                    for (x = value_n - 1; x >= aux_n; --x)
                    {
                        text_m += matrix[value_m - 1, x];
                    }
                    value_m--;
                }
                if (aux_n < value_n)
                {
                    for (x = value_m - 1; x >= aux_m; --x)
                    {
                        text_m += matrix[x, aux_n];
                    }
                    aux_n++;
                }

            }
            file_encrypted(text_m);

        }
        //Matriz cifrado.
        public void matrix_encrypted(int value_m, bool direction)
        {
            var value_n = this.text.Length / value_m;
            int counter_text = 0;
            if (this.text.Length % value_m != 0)
            {
                value_n++;
            }

            char[,] matrix = new char[value_m, value_n];
            if (direction)
            {
                //Llenar matriz horizontalmente.
                for (int x = 0; x < value_m; x++)
                {
                    for (int y = 0; y < value_n; y++)
                    {
                        if (counter_text == text.Length)
                        {
                            matrix[x, y] = Convert.ToChar(36);
                        }
                        else
                        {
                            matrix[x, y] = text[counter_text];
                            counter_text++;
                        }
                    }
                }

                down_path(value_m, value_n, matrix);
            }
            else
            {
                //Llenar matriz verticalmente.
                for (int x = 0; x < value_n; x++)
                {
                    for (int y = 0; y < value_m; y++)
                    {
                        if (counter_text == text.Length)
                        {
                            matrix[x, y] = Convert.ToChar(36);
                        }
                        else
                        {
                            matrix[x, y] = text[counter_text];
                            counter_text++;
                        }
                    }
                }
                right_path(value_m, value_n, matrix);
            }
        }
        //Matriz descifrado.
        public void matrix_decrypted(int value_m, bool direction)
        {
            var value_n = this.text.Length / value_m;
            int counter_text = 0;
            if (this.text.Length % value_m != 0)
            {
                value_n++;
            }
            var x = value_m;
            var y = value_n;

            char[,] matrix = new char[value_m, value_n];
            if (direction)
            {
                int i, aux_m = 0, aux_n = 0;
                while (aux_m < value_m && aux_n < value_n)
                {
                    for (i = aux_m; i < value_m; ++i)
                    {
                        matrix[i, aux_n] = text[counter_text];
                        counter_text++;
                    }
                    aux_n++;
                    for (i = aux_n; i < value_n; ++i)
                    {
                        matrix[value_m - 1, i] = text[counter_text];
                        counter_text++;
                    }
                    value_m--;
                    if (aux_n < value_n)
                    {
                        for (i = value_m - 1; i >= aux_m; --i)
                        {
                            matrix[i, value_n - 1] = text[counter_text];
                            counter_text++;
                        }
                        value_n--;
                    }
                    if (aux_m < value_m)
                    {
                        for (i = value_n - 1; i >= aux_n; --i)
                        {
                            matrix[aux_m, i] = text[counter_text];
                            counter_text++;
                        }
                        aux_m++;
                    }
                }
                var text_dec = string.Empty;
                for (int p = 0; p < x; p++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        if (matrix[p, j] != 36)
                        {
                            text_dec += matrix[p, j];

                        }
                    }
                }
                file_decrypted(text_dec);
            }
            else
            {
                //Recorrer matriz en espiral.
                int i, aux_m = 0, aux_n = 0;
                while (aux_m < value_m && aux_n < value_n)
                {
                    for (i = aux_n; i < value_n; ++i)
                    {
                        matrix[aux_m, i] = text[counter_text];
                        counter_text++;
                    }
                    aux_m++;
                    for (i = aux_m; i < value_m; ++i)
                    {
                        matrix[i, value_n - 1] = text[counter_text];
                        counter_text++;
                    }
                    value_n--;
                    if (aux_m < value_m)
                    {
                        for (i = value_n - 1; i >= aux_n; --i)
                        {
                            matrix[value_m - 1, i] = text[counter_text];
                            counter_text++; ;
                        }
                        value_m--;
                    }
                    if (aux_n < value_n)
                    {
                        for (i = value_m - 1; i >= aux_m; --i)
                        {
                            matrix[i, aux_n] = text[counter_text];
                            counter_text++;
                        }
                        aux_n++;
                    }
                }
                var text_deci = string.Empty;
                for (int p = 0; p < y; p++)
                {
                    for (int j = 0; j < x; j++)
                    {
                        if (matrix[j, p] != 36)
                        {
                            text_deci += matrix[j, p];

                        }
                    }
                }
                file_decrypted(text_deci);
            }
        }
        //Cifrar mensaje.
        public void message(string route_a, string file, int m, bool direction)
        {
            route = route_a;
            lecture(file);
            matrix_encrypted(m, direction);
            file_encrypted(text_m);
        }
        //Descifrar mensaje.
        public void message_d(string route_a, string file, int m, bool direction)
        {
            route = route_a;
            lecture(file);
            matrix_decrypted(m, direction);
        }
    }
}
