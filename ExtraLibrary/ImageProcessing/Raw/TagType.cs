using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public enum  TagType : short
    {
        UnsignedChar = 1,
        StringWithEndingZero = 2,
        UnsignedShort = 3,
        UnsignedLong = 4,
        UnsignedRationnal = 5,
        SignedChar = 6,
        ByteSequence = 7,
        SignedShort = 8,
        SignedLong = 9,
        SignedRationnal = 10,
        Float_4_Bytes = 11,
        Float_8_Bytes = 12
     }
}
