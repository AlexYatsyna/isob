using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class DES
    {
        private static int sizeOfBlock = 64;
        private static int sizeOfChar = 8;
        private static int shiftKey = 2;
        private static int quantityOfRounds = 16;
        private static string EncodeKey { get; set; }
        private static string DecodeKey { get; set; }
        static string[] Blocks;

        private static string ExpendingStringSize(string input)
        {
            var currInput = new StringBuilder(input);

            while ((currInput.Length * sizeOfChar) % sizeOfBlock != 0)
            {
                currInput.Append(" ");
            }

            return currInput.ToString();
        }

        private static void CutStringIntoBlocks(string input)
        {
            Blocks = new string[(input.Length * sizeOfChar) / sizeOfBlock];
            var lengthOfBlock = input.Length / Blocks.Length;

            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = input.Substring(i * lengthOfBlock, lengthOfBlock);
                Blocks[i] = StringToBinary(Blocks[i]);
            }
        }

        private static string StringToBinary(string input)
        {
            var result = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                var charB = new StringBuilder(Convert.ToString(input[i], 2));

                while (charB.Length < sizeOfChar)
                {
                    charB.Insert(0,"0");
                }

                result.Append(charB.ToString());
            }
            return result.ToString();
        }

        private static void CutBinaryStringIntoBlocks(string input)
        {
            Blocks = new string[input.Length / sizeOfBlock];

            var lengthOfBlock = input.Length / Blocks.Length;

            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = input.Substring(i * lengthOfBlock, lengthOfBlock);
            }
        }

        private static string ExpendingKey(string key, int lengthKey)
        {
            var result = new StringBuilder();
            if (key.Length > lengthKey)
            {
                result.Append(key.Substring(0, lengthKey));
            }
            else
            {
                var zeros = new StringBuilder(key);
                while (zeros.Length < lengthKey)
                {
                    zeros.Insert(0,"0");
                }

                result.Append(zeros.ToString());
            }

            return result.ToString();
        }

        private static string EncodeOneRound(string input, string key)
        {
            var L = input.Substring(0, input.Length / 2);
            var R = input.Substring(input.Length / 2, input.Length / 2);

            return (R + XOR(L, f(R, key)));
        }

        private static string f(string val1, string val2)
        {
            return XOR(val1, val2);
        }

        private static string XOR(string val1, string val2)
        {
            var result = new StringBuilder();

            for (var i = 0; i < val1.Length; i++)
            {
                bool a = Convert.ToBoolean(Convert.ToInt32(val1[i].ToString()));
                bool b = Convert.ToBoolean(Convert.ToInt32(val2[i].ToString()));

                if (a ^ b)
                    result.Append("1");
                else
                    result.Append("0");
            }
            return (result.ToString());
        }

        private static string DecodeOneRound(string input, string key)
        {
            var L =input.Substring(0,input.Length / 2);
            var R = input.Substring(input.Length /2, input.Length / 2);

            return XOR(f(L, key), R) + L;
        }

        private static string NextKey(string key)
        {
            for (int i = 0; i < shiftKey; i++)
            {
                key = key[key.Length - 1] + key;
                key = key.Remove(key.Length - 1);
            }

            return key;
        }

        private static string PrevKey(string key)
        {
            for (int i = 0; i < shiftKey; i++)
            {
                key = key + key[0];
                key = key.Remove(0, 1);
            }

            return key;
        }

        private static string FromBinaryToString(string input)
        {
            var result = new StringBuilder();

            while (input.Length > 0)
            {
                var charB = input.Substring(0, sizeOfChar);
                input = input.Remove(0, sizeOfChar);

                var a = 0;
                var degree = charB.Length - 1;

                foreach (char c in charB)
                    a += Convert.ToInt32(c.ToString()) * (int)Math.Pow(2, degree--);

                result.Append(((char)a).ToString());
            }

            return result.ToString();
        }

        public static string Encrypt(string input, string key)
        {
            input = ExpendingStringSize(input);

            CutStringIntoBlocks(input);
            EncodeKey = key;
            key = ExpendingKey(key, input.Length / (2 * Blocks.Length));

            key = StringToBinary(key);

            for (int i = 0; i < quantityOfRounds; i++)
            {
                for (int j = 0; j < Blocks.Length; j++)
                {
                    Blocks[j] = EncodeOneRound(Blocks[j], key);
                }
                key = NextKey(key);
            }

            key = PrevKey(key);

            DecodeKey = FromBinaryToString(key);

            var result = new StringBuilder();
            for (int i = 0; i< Blocks.Length;i++)
            {
                result.Append(FromBinaryToString(Blocks[i].ToString()));
            }


            return result.ToString();
        }

        public static string Decrypt(string input, string key ="")
        {
            ExpendingStringSize(input);

            CutStringIntoBlocks(input);

            key = ExpendingKey(key, input.Length / (2 * Blocks.Length));

            key = StringToBinary(key);

            for (int i = 0; i < quantityOfRounds; i++)
            {

                key = NextKey(key);
            }

            key = FromBinaryToString(PrevKey(key));


            key = StringToBinary(key);
            input = StringToBinary(input);



            for (int i = 0; i < quantityOfRounds; i++)
            {
                for (int j = 0; j < Blocks.Length; j++)
                {
                    Blocks[j] = DecodeOneRound(Blocks[j], key);
                }
                key = PrevKey(key);
            }

            var result = new StringBuilder();
            for (int i = 0; i < Blocks.Length; i++)
            {
                result.Append(FromBinaryToString(Blocks[i].ToString()));
            }


            return result.ToString();

        }

        public static bool CheckTime(DateTime time1, DateTime time2, long duration)
        {
            TimeSpan timeSpan = new TimeSpan(duration);
            if (time2 < time1 + timeSpan)
                return true;
            else
                return false;
        }
    }


}
