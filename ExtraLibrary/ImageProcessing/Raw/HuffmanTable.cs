using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class HuffmanTable
    {
        public byte TableClass { get; set; }
        public byte TableIndex { get; set; }
        
        public byte[] LengthHuffmanCodes { get; set; }
        public byte[] ValueHuffmanCodes { get; set; }

        public HuffmanTable()
        {
            
        }

        public Dictionary<string, byte> GetHuffmanCodeValueMap()
        {
            Dictionary<string, byte> huffmanCodeValueDictionary = new Dictionary<string, byte>();

            UInt16 code = 0;
            UInt32 valueIndex = 0;

            for (int i = 0; i < LengthHuffmanCodes.Length; i++)
            {
                for (int k = 0; k < LengthHuffmanCodes[i]; k++)
                {
                    string codeString = Convert.ToString(code, 2).PadLeft(i + 1, '0');
                    huffmanCodeValueDictionary[codeString] = ValueHuffmanCodes[valueIndex];
                    code++;
                    valueIndex++;
                }
                code <<= 1;
            }

            return huffmanCodeValueDictionary;
        }
    }
}
