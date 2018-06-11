using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    class DecodingHelper
    {
        public const byte MAX_DIFFERENCE_CODE_LENGTH = 15;
        
        public static byte[] GetCorrectedScanDataBytes(byte[] bytes)
        {
                       
            List<byte> resBytesList = new List<byte>();
            int index = 0;
            while (index < bytes.Length)
            {
                byte value = bytes[index];
                resBytesList.Add(value);
                index++;
                if (value == 0xFF)
                {
                    byte nextValue = bytes[index];
                    if (nextValue == 0x00)
                    {
                        index++;
                    }
                }
            }
            
            byte[] resArray = resBytesList.ToArray();
            ByteOperationHelper.ReverseBits(resArray);

            return resArray;
        }

        public static Dictionary<String, Int32>[] GetDifferenceCodeValueMap()
        {
            Dictionary<String, Int32>[] resArray = new Dictionary<String, Int32>[MAX_DIFFERENCE_CODE_LENGTH + 1];
            
            //Difference code length '0' -> Difference value '0'
            Dictionary<String, Int32> zeroDifferenceCodeValueDictionary = new Dictionary<String, Int32>();
            zeroDifferenceCodeValueDictionary.Add(String.Empty, 0);
            resArray[0] = zeroDifferenceCodeValueDictionary;
            
            Int32 differenceCodesCount, positiveValuesCount, negativeValuesCount;
            Int32 maxPositiveValue = 0, minPositiveValue = 0;
            Int32 minNegativeValue, maxNegativeValue;

            for (byte differenceCodeLength = 1; differenceCodeLength <= MAX_DIFFERENCE_CODE_LENGTH; differenceCodeLength++)
            {
                differenceCodesCount = Convert.ToInt32(Math.Pow(2, differenceCodeLength));
                positiveValuesCount = negativeValuesCount = differenceCodesCount / 2;

                minPositiveValue = maxPositiveValue + 1;
                maxPositiveValue = minPositiveValue + positiveValuesCount - 1;

                minNegativeValue = -maxPositiveValue;
                maxNegativeValue = -minPositiveValue;

                Int32 differenceCode = 0;
                Dictionary<String, Int32> codeValueDictionary = new Dictionary<String, Int32>();

                //Negative values
                for (Int32 value = minNegativeValue; value <= maxNegativeValue; value++)
                {
                    BitArray bitArray = CreateBitArray(differenceCodeLength, differenceCode);
                    codeValueDictionary.Add(bitArray.ToBitString(), value);
                    differenceCode++;
                }
                
                //Positive values
                for (Int32 value = minPositiveValue; value <= maxPositiveValue; value++)
                {
                    BitArray bitArray = CreateBitArray(differenceCodeLength, differenceCode);
                    codeValueDictionary.Add(bitArray.ToBitString(), value);
                    differenceCode++;
                }

                resArray[differenceCodeLength] = codeValueDictionary;

            }

            return resArray;
        }

        private static BitArray CreateBitArray(byte differenceCodeLength, Int32 differenceCode)
        {
            return (new BitArray(new Int32[] { differenceCode })).SubArray(0, differenceCodeLength).Reverse();
        }
    }
}
