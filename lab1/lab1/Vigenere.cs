using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class Vigenere
    {
        readonly string alf;

        public Vigenere(string alf = null)
        {
            var defaultAlf = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            defaultAlf += defaultAlf.ToLower();

            this.alf = string.IsNullOrEmpty(alf) ? defaultAlf : alf;
        }

        private string GetAllPass(string password, int textLength)
        {
            var temp = password;

            while (temp.Length < textLength)
            {
                temp += temp;
            }

            var result = temp.Substring(0, textLength);

            return result;
        }

        private string Encode(string text, string password, bool isEncrypt = true)
        {
            var allPass = GetAllPass(password, text.Length);
            var alfSize = alf.Length;
            var result = "";

            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var index = alf.IndexOf(c);
                var cp = allPass[i];
                var passIndex = alf.IndexOf(cp);

                passIndex *= isEncrypt ? 1 : -1;

                if (index == -1)
                {
                    result += c.ToString();
                }
                else
                {
                    result += alf[(alfSize + index + passIndex) % alfSize].ToString();
                }
            }

            return result;
        }

        public string Encrypt(string text, string password) => Encode(text, password);

        public string Decrypt(string text, string password) => Encode(text, password, false);
    }
}
