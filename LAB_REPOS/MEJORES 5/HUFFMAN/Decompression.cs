using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace LAB_REPOS.MEJORES_5.HUFFMAN
{
    public class Decompression
    {
        public static byte[] finish_descompression(byte[] full)
        {
            byte[] lenght = { full[0], full[1], full[2], full[3] };
            int large = BitConverter.ToInt32(lenght, 0);
            List<N_minimo> mins = new List<N_minimo>();
            for (int x = 0; x < large; x++)
            {
                byte[] bytes_letter = { full[4 + x * 9] };
                byte[] bytes_left = { full[4 + x * 9 + 1], full[4 + x * 9 + 2], full[4 + x * 9 + 3], full[4 + x * 9 + 4] };
                byte[] bytes_right = { full[4 + x * 9 + 5], full[4 + x * 9 + 6], full[4 + x * 9 + 7], full[4 + x * 9 + 8] };

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes_letter);
                    Array.Reverse(bytes_left);
                    Array.Reverse(bytes_right);
                }
                byte letter2 = bytes_letter[0];
                int left2 = BitConverter.ToInt32(bytes_left, 0);
                int right2 = BitConverter.ToInt32(bytes_left, 0);

                mins.Add(new N_minimo(letter2, left2, right2));
            }
            List<byte> decompressed_list = new List<byte>();
            int actual_node = 0;
            byte[] compressed_bytes = new byte[full.Length - 4 - large * 9];
            for (int y = 0; y < compressed_bytes.Length; y++)
            {
                compressed_bytes[y] = full[4 + large * 9 + y];
            }
            BitArray bitA = new BitArray(compressed_bytes);
            for (int z = 0; z < bitA.Length; z++)
            {
                if (!bitA[z])
                {
                    actual_node = mins[actual_node].left;
                    if (mins[actual_node].left == mins[actual_node].right && mins[actual_node].left == -1)
                    {
                        decompressed_list.Add(mins[actual_node].letter);
                        actual_node = 0;
                    }
                    else if (mins[actual_node].left == mins[actual_node].right && mins[actual_node].left == -2)
                    {
                        z = bitA.Length - 1;
                    }
                }
                else
                {
                    actual_node = mins[actual_node].right;
                    if (mins[actual_node].left == mins[actual_node].right && mins[actual_node].left == -1)
                    {
                        decompressed_list.Add(mins[actual_node].letter);
                        actual_node = 0;
                    }
                    else if (mins[actual_node].left == mins[actual_node].right && mins[actual_node].left == -2)
                    {
                        z = bitA.Length - 1;
                    }
                }
            }
            return decompressed_list.ToArray();
        }
    }
}
