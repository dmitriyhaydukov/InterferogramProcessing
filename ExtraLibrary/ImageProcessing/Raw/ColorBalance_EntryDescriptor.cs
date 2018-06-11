using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class ColorBalance_Descriptor
    {
        public const string RGGB_LEVEK_AS_SHOT = "RGGB_Level_As_Shot";

        public Int32 Offset { get; set; }
        public string Name { get; set; }
        public TagType Type { get; set; }
        public Int32 Length { get; set; }

        public ColorBalance_Descriptor(Int32 offset, string name, TagType type, int length)
        {
            this.Offset = offset;
            this.Name = name;
            this.Type = type;
            this.Length = length;
        }

        public static List<ColorBalance_Descriptor> GetDescriptorList()
        {
            List<ColorBalance_Descriptor> list = new List<ColorBalance_Descriptor>();

            list.Add(new ColorBalance_Descriptor(0x003f, RGGB_LEVEK_AS_SHOT, TagType.SignedShort, 4));

            return list;
        }
    }
}
