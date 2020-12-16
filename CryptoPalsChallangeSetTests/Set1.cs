using Cryptopals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CryptoPalsChallangeSetTests
{
    public class Set1
    {
        // https://cryptopals.com/sets/1/challenges/1
        [Fact]
        public void Challange1()
        {
            string input = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";
            string expected = "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t";

            var inputAsBytes = Utils.StringToByteArray(input);
            var inputsAsBase64 = Convert.ToBase64String(inputAsBytes);

            Assert.Equal(expected, inputsAsBase64);
        }

        // https://cryptopals.com/sets/1/challenges/2
        [Fact]
        public void Challange2()
        {
            var input = "1c0111001f010100061a024b53535009181c";
            var xorKey = "686974207468652062756c6c277320657965";
            var expected = "746865206b696420646f6e277420706c6179";

            var inputAsBytes = Utils.StringToByteArray(input);
            var xorKeyAsBytes = Utils.StringToByteArray(xorKey);

            var resultByteArray = Utils.calcXor(inputAsBytes, xorKeyAsBytes);

            var resultAsHex = Utils.ByteArrayToString(resultByteArray);
            var resultAsHexRelavantPart = resultAsHex.Substring(0, expected.Length);

            Assert.Equal(expected, resultAsHexRelavantPart);
            
        }

        // https://cryptopals.com/sets/1/challenges/3
        // help from: https://cedricvanrompay.gitlab.io/cryptopals/challenges/01-to-08.html
        // and help from: https://blog.mattclemente.com/2019/07/12/modulus-operator-modulo-operation.html#rotating-through-limited-options-circular-array
        // and help from: https://stackoverflow.com/a/2806074/606724

        [Fact]
        public void Challange3()
        {
            var input = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";
            var expected = "Cooking MC's like a pound of bacon"; // after checking manually.. 

            var resultList = new Dictionary<string, double>();
            

            var inputAsBytes = Utils.StringToByteArray(input);

            for (int i = 0; i < 256; i++)
            {
                //var key = Convert.ToString(i, 16);
                var key = i.ToString("X2");
                var keyAsBytes = Utils.StringToByteArray(key);
                var resultByteArray = Utils.calcXorLoopingKey(inputAsBytes, keyAsBytes);
                
                // ASCII conversion - string from bytes  
                string asciiString = Encoding.ASCII.GetString(resultByteArray, 0, resultByteArray.Length);

                // cheat ? if this is a sentence it should have space? 
                if (!asciiString.Contains(" "))
                {
                    continue;
                }

                var letterFrequencyScore = ScoreString(asciiString);
                if (!resultList.ContainsKey(asciiString))
                {
                    resultList.Add(asciiString, letterFrequencyScore);
                }
            }

            var max = resultList.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            Assert.Equal(expected, max);

        }

        /// <summary>
        /// this isnt the way to do it - n-grams ?
        /// maybe need to remove penalty for spaces ? 
        /// </summary>
        /// <param name="asciiString"></param>
        /// <returns></returns>
        private double ScoreString(string asciiString)
        {
            double scoreToRetrun = 0;

            for (int i = 0; i < asciiString.Length - 1; i++)
            {
                var letterScore = Utils.EnglishLetterFrequencyScore(asciiString[i].ToString());
                if (letterScore == 0)
                {
                    letterScore = -10; // large penalty for non letters as this ( challange 3 ) is supposed to be english
                }
                scoreToRetrun = scoreToRetrun + letterScore;
            }

            return scoreToRetrun;
        }
    }
}
