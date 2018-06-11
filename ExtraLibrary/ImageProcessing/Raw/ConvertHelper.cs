using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class ConvertHelper
    {
        public static Int16 ConvertToInt16(byte[] bytes)
        {
            return BitConverter.ToInt16(bytes, 0);
        }

        public static Int16 ConvertToInt16(byte[] bytes, bool isReverse)
        {
            if (isReverse)
            {
                return BitConverter.ToInt16(bytes.Reverse().ToArray(), 0);
            }
            else
            {
                return BitConverter.ToInt16(bytes, 0);
            }
        }

        public static UInt16 ConvertToUInt16(byte[] bytes)
        {
            return BitConverter.ToUInt16(bytes, 0);
        }
       
        public static UInt16 ConvertToUInt16(byte[] bytes, bool isReverse)
        {
            if (isReverse)
            {
                return BitConverter.ToUInt16(bytes.Reverse().ToArray(), 0);
            }
            else
            {
                return BitConverter.ToUInt16(bytes, 0);
            }
        }

        public static Int32 ConvertToInt32(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes, 0);
        }

        public static UInt32 ConvertToUInt32(byte[] bytes)
        {
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static Int64 ConvertToInt64(byte[] bytes)
        {
            return BitConverter.ToInt64(bytes, 0);
        }
        
        public static UInt64 ConvertToUInt64(byte[] bytes)
        {
            return BitConverter.ToUInt64(bytes, 0);
        }
        
        public static string ConvertToString(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
