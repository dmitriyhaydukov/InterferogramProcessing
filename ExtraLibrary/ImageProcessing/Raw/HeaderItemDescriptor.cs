using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class HeaderItemDescriptor
    {
        #region Constants

        public const string ITEM_BYTE_ORDER = "Byte order";
        public const string ITEM_TIFF_MAGIC_WORD = "TIFF magic word";
        public const string ITEM_TIFF_OFFSET = "TIFF offset";
        public const string ITEM_CR2_MAGIC_WORD = "CR2 magic word";
        public const string ITEM_CR2_MAJOR_VERSION = "CR2 major version";
        public const string ITEM_CR2_MINOR_VERSION = "CR2 minor version";
        public const string ITEM_RAW_IFD_OFFSET = "RAW IFD offset";

        #endregion Constants
        
        public UInt32 Offset { get; set; }
        public UInt32 Length { get; set; }
        public Type Type { get; set; }
        public string Name { get; set; }
                
        public HeaderItemDescriptor(string name, UInt32 offset, UInt16 length, Type type)
        {
            this.Name = name;
            this.Offset = offset;
            this.Length = length;
            this.Type = type;
        }

        public static List<HeaderItemDescriptor> GetHeaderItemDescriptorList()
        {
            List<HeaderItemDescriptor> list = new List<HeaderItemDescriptor>();

            list.Add(new HeaderItemDescriptor(ITEM_BYTE_ORDER,         0x0000, 2, typeof(char)));
            list.Add(new HeaderItemDescriptor(ITEM_TIFF_MAGIC_WORD,    0x0002, 1, typeof(short)));
            list.Add(new HeaderItemDescriptor(ITEM_TIFF_OFFSET,        0x0004, 1, typeof(int)));
            list.Add(new HeaderItemDescriptor(ITEM_CR2_MAGIC_WORD,     0x0008, 1, typeof(short)));
            list.Add(new HeaderItemDescriptor(ITEM_CR2_MAJOR_VERSION,  0x000A, 1, typeof(char)));
            list.Add(new HeaderItemDescriptor(ITEM_CR2_MINOR_VERSION,  0x000B, 1, typeof(char)));
            list.Add(new HeaderItemDescriptor(ITEM_RAW_IFD_OFFSET,     0x000C, 1, typeof(int)));

            return list;
        }
    }
}