using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace Coding
{
    class Program
    {
        static byte vactual = 0; 
        static Dictionary<char, byte> stcoding = new Dictionary<char, byte>();
        static Dictionary<byte,char > stdecoding = new Dictionary<byte, char>();

        static String ReadFile(String s)
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(s))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadToEnd();
                    return line;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return null;
            }
        }

        static void WriteFile(String s,byte[] a)
        {
            File.WriteAllBytes(s,a);
        }

        static byte Ccodify(char l)
        {
            byte result;

            if (stcoding.TryGetValue(l, out result)) return result;
            else
            {
                stcoding.Add(l, vactual);
                stdecoding.Add(vactual, l);
                byte vanterior = vactual;
                vactual += 1;
                return vanterior;
            }

        }

        static char Cdecodify(byte n)
        {
            char result;
            if (stdecoding.TryGetValue(n, out result)) return result;
            else
            {
                return '?';
            }
        }

        static byte[] Scodify(String s)
        {
            byte[] cvalues = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                cvalues[i] = Ccodify(s[i]);
            }
            return cvalues;

        }

        static string Sdecodify(byte[] c)
        {
            char[] cvalues = new char[c.Length];

            for (int i = 0; i < c.Length; i++)
            {
                cvalues[i] = Cdecodify(c[i]);
            }
            string svalues = new string(cvalues);
            return svalues;

        }

        static byte[] Compress(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }

        static void CompressFile(String origin,String destiny)
        {
            string message = ReadFile(origin);
            byte[] mcode = new byte[message.Length];
            mcode = Scodify(message);
            byte[] cmcode = Compress(mcode);

            WriteFile(destiny, cmcode);
        }

        static string DecompressFile(String origin)
        {
            byte[] lecture = File.ReadAllBytes(origin);
            byte[] dlecture = Decompress(lecture);
            string result = Sdecodify(dlecture);
            return result;

        }

        static void CompressString(String message, String destiny)
        {
            
            byte[] mcode = new byte[message.Length];
            mcode = Scodify(message);
            byte[] cmcode = Compress(mcode);

            WriteFile(destiny, cmcode);
        }

        static void Main(string[] args)
        {
 
            Console.WriteLine("Probando codificacion");

           // CompresFile(@"C: \Users\mprada\text.txt", @"C: \Users\mprada\codified.txt");
           // DecompressFile(@"C: \Users\mprada\codified.txt");
           
            //Para probar, cambiar ruta de destino                                    !
            CompressString("Its time to go to argentina, fly number 4568341", @"C: \Users\mprada\Stringcodified.txt");

            Console.WriteLine(DecompressFile(@"C: \Users\mprada\Stringcodified.txt"));

            Console.ReadKey();
            
        }
    }
}
