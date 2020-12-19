using Cryptopals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace CryptoPalsChallangeSetTests
{
    public class Set1
    {
        string[] _challange4Strings;

        public Set1()
        {
            SetupChallange4();
        }
        private void SetupChallange4()
        {
            _challange4Strings = File.ReadAllLines("4.txt");
        }

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

            var max = Utils.MostLiklyEnglishSingleCharKeyXOR(input);

            Assert.Equal(expected, max.Key);

        }

        [Fact]
        public void Challange3Marius()
        {
            var input = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";
            var expected = "Cooking MC's like a pound of bacon"; // after checking manually.. 
            KeyValuePair<string, double> max = Utils.MostLikelyXORDecryptMarius(input);

            Assert.Equal(expected, max.Key);

        }

        public void Challange31()
        {
            var input = "ETAOIN SHRDLUi";
            var key = "X"; // the key from challange 3

            var inputAsBytes = Utils.StringToByteArray(input);
            var keyAsBytes = Utils.StringToByteArray(key);
            var resultByteArray = Utils.calcXorLoopingKey(inputAsBytes, keyAsBytes);

            string asciiString = Encoding.ASCII.GetString(resultByteArray, 0, resultByteArray.Length);

        }

        // https://cryptopals.com/sets/1/challenges/4
        [Fact]
        public void Challange4()
        {
            var expected = "Now that the party is jumping\n";
            var resultDic = new Dictionary<string, double>();

            foreach (var stringToTest in _challange4Strings)
            {
                var x = Utils.MostLiklyEnglishSingleCharKeyXOR(stringToTest);
                resultDic.Add(x.Key, x.Value);
            }
            
            var highestValueResult = resultDic.Aggregate((l, r) => l.Value > r.Value ? l : r);
            Assert.Equal(expected, highestValueResult.Key);

        }

        [Fact]
        public void challange4Marius()
        {
            var expected = "Now that the party is jumping\n";
            var sortedresult = new SortedList< double, string>();

            foreach (var item in _challange4Strings)
            {
                var decrypted = Utils.MostLikelyXORDecryptMarius(item);
                if (!sortedresult.ContainsKey(decrypted.Value))
                {
                    sortedresult.Add(decrypted.Value, decrypted.Key);

                }
                //Debug.Print($"key {decrypted.Key} - value {decrypted.Value}");
            }

            var highestValueResult = sortedresult.First().Value;
            Assert.Equal(expected, highestValueResult);

        }

    }
}
