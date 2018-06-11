using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class ImageFileDirectory
    {
        #region Constants

        public const UInt32 ENTRIES_COUNT_OFFSET = 0x00;
        public const UInt32 ENTRIES_COUNT_SIZE_IN_BYTES = 2;
        
        public const UInt32 FIRST_ENTRY_OFFSET = 2;
        public const UInt32 ENTRY_SIZE_IN_BYTES = 12;

        public const UInt32 NEXT_IFD_OFFSET_SIZE_IN_BYTES = 4;

        #endregion Constants

        public UInt32 EntriesCount { get; set; }

        public static UInt32 GetImageFileDirectoryEntryOffset(UInt32 entryNumber)
        {
            return FIRST_ENTRY_OFFSET + ENTRY_SIZE_IN_BYTES * entryNumber;
        }

        public UInt32 GetNextImageFileDirectoryOffset()
        {
            return FIRST_ENTRY_OFFSET + ENTRY_SIZE_IN_BYTES * EntriesCount;
        }
    }
}
