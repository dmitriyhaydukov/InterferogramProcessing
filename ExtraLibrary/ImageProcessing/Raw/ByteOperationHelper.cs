using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class ByteOperationHelper
    {
        // Reverses bits in each byte in the array 
        public static void ReverseBits(byte[] bytes)
        {
            // Precompute the value of each reversed byte 
            byte[] reversed = new byte[256];
            for (int i = 0; i < 256; i++) reversed[i] = ReverseBits((byte)i);

            // Reverse each byte in the input 
            for (int i = 0; i < bytes.Length; i++) bytes[i] = reversed[bytes[i]];
        }

        // Reverses bits in a byte 
        public static byte ReverseBits(byte b)
        {
            int rev = (b >> 4) | ((b & 0xf) << 4);
            rev = ((rev & 0xcc) >> 2) | ((rev & 0x33) << 2);
            rev = ((rev & 0xaa) >> 1) | ((rev & 0x55) << 1);

            return (byte)rev;
        }
    }
}
