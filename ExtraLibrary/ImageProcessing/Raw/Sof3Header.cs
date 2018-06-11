using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class Sof3Header : Header
    {
        public UInt16 HeaderLength { get; set; }
        public byte SamplePrecision { get; set; }
        public UInt16 LinesNumber { get; set; }
        public UInt16 SamplesPerLineNumber { get; set; }
        public Sof3HeaderComponentInfo[] Components { get; set; }
    }
}

