using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class Makernote_EntryDescriptor
    {
        public const string COLOR_BALANCE = "colorBalance";

        public Int32 TagID { get; set; }
        public string Name { get; set; }
        public TagType TagType { get; set; }
        public Int32 Length { get; set; }

        public Makernote_EntryDescriptor(Int32 tagID, string name, TagType tagType, int length)
        {
            this.TagID = tagID;
            this.Name = name;
            this.TagType = tagType;
            this.Length = length;
        }

        public static List<Makernote_EntryDescriptor> GetEntryDescriptorList()
        {
            List<Makernote_EntryDescriptor> list = new List<Makernote_EntryDescriptor>();

            list.Add(new Makernote_EntryDescriptor(0x4001, COLOR_BALANCE, TagType.UnsignedShort, -1));

            return list;
        }
    }
}
