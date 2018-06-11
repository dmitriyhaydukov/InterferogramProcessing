using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class DhtHeaderTable
    {
        public const byte NUMBER_OF_CODES = 16;
                
        public byte TableClass { get; set; }

        public byte TableIndex { get; set; }
        
        public byte[] LengthHuffmanCodes { get; set; }

    }
}
