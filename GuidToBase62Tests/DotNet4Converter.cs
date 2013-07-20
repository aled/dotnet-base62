using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace GuidToBase62Tests {
    // Convert guids to base64 using the BigInteger class introduced in .NET 4
    public class DotNet4Converter {
        private const int radix = 62;
        private const string symbols = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private static void DivRem(BigInteger num, out BigInteger quot, out int rem) {
            quot = num / radix;
            rem = (int) (num - (radix * quot));
        }

        public static String ToBase62(byte[] bytes, bool zeroPadding = false)
        {
            var i = new BigInteger(bytes); // uses little endian representation
            var sb = new StringBuilder("0000000000000000000000");
            var pos = sb.Length;
            int rem;
            do {
                DivRem(i, out i, out rem);
                sb[--pos] = symbols[rem];
            } while (i > 0);

            if (zeroPadding) pos = 0;
            return sb.ToString(pos, sb.Length - pos);
        }
    }
}
