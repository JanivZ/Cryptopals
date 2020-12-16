using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptopals
{
    public class Utils
    {
        static Dictionary<string, double> _EnglishLetterFrequency = new Dictionary<string, double>();

        static Utils()
        {
            SetupEnglishLetterDictionary();
        }

        // from: http://practicalcryptography.com/cryptanalysis/letter-frequencies-various-languages/english-letter-frequencies/
        private static void SetupEnglishLetterDictionary()
        {
            _EnglishLetterFrequency.Add("A", 8.55);
            _EnglishLetterFrequency.Add("B", 1.60);
            _EnglishLetterFrequency.Add("C", 3.16);
            _EnglishLetterFrequency.Add("D", 3.87);
            _EnglishLetterFrequency.Add("E",12.10);
            _EnglishLetterFrequency.Add("F", 2.18);
            _EnglishLetterFrequency.Add("G", 2.09);
            _EnglishLetterFrequency.Add("H", 4.96);
            _EnglishLetterFrequency.Add("I", 7.33);
            _EnglishLetterFrequency.Add("J", 0.22);
            _EnglishLetterFrequency.Add("K", 0.81);
            _EnglishLetterFrequency.Add("L", 4.21);
            _EnglishLetterFrequency.Add("M", 2.53);
            _EnglishLetterFrequency.Add("N", 7.17);
            _EnglishLetterFrequency.Add("O", 7.47);
            _EnglishLetterFrequency.Add("P", 2.07);
            _EnglishLetterFrequency.Add("Q", 0.10);
            _EnglishLetterFrequency.Add("R", 6.33);
            _EnglishLetterFrequency.Add("S", 6.73);
            _EnglishLetterFrequency.Add("T", 8.94);
            _EnglishLetterFrequency.Add("U", 2.68);
            _EnglishLetterFrequency.Add("V", 1.06);
            _EnglishLetterFrequency.Add("W", 1.83);
            _EnglishLetterFrequency.Add("X", 0.19);
            _EnglishLetterFrequency.Add("Y", 1.72);
            _EnglishLetterFrequency.Add("Z", 0.11);
        }

        // from: https://stackoverflow.com/a/311179/606724
        public static string ByteArrayToString(byte[] ba)
        {
            //return BitConverter.ToString(ba).Replace("-", ""); // this doesnt work for the comparison in the tests 
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        // from: https://stackoverflow.com/a/311179/606724
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        // https://stackoverflow.com/a/11743162/606724
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        // https://stackoverflow.com/a/11743162/606724
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static byte[] calcXor(byte[] a, byte[] b)
        {
            ////char[] charAArray = a.ToCharArray();
            //char[] charBArray = b.ToCharArray();
            byte[] result = new byte[1024];
            int len = a.Length;

            // Set length to be the length of the shorter string
            //if (a.Length > b.Length)
            //    len = b.Length - 1;
            //else
            //    len = a.Length ;

            for (int i = 0; i < len; i++)
            {
                var z = a[i] ^ b[i];
                result[i] = (byte)(z); // Error here
            }

            return result;
        }

        public static byte[] calcXorLoopingKey(byte[] a, byte[] key)
        {
            ////char[] charAArray = a.ToCharArray();
            //char[] charBArray = b.ToCharArray();
            byte[] result = new byte[a.Length];
            int len = a.Length;

            // Set length to be the length of the shorter string
            //if (a.Length > b.Length)
            //    len = b.Length - 1;
            //else
            //    len = a.Length ;

            for (int i = 0; i < len; i++)
            {
                var z = a[i] ^ key[i % key.Length];
                result[i] = (byte)(z); // Error here
            }

            return result;
        }

        public static double EnglishLetterFrequencyScore(string inputChar)
        {
            var toScore = inputChar.ToUpper();
            if (!_EnglishLetterFrequency.ContainsKey(toScore))
            {
                return 0;
            }
            return _EnglishLetterFrequency[toScore];
        }

    }
}
