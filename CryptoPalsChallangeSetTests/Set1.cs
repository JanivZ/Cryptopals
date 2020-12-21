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
        string _challange6String = string.Empty;

        public Set1()
        {
            SetupChallange4();
            SetupChallange6();
        }

        private void SetupChallange6()
        {
            _challange6String = File.ReadAllText("6.txt");
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

            var inputAsBytes = Utils.HexStringToByteArray(input);
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

            var inputAsBytes = Utils.HexStringToByteArray(input);
            var xorKeyAsBytes = Utils.HexStringToByteArray(xorKey);

            var resultByteArray = Utils.CalcXor(inputAsBytes, xorKeyAsBytes);

            var resultAsHex = Utils.ByteArrayToHexString(resultByteArray);
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

            var inputAsBytes = Utils.HexStringToByteArray(input);
            var keyAsBytes = Utils.HexStringToByteArray(key);
            var resultByteArray = Utils.CalcXorLoopingKey(inputAsBytes, keyAsBytes);

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

        // https://cryptopals.com/sets/1/challenges/5
        [Fact]
        public void Challange5()
        {
            var input = "Burning 'em, if you ain't quick and nimble\nI go crazy when I hear a cymbal";
            var key = "ICE";
            var expected = "0b3637272a2b2e63622c2e69692a23693a2a3c6324202d623d63343c2a26226324272765272a282b2f20430a652e2c652a3124333a653e2b2027630c692b20283165286326302e27282f";

            var inputAsBytes = Encoding.ASCII.GetBytes(input);
            var keyAsBytes = Encoding.ASCII.GetBytes(key);

            var s = Utils.CalcXorLoopingKey(inputAsBytes, keyAsBytes);

            var xordString1 = Utils.ByteArrayToHexString(s);
            var xordString = Encoding.ASCII.GetString(s);
            
            

            Assert.Equal(expected, xordString1);
        }

        [Fact]
        public void Challange6()
        {
            int minKeySize = 2;
            int maxKeySize = 40;
            var resultDic = new Dictionary<int, int>(); // keySize, Distance
            var listOfByteArraysToTranspose = new List<byte[]>();
            var listOfByteArraysAfterTranspose = new List<byte[]>();
            var listOfSingleCharKeys = new List<string>();


            // convert from base 64
            var input = Convert.FromBase64String(_challange6String);

            // try to understand keySize
            //  grab the 3 lowest distance keySizes 
            for (int i = minKeySize; i <= maxKeySize; i++)
            {
                var firstKeySizeWorthOfBytes = input.Take(i).ToArray();
                var secondKeySizeWorthOfBytes = input.Skip(i).Take(i).ToArray();

                var hammingDistance = Utils.HammingDistance(firstKeySizeWorthOfBytes, secondKeySizeWorthOfBytes);
                var normalized = hammingDistance / i;

                resultDic.Add(i, normalized);
            }
            var probableKeySizes = resultDic.OrderBy(x => x.Value).Take(3).ToArray();

            for (int i = 0; i < probableKeySizes.Length - 1; i++) // foreach probableKeySize length
            {
                listOfByteArraysToTranspose.Clear();

                // break up cyphertext into keySize blocks
                var keySizeBlocksOfCypherText = _challange6String.ChunksOfSize(probableKeySizes[i].Key).ToList();
                keySizeBlocksOfCypherText.ForEach(x => listOfByteArraysToTranspose.Add(
                    Encoding.UTF8.GetBytes(x)));

                var transposedByteArray = new byte[listOfByteArraysToTranspose.Count];

                // transpose  - make a block that is the first byte of every block, and a block that is the second byte of every block, and so on.
                for (int t = 0; t <= probableKeySizes[i].Key - 1; t++) // loop for index in byte array
                {
                    transposedByteArray = new byte[listOfByteArraysToTranspose.Count ]; // ???wtf

                    for (int j = 0; j < listOfByteArraysToTranspose.Count ; j++) // loop for all blocks
                    {
                        transposedByteArray[j] = listOfByteArraysToTranspose[j][t];
                    }
                    
                    listOfByteArraysAfterTranspose.Add(transposedByteArray);
                    
                    //transposedByteArray = new byte[listOfByteArraysToTranspose.Count - 1];
                    //transposedByteArray[j] = listOfByteArraysToTranspose.Select(x => new () { x[j] }));
                    //listOfByteArraysAfterTranspose.Add();
                }


                // find probable single char xor key for each of the transposed byte arrays 
                foreach (var bytesToFindSingleCharKey in listOfByteArraysAfterTranspose)
                {
                    listOfSingleCharKeys.Add(Utils.MostLiklyEnglishSingleCharKeyXOR(bytesToFindSingleCharKey));
                }
            }

            var potentialKey = listOfSingleCharKeys.ToString();
            //var keyAsBytes = Utils.s

        }

        [Fact]
        public void Challange6PreReq()
        {
            var input1 = "this is a test";
            var input2 = "wokka wokka!!!";
            var expected = 37;

            var hammingDistance = Utils.HammingDistance(input1, input2);

            Assert.Equal(expected, hammingDistance);
        }

    }
}
