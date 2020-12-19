using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CryptoPalsChallangeSetTests
{
    public class OtherChallanges
    {
        [Fact]
        public void OtherChallangeFindHighestContinious()
        {
            long[] input = new long[4] { 4, -5, 8, 6 };

            var continuemLength = 0;


            for (int i = 1; i < input.Length; i++) // container length
            {
                var x = new List<int>();
                for (int j = 0; j < (input.Length ^ i + 1); j = j + i) // make container size jumps
                {
                    for (int y = j; y <= j + i; y++) // collect
                    {

                    }
                }
                //  var query =
                //  fruits.Select((fruit, index) =>
                //                  new { index, str = fruit.Substring(0, index) });
                //  fill containers
                //input.Select((item, index) =>
                //                    new long[] { item });

            }

        }
    }
}
