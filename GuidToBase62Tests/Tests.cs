using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using GuidToBase62;

namespace GuidToBase62Tests {
    class Tests {
        static void Main(string[] args) {
            var random = new Random(123);
            int iterations = 20000;
            var testData = new byte[iterations][];

            for (int i = 0; i < iterations; i++) {
                testData[i] = new byte[16];
                random.NextBytes(testData[i]);
            }

            // A couple of tricky ones
            testData[0] = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            testData[1] = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xEF };
            
            // Need to append a zero byte to force unsigned for BigInteger
            var testData2 = new byte[iterations][];
            for (int i = 0; i < iterations; i++)
            {
                testData2[i] = new byte[testData[i].Length + 1];
                testData2[i][testData[i].Length] = 0;
                Array.Copy(testData[i], 0, testData2[i], 0, testData[i].Length);
            }

            long time = 0, timeBigInteger = 0;
            long start, end;
            for (int i = 0; i < iterations; i++)
            {
                start = DateTime.Now.Ticks;
                var str = testData[i].ToBase62(zeroPadding: true);
                end = DateTime.Now.Ticks;
                //Console.WriteLine(str);
                time += end - start;

                start = DateTime.Now.Ticks;
                var strBigInteger = DotNet4Converter.ToBase62(testData2[i], zeroPadding: true);
                end = DateTime.Now.Ticks;
                //Console.WriteLine(strBigInteger);
                timeBigInteger += end - start;

                if (str != strBigInteger) {
                    Console.WriteLine("Tests Failed: Expected {0} but was {1}.", strBigInteger, str);
                    return;
                }
            }
       
            Console.WriteLine(String.Format("Tests OK: Time: {0} ms, using BigInteger={1} ms\n", time / 10000.0, timeBigInteger / 10000.0));
        }
    }
}
