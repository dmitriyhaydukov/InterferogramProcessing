using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class IFD_3_EntryDescriptor
    {
        public const string COMPRESSION = "compression";
        public const string STRIP_OFFSET = "StripOffset";
        public const string STRIP_BYTE_COUNTS = "StripByteCounts";
        public const string CR2_SLICE = "cr2 slice";

        public const string NAME_1 = "Name1";
        public const string NAME_2 = "Name2";
        public const string NAME_3 = "Name3";
        

        public Int32 TagID { get; set; }
        public string Name { get; set; }
        public TagType TagType { get; set; }
        public Int32 Length { get; set; }

        public IFD_3_EntryDescriptor(Int32 tagID, string name, TagType tagType, int length)
        {
            this.TagID = tagID;
            this.Name = name;
            this.TagType = tagType;
            this.Length = length;
        }

        public static List<IFD_3_EntryDescriptor> GetEntryDescriptorList()
        {
            List<IFD_3_EntryDescriptor> list = new List<IFD_3_EntryDescriptor>();

            list.Add(new IFD_3_EntryDescriptor(0x0103, COMPRESSION,         TagType.UnsignedShort,  1));
            list.Add(new IFD_3_EntryDescriptor(0x0111, STRIP_OFFSET,        TagType.UnsignedLong,   1));
            list.Add(new IFD_3_EntryDescriptor(0x0117, STRIP_BYTE_COUNTS,   TagType.UnsignedLong,   1));
            list.Add(new IFD_3_EntryDescriptor(0xC5D8, "",                  TagType.UnsignedLong,   1));
            list.Add(new IFD_3_EntryDescriptor(0x5E0,  "",                  TagType.UnsignedLong,   1));
            list.Add(new IFD_3_EntryDescriptor(0xC640, CR2_SLICE,           TagType.UnsignedShort,  3));
            
            return list;
        }
    }
}

