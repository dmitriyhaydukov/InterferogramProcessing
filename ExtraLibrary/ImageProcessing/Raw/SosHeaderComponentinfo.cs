using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class SosHeaderComponentInfo
    {
        public byte ComponentNumber { get; set; }
        public byte DCtable { get; set; }
        public byte ACtable { get; set; }

    }
}
