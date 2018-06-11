using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class IFD_0_EntryDescriptor
    {
        public const string IMAGE_WIDTH = "imageWidth";
        public const string IMAGE_LENGTH = "imageLength";
        public const string BITS_PER_SAMPLE = "	bitsPerSample";
        public const string COMPRESSION = "compression";
        public const string MAKE = "make";
        public const string MODEL = "model";
        public const string STRIP_OFFSET = "stripOffset";
        public const string ORIENTATION = "orientation";
        public const string STRIP_BYTE_COUNT = "stripByteCount";
        public const string X_RESOLUTION = "xResolution";
        public const string Y_RESOLUTION = "yResolution";
        public const string RESOLUTION_UNIT = "resulutionUnit";
        public const string DATE_TIME = "dateTime";
        public const string EXIF = "EXIF";
        
        public Int32 TagID { get; set; }
        public string Name { get; set; }
        public TagType TagType { get; set; }
        public Int32 Length { get; set; }

        public IFD_0_EntryDescriptor(Int32 tagID, string name, TagType tagType, int length)
        {
            this.TagID = tagID;
            this.Name = name;
            this.TagType = tagType;
            this.Length = length;
        }

        public static List<IFD_0_EntryDescriptor> GetEntryDescriptorList()
        {
            List<IFD_0_EntryDescriptor> list = new List<IFD_0_EntryDescriptor>();

            list.Add(new IFD_0_EntryDescriptor(0x0100, IMAGE_WIDTH,         TagType.UnsignedShort,          1));
            list.Add(new IFD_0_EntryDescriptor(0x0101, IMAGE_LENGTH,        TagType.UnsignedShort,          1));
            list.Add(new IFD_0_EntryDescriptor(0x0102, BITS_PER_SAMPLE,     TagType.UnsignedShort,          3));
            list.Add(new IFD_0_EntryDescriptor(0x0103, COMPRESSION,         TagType.UnsignedShort,          1));
            list.Add(new IFD_0_EntryDescriptor(0x010F, MAKE,                TagType.StringWithEndingZero,   1));
            list.Add(new IFD_0_EntryDescriptor(0x0110, MODEL,               TagType.StringWithEndingZero,   1));
            list.Add(new IFD_0_EntryDescriptor(0x0111, STRIP_OFFSET,        TagType.UnsignedLong,           1));
            list.Add(new IFD_0_EntryDescriptor(0x0112, ORIENTATION,         TagType.UnsignedShort,          1));
            list.Add(new IFD_0_EntryDescriptor(0x0117, STRIP_BYTE_COUNT,    TagType.SignedLong,             1));
            list.Add(new IFD_0_EntryDescriptor(0x011A, X_RESOLUTION,        TagType.SignedRationnal,        1));
            list.Add(new IFD_0_EntryDescriptor(0x011B, Y_RESOLUTION,        TagType.SignedRationnal,        1));
            list.Add(new IFD_0_EntryDescriptor(0x0128, RESOLUTION_UNIT,     TagType.UnsignedShort,          1));
            list.Add(new IFD_0_EntryDescriptor(0x0132, DATE_TIME,           TagType.StringWithEndingZero,   20));
            list.Add(new IFD_0_EntryDescriptor(0x8769, EXIF,                TagType.UnsignedLong,           1));

            return list;
        }
    }
}
