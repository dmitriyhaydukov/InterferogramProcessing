using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public static class BitArrayExtension
    {
        public static bool IsEqual(this BitArray array, BitArray otherArray)
        {
            if (array.Length != otherArray.Length) return false;

            for (int k = 0; k < array.Length; k++)
            {
                if (array[k] != otherArray[k]) return false;
            }

            return true;
        }

        public static BitArray SubArray(this BitArray array, Int32 startIndex, Int32 count)
        {
            BitArray resArray = new BitArray(count);
            int finalIndex = startIndex + count - 1;

            for (int i = 0, k = startIndex; k <= finalIndex; k++, i++)
            {
                resArray[i] = array[k];
            }

            return resArray;
        }

        public static BitArray Reverse(this BitArray bits)
        {
            int len = bits.Count;
            BitArray a = new BitArray(bits);
            BitArray b = new BitArray(bits);

            for (int i = 0, j = len - 1; i < len; ++i, --j)
            {
                a[i] = a[i] ^ b[j];
                b[j] = a[i] ^ b[j];
                a[i] = a[i] ^ b[j];
            }

            return a;
        }
        
        public static string ToBitString(this BitArray bits)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < bits.Count; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }

        public static bool IsAllTrue(this BitArray bits)
        {
            for (int k = 0; k < bits.Length; k++)
            {
                if (!bits[k]) return false;
            }
            return true;
        }

        public static bool IsAllFalse(this BitArray bits)
        {
            for (int k = 0; k < bits.Length; k++)
            {
                if (bits[k]) return false;
            }

            return true;
        }
    }
}
