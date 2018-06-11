using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class ImageFileDirectoryEntry
    {
        #region Constants

        public const UInt32 TAG_ID_OFFSET = 0;
        public const UInt32 TAG_ID_SIZE_IN_BYTES = 2;

        public const UInt32 TAG_TYPE_OFFSET = 2;
        public const UInt32 TAG_TYPE_SIZE_IN_BYTES = 2;
    
        public const UInt32 NUMBER_OF_VALUE_OFFSET = 4;
        public const UInt32 NUMBER_OF_VALUE_SIZE_IN_BYTES = 4;

        public const UInt32 VALUE_OFFSET = 8;
        public const UInt32 VALUE_SIZE_IN_BYTES = 4;
        
        #endregion Constants

        public UInt32 TagID { get; set; }
        public TagType TagType { get; set; }
        public UInt32 NumberOfValue { get; set; }
        public byte[] BytesValue { get; set; }

        public Int64 GetInt64Value()
        {
            return ConvertHelper.ConvertToInt64(this.BytesValue);
        }

        public UInt64 GetUInt64Value()
        {
            return ConvertHelper.ConvertToUInt64(this.BytesValue);
        }
        
        public Int32 GetInt32Value()
        {
            return ConvertHelper.ConvertToInt32(this.BytesValue);
        }

        public UInt32 GetUInt32Value()
        {
            return ConvertHelper.ConvertToUInt32(this.BytesValue);
        }

        public Int16 GetInt16Value()
        {
            return ConvertHelper.ConvertToInt16(this.BytesValue);
        }

        public UInt16 GetUInt16Value()
        {
            return ConvertHelper.ConvertToUInt16(this.BytesValue);
        }

        public string GetStringValue()
        {
            return ConvertHelper.ConvertToString(this.BytesValue);
        }

    }
}
