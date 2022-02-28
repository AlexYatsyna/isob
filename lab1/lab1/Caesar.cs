using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class Caesar
    {
        readonly string alf;

        public Caesar(string alf = null)
        {
            var defaultAlf = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            defaultAlf += defaultAlf.ToLower();

            this.alf = string.IsNullOrEmpty(alf) ? defaultAlf : alf;
        }

        private string Encode(string text, int offset)
        {
            var fullAlf = alf + alf.ToLower();
            var alfSize = fullAlf.Length;
            var result = "";

            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var index = fullAlf.IndexOf(c);

                if (index == -1)
                {
                    result += c.ToString();
                }
                else
                {
                    result += fullAlf[(alfSize + index + offset) % alfSize].ToString();
                }
            }

            return result;
        }

        public string Encrypt(string text, int offset) => Encode(text, offset);

        public string Decrypt(string text, int offset) => Encode(text, -offset);
    }
}
