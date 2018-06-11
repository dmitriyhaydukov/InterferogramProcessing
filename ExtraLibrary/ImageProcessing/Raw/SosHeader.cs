using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class SosHeader : Header
    {
        public UInt16 HeaderLength { get; set; }
        public SosHeaderComponentInfo[] Components { get; set; }
    }
}
