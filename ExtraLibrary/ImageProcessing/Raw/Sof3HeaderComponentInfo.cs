using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class Sof3HeaderComponentInfo
    {
        public byte ComponentNumber { get; set; }
        public byte HorizontalSamplingFactor { get; set; }
        public byte VerticalSamplingFactor { get; set; }
        public byte QuantizationTable { get; set; }
    }
}

