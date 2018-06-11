using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class EXIF_EntryDescriptor
    {
        public const string MAKER_NOTE = "Makernote";
        
        public Int32 TagID { get; set; }
        public string Name { get; set; }
        public TagType TagType { get; set; }
        public Int32 Length { get; set; }

        public EXIF_EntryDescriptor(Int32 tagID, string name, TagType tagType, int length)
        {
            this.TagID = tagID;
            this.Name = name;
            this.TagType = tagType;
            this.Length = length;
        }

        public static List<EXIF_EntryDescriptor> GetEntryDescriptorList()
        {
            List<EXIF_EntryDescriptor> list = new List<EXIF_EntryDescriptor>();

            list.Add(new EXIF_EntryDescriptor(0x927c, MAKER_NOTE, TagType.UnsignedLong, 1));
            
            return list;
        }
    }
}
