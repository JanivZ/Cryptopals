using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptopals
{
    public static class Extensions
    {
        public static IEnumerable<string> ChunksOfSize(this string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
                yield return str.Substring(i, chunkSize);
        }
    }
}
