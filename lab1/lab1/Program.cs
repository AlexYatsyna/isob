using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var caesar = new Caesar();
            var vigenere = new Vigenere();

            Console.WriteLine("Key for Caesar(digit):");
            var key = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Key word for Vigenere:");
            var keyWord = Console.ReadLine();
            string text = "";

            using (FileStream fstream = File.OpenRead(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/ms.txt"))
            {
                byte[] buffer = new byte[fstream.Length];
                fstream.Read(buffer, 0, buffer.Length);
                text = Encoding.UTF8.GetString(buffer);
                Console.WriteLine($"Текст из файла: {text}");
            }

            var encryptedByCaesar= caesar.Encrypt(text, key);
            Console.WriteLine($"Encrypt with {key} : {encryptedByCaesar}");
            using (FileStream fstream = new FileStream(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/resCaesar.txt", FileMode.OpenOrCreate))
            {

                var result = encryptedByCaesar ;
                byte[] buffer = Encoding.UTF8.GetBytes(result);

                fstream.Write(buffer, 0, buffer.Length);

            }
            using (FileStream fstream = File.OpenRead(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/resCaesar.txt"))
            {
                byte[] buffer = new byte[fstream.Length];
                fstream.Read(buffer, 0, buffer.Length);
                encryptedByCaesar = Encoding.UTF8.GetString(buffer);

            }

            var decryptedByCaesar = caesar.Decrypt(encryptedByCaesar, key);
            Console.WriteLine($"Decrypted : {decryptedByCaesar}");

            var encryptedByVigenere = vigenere.Encrypt(text, keyWord);
            Console.WriteLine($"Encrypted with key word {keyWord}: {encryptedByVigenere}");
            using (FileStream fstream = new FileStream(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/resVigenere.txt", FileMode.OpenOrCreate))
            {

                var result = encryptedByVigenere;
                byte[] buffer = Encoding.UTF8.GetBytes(result);

                fstream.Write(buffer, 0, buffer.Length);

            }
            using (FileStream fstream = File.OpenRead(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/resVigenere.txt"))
            {
                byte[] buffer = new byte[fstream.Length];
                fstream.Read(buffer, 0, buffer.Length);
                encryptedByVigenere = Encoding.UTF8.GetString(buffer);

            }

            var decryptedByVigenere = vigenere.Decrypt(encryptedByVigenere, keyWord);
            Console.WriteLine($"Decrypted : {decryptedByVigenere}");



            Console.ReadKey();

        }
    }
}
