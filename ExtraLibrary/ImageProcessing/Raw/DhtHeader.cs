using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class DhtHeader : Header
    {
        public const UInt32 LENGTH_NUMBER_HUFFMAN_CODES_COUNT = 16;
        
        public UInt16 HeaderLength { get; set; }
        public HuffmanTable[] HuffmanTables { get; set; }
    }
}