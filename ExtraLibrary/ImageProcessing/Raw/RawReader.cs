using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Runtime.InteropServices;

using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class RawReader
    {
        #region Members

        private static Dictionary<string, int>[] diiferenceCodeValueMapsArray = DecodingHelper.GetDifferenceCodeValueMap();
        
        #endregion 
        
        public static IntegerMatrix ReadImageFromFile(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);

            List<HeaderItemDescriptor> headerItemList = HeaderItemDescriptor.GetHeaderItemDescriptorList();

            #region Byte Order
            
            //Byte order
            HeaderItemDescriptor headerItemByteOrder =
                headerItemList.FirstOrDefault(item => item.Name == HeaderItemDescriptor.ITEM_BYTE_ORDER);
            byte[] headerItemByteOrderBytes = RawReader.ReadHeaderItem(fileBytes, headerItemByteOrder);

            #endregion
            
            //First IFD
            HeaderItemDescriptor headerItemFirstIFD =
                headerItemList.FirstOrDefault(item => item.Name == HeaderItemDescriptor.ITEM_TIFF_OFFSET);
            byte[] headerItemFirstIFDBytes = RawReader.ReadHeaderItem(fileBytes, headerItemFirstIFD);

            List<IFD_0_EntryDescriptor> firstIFDEntryDescriptorList = IFD_0_EntryDescriptor.GetEntryDescriptorList();

            //Compression
            int compressionEntryIndex = firstIFDEntryDescriptorList.FindIndex(item => item.Name == IFD_0_EntryDescriptor.COMPRESSION);
            UInt32 localCompressionOffset = ImageFileDirectory.GetImageFileDirectoryEntryOffset(Convert.ToUInt32(compressionEntryIndex));
            UInt32 compressionOffset = BitConverter.ToUInt32(headerItemFirstIFDBytes, 0) + localCompressionOffset;
            ImageFileDirectoryEntry compressionEntry = RawReader.ReadImageFileDirectoryEntry(fileBytes, compressionOffset);
            UInt16 compressionValue = compressionEntry.GetUInt16Value();

            //Make
            int makeEntryIndex = firstIFDEntryDescriptorList.FindIndex(item => item.Name == IFD_0_EntryDescriptor.MAKE);
            UInt32 localMakeOffset = ImageFileDirectory.GetImageFileDirectoryEntryOffset(Convert.ToUInt32(makeEntryIndex));
            UInt32 makeOffset = BitConverter.ToUInt32(headerItemFirstIFDBytes, 0) + localMakeOffset;
            ImageFileDirectoryEntry makeEntry = RawReader.ReadImageFileDirectoryEntry(fileBytes, makeOffset);
            UInt32 makeOffseValue = makeEntry.GetUInt32Value();
            byte[] makeBytes = RawReader.ReadBytesUntilEndingZero(fileBytes, makeOffseValue);
            string makeValue = ConvertHelper.ConvertToString(makeBytes);
            
            //Model
            int modelEntryIndex = firstIFDEntryDescriptorList.FindIndex(item => item.Name == IFD_0_EntryDescriptor.MODEL);
            UInt32 localModelOffset = ImageFileDirectory.GetImageFileDirectoryEntryOffset(Convert.ToUInt32(modelEntryIndex));
            UInt32 modelOffset = BitConverter.ToUInt32(headerItemFirstIFDBytes, 0) + localModelOffset;
            ImageFileDirectoryEntry modelEntry = RawReader.ReadImageFileDirectoryEntry(fileBytes, modelOffset);
            UInt32 modelOffsetValue = modelEntry.GetUInt32Value();
            byte[] modelBytes = RawReader.ReadBytesUntilEndingZero(fileBytes, modelOffsetValue);
            string modelValue = ConvertHelper.ConvertToString(modelBytes);
            
            //X Resolution
            int xResolutionEntryIndex = firstIFDEntryDescriptorList.FindIndex(item => item.Name == IFD_0_EntryDescriptor.X_RESOLUTION);
            UInt32 localXResolutionOffset = ImageFileDirectory.GetImageFileDirectoryEntryOffset(Convert.ToUInt32(xResolutionEntryIndex));
            UInt32 xResolutionOffset = BitConverter.ToUInt32(headerItemFirstIFDBytes, 0) + localXResolutionOffset;
            ImageFileDirectoryEntry xResolutionEntry = RawReader.ReadImageFileDirectoryEntry(fileBytes, xResolutionOffset);
            UInt32 xResolutionOffsetValue = xResolutionEntry.GetUInt32Value();
            byte[] xResolutionBytes = RawReader.ReadBytes(fileBytes, xResolutionOffsetValue, 4);
            UInt32 xResolutionValue = ConvertHelper.ConvertToUInt32(xResolutionBytes);
            
            //DateTime
            int dateTimeEntryIndex = firstIFDEntryDescriptorList.FindIndex(item => item.Name == IFD_0_EntryDescriptor.DATE_TIME);
            UInt32 localDateTimeOffset = ImageFileDirectory.GetImageFileDirectoryEntryOffset(Convert.ToUInt32(dateTimeEntryIndex));
            UInt32 dateTimeOffset = BitConverter.ToUInt32(headerItemFirstIFDBytes, 0) + localDateTimeOffset;
            ImageFileDirectoryEntry dateTimeEntry = RawReader.ReadImageFileDirectoryEntry(fileBytes, dateTimeOffset);
            UInt32 dateTimeOffsetValue = dateTimeEntry.GetUInt32Value();
            byte[] dateTimeBytes = RawReader.ReadBytesUntilEndingZero(fileBytes, dateTimeOffsetValue);
            string dateTimeValue = ConvertHelper.ConvertToString(dateTimeBytes);
            DateTime dateTimeStruct = DateTime.ParseExact(dateTimeValue, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture);
                        
            //EXIF bytes
            IFD_0_EntryDescriptor exifEntryDescriptor = 
                firstIFDEntryDescriptorList.Where(item => item.Name == IFD_0_EntryDescriptor.EXIF).FirstOrDefault();
            uint exifOffset = RawReader.GetEntryOffsetByTagID(fileBytes, exifEntryDescriptor.TagID);
            ImageFileDirectoryEntry exifOffsetEntry = RawReader.ReadImageFileDirectoryEntry(fileBytes, exifOffset);
            UInt32 exifOffsetValue = exifOffsetEntry.GetUInt32Value();
            byte[] exifBytes = RawReader.ReadBytes(fileBytes, exifOffsetValue);

            //Makernote bytes
            List<EXIF_EntryDescriptor> exifEntryDescriptorList = EXIF_EntryDescriptor.GetEntryDescriptorList();
            EXIF_EntryDescriptor makerNoteEntryDescriptor = 
                exifEntryDescriptorList.Where(item => item.Name == EXIF_EntryDescriptor.MAKER_NOTE).FirstOrDefault();
            uint makerNoteOffset = RawReader.GetEntryOffsetByTagID(fileBytes,  makerNoteEntryDescriptor.TagID);
            ImageFileDirectoryEntry makerNoteOffsetEntry = RawReader.ReadImageFileDirectoryEntry(fileBytes, makerNoteOffset);
            UInt32 makerNoteOffsetValue = makerNoteOffsetEntry.GetUInt32Value();
            byte[] makerNoteBytes = RawReader.ReadBytes(fileBytes, makerNoteOffsetValue);
            
            //Color Balance
            List<Makernote_EntryDescriptor> makernoteEntryDescriptorList = Makernote_EntryDescriptor.GetEntryDescriptorList();
            Makernote_EntryDescriptor colorBalanceEntryDescriptor =
                makernoteEntryDescriptorList.Where(item => item.Name == Makernote_EntryDescriptor.COLOR_BALANCE).FirstOrDefault();
            uint colorBalanceOffset = RawReader.GetEntryOffsetByTagID(makerNoteBytes, colorBalanceEntryDescriptor.TagID);
            ImageFileDirectoryEntry colorBalanceEntry = RawReader.ReadImageFileDirectoryEntry(makerNoteBytes, colorBalanceOffset);
            UInt32 colorBalanceOffsetValue = colorBalanceEntry.GetUInt32Value();
                       
            
            //Raw Data IFD
            HeaderItemDescriptor headerItemRawIFD =
                headerItemList.FirstOrDefault(item => item.Name == HeaderItemDescriptor.ITEM_RAW_IFD_OFFSET);
            byte[] headerItemRawBytes = RawReader.ReadHeaderItem(fileBytes, headerItemRawIFD);
            List<IFD_3_EntryDescriptor> rawEntryDescriptorList = IFD_3_EntryDescriptor.GetEntryDescriptorList();

            //Slices
            int slicesIndex = rawEntryDescriptorList.FindIndex(item => item.Name == IFD_3_EntryDescriptor.CR2_SLICE);
            UInt32 slicesOffset = BitConverter.ToUInt32(headerItemRawBytes, 0) + ImageFileDirectory.GetImageFileDirectoryEntryOffset(Convert.ToUInt32(slicesIndex));
            ImageFileDirectoryEntry slicesEntry = RawReader.ReadImageFileDirectoryEntry(fileBytes, slicesOffset);
            UInt16[] slices = RawReader.ReadUInt16Array(fileBytes, slicesEntry.GetUInt32Value(), slicesEntry.NumberOfValue);

            //Headers and compressed raw bytes
            int rawOffsetEntryIndex = rawEntryDescriptorList.FindIndex(item => item.Name == IFD_3_EntryDescriptor.STRIP_OFFSET);
            UInt32 localStripOffset = ImageFileDirectory.GetImageFileDirectoryEntryOffset(Convert.ToUInt32(rawOffsetEntryIndex));
            UInt32 stripOffset = BitConverter.ToUInt32(headerItemRawBytes, 0) + localStripOffset;
            ImageFileDirectoryEntry stripOffsetEntry = RawReader.ReadImageFileDirectoryEntry(fileBytes, stripOffset);
            UInt32 stripOffsetValue = stripOffsetEntry.GetUInt32Value();
            byte[] rawBytes = RawReader.ReadBytes(fileBytes, stripOffsetValue);

            DhtHeader dhtHeader = RawReader.ReadDhtHeader(rawBytes);
            Sof3Header sof3Header = RawReader.ReadSof3Header(rawBytes);
            SosHeader sosHeader = RawReader.ReadSosHeader(rawBytes);

            Dictionary<string, byte> huffmanCodeValueDictionaryOne = dhtHeader.HuffmanTables[0].GetHuffmanCodeValueMap();
            Dictionary<string, byte> huffmanCodeValueDictionaryTwo = dhtHeader.HuffmanTables[1].GetHuffmanCodeValueMap();

            //SCAN DATA
            UInt32 scanDataOffset = sosHeader.Offset + sosHeader.HeaderLength + 2;
            byte[] scanDataBytes = RawReader.ReadBytes(rawBytes, scanDataOffset);

            return RawReader.ReadScanData(scanDataBytes, dhtHeader, sof3Header, sosHeader, slices);
        }
        

        public static byte[] ReadHeaderItem(byte[] bytes, HeaderItemDescriptor descriptor)
        {
            UInt32 offset = descriptor.Offset;
            UInt32 length = descriptor.Length;
            Type type = descriptor.Type;

            UInt32 sizeInBytes = GetSizeInBytes(type, length);
            byte[] resBytes = ReadBytes(bytes, offset, sizeInBytes);

            return resBytes;
        }
                
        public static ImageFileDirectoryEntry ReadImageFileDirectoryEntry(byte[] bytes, UInt32 offset)
        {
            UInt32 offsetTagID = ImageFileDirectoryEntry.TAG_ID_OFFSET + offset;
            UInt32 lengthTagID = ImageFileDirectoryEntry.TAG_ID_SIZE_IN_BYTES;
            byte[] bytesTagID = ReadBytes(bytes, offsetTagID, lengthTagID);

            UInt32 offsetTagType = ImageFileDirectoryEntry.TAG_TYPE_OFFSET + offset;
            UInt32 lengthTagType = ImageFileDirectoryEntry.TAG_TYPE_SIZE_IN_BYTES;
            byte[] bytesTagType = ReadBytes(bytes, offsetTagType, lengthTagType);

            UInt32 offsetNumberOfValue = ImageFileDirectoryEntry.NUMBER_OF_VALUE_OFFSET + offset;
            UInt32 lengthNumberOfValue = ImageFileDirectoryEntry.NUMBER_OF_VALUE_SIZE_IN_BYTES;
            byte[] bytesNumberOfValue = ReadBytes(bytes, offsetNumberOfValue, lengthNumberOfValue);

            UInt32 offsetValue = ImageFileDirectoryEntry.VALUE_OFFSET + offset;
            UInt32 lengthValue = ImageFileDirectoryEntry.VALUE_SIZE_IN_BYTES;
            byte[] bytesValue = ReadBytes(bytes, offsetValue, lengthValue);
            
            ImageFileDirectoryEntry entry = new ImageFileDirectoryEntry()
            {
                TagID = ConvertHelper.ConvertToUInt16(bytesTagID),
                TagType = (TagType)ConvertHelper.ConvertToInt16(bytesTagType),
                NumberOfValue = ConvertHelper.ConvertToUInt32(bytesNumberOfValue),
                BytesValue = bytesValue
            };

            return entry;
        }

        private static UInt32 GetSizeInBytes(Type type, UInt32 length)
        {
            return Convert.ToUInt32(length * Marshal.SizeOf(type));
        }
        
        public static byte ReadByte(byte[] bytes, UInt32 offset)
        {
            return bytes[offset];
        }
        
        public static byte[] ReadBytes(byte[] bytes, UInt32 offset)
        {
            byte[] resBytes = new byte[bytes.Length - offset];
            UInt32 start = offset;
            UInt32 end = Convert.ToUInt32(bytes.Length);
            for (UInt32 k = start, i = 0; k < end; k++, i++)
            {
                resBytes[i] = bytes[k];
            }

            return resBytes;
        }
        
        public static byte[] ReadBytesUntilEndingZero(byte[] bytes, UInt32 offset)
        {
            List<byte> bytesList = new List<byte>();
            UInt32 index = offset;

            while (bytes[index] != 0)
            {
                bytesList.Add(bytes[index]);
                index++;
            }

            return bytesList.ToArray();
        }
        
        public static byte[] ReadBytes(byte[] bytes, UInt32 offset, UInt32 length)
        {
            byte[] resBytes = new byte[length];
            UInt32 start = offset;
            UInt32 end = offset + length;
            for (UInt32 k = start, i = 0; k < end; k++, i++)
            {
                resBytes[i] = bytes[k];
            }

            return resBytes;
        }
        
                
        public static UInt16[] ReadUInt16Array(byte[] bytes, UInt32 offset, UInt32 length)
        {
            UInt16[] resArray = new UInt16[length];
            UInt32 itemSize = sizeof(UInt16);

            for (UInt32 index = 0; index < length; index++)
            {
                byte[] bytesValue = RawReader.ReadBytes(bytes, offset + index * itemSize, itemSize);
                UInt16 value = ConvertHelper.ConvertToUInt16(bytesValue);
                resArray[index] = value;
            }
            
            return resArray;
        }

        public static Int32 FindByteValueIndex(byte[] bytes, byte value)
        {
            Int32 resIndex = -1;

            for (int index = 0; index < bytes.Length; index++)
            {
                if (bytes[index] == value)
                {
                    resIndex = index;
                    break;
                }
            }

            return resIndex;
        }

        public static UInt32 FindByteValueIndex(byte[] bytes, byte value, byte nextValue)
        {
            Int32 resIndex = -1;
                        
            for (int index = 0; index < bytes.Length; index++)
            {
                if (bytes[index] == value && bytes[index + 1] == nextValue)
                {
                    resIndex = index;
                    return Convert.ToUInt32(resIndex);
                }
            }

            return Convert.ToUInt32(resIndex);
        }
        
        private static uint GetEntryOffsetByTagID(byte[] bytes, int entryTagID)
        {
            byte[] tagIdBytes = BitConverter.GetBytes(entryTagID);
            uint offset = RawReader.FindByteValueIndex(bytes, tagIdBytes[0], tagIdBytes[1]);
            return offset;
        }

        private static SosHeaderComponentInfo ReadSosHeaderComponentInfo(byte[] bytes, UInt32 offset)
        {
            byte[] componentBytes = RawReader.ReadBytes(bytes, offset, 2);
            HalvedByte halvedByte = new HalvedByte(componentBytes[1]);
            return new SosHeaderComponentInfo()
            {
                ComponentNumber = componentBytes[0],
                DCtable = halvedByte.High,
                ACtable = halvedByte.Low
            };
        }

        private static Sof3HeaderComponentInfo ReadSof3HeaderComponentInfo(byte[] bytes, UInt32 offset)
        {
            byte[] componentBytes = RawReader.ReadBytes(bytes, offset, 3);
            HalvedByte halvedByte = new HalvedByte(componentBytes[1]);
            return new Sof3HeaderComponentInfo()
            {
                ComponentNumber = componentBytes[0],
                HorizontalSamplingFactor = halvedByte.High,
                VerticalSamplingFactor = halvedByte.Low,
                QuantizationTable = componentBytes[2]
            };
        }
         
        public static SosHeader ReadSosHeader(byte[] bytes)
        {
            //SOS HEADER

            UInt32 sosHeaderOffset = RawReader.FindByteValueIndex(bytes, 0xFF, 0xDA);
            byte[] sosHeaderBytes = RawReader.ReadBytes(bytes, sosHeaderOffset);

            UInt32 sosHeaderLengthOffset = 2;
            byte[] sosHeaderLengthBytes = RawReader.ReadBytes(sosHeaderBytes, sosHeaderLengthOffset, 2);
            UInt16 sosHeaderLength = ConvertHelper.ConvertToUInt16(sosHeaderLengthBytes, true);

            UInt32 sosHeaderComponentsNumberOffset = 4;
            byte sosHeaderComponentsNumber = RawReader.ReadByte(sosHeaderBytes, sosHeaderComponentsNumberOffset);

            SosHeaderComponentInfo[] components = new SosHeaderComponentInfo[sosHeaderComponentsNumber];
            UInt32 sosHeaderComponentsOffset = 5;
            for (byte k = 0; k < sosHeaderComponentsNumber; k++)
            {
                SosHeaderComponentInfo componentInfo = RawReader.ReadSosHeaderComponentInfo(sosHeaderBytes, sosHeaderComponentsOffset);
                components[k] = componentInfo;
                sosHeaderComponentsOffset += 2;
            }

            return new SosHeader()
            {
                Offset = sosHeaderOffset,

                HeaderLength = sosHeaderLength,
                Components = components
            };
        }


        public static Sof3Header ReadSof3Header(byte[] bytes)
        {
            //SOF3 HEADER

            UInt32 sof3HeaderOffset = RawReader.FindByteValueIndex(bytes, 0xFF, 0xC3);
            byte[] sof3HeaderBytes = RawReader.ReadBytes(bytes, sof3HeaderOffset);

            UInt32 sof3HeaderLengthOffset = 2;
            byte[] sof3HeaderLengthBytes = RawReader.ReadBytes(sof3HeaderBytes, sof3HeaderLengthOffset, 2);
            UInt16 sof3HeaderLength = ConvertHelper.ConvertToUInt16(sof3HeaderLengthBytes, true);

            UInt32 sof3HeaderSamplePrecisionOffset = 4;
            byte sof3HeaderSamplePrecision = RawReader.ReadByte(sof3HeaderBytes, sof3HeaderSamplePrecisionOffset);

            UInt32 sof3HeaderLinesNumberOffset = 5;
            byte[] sof3HeaderLinesNumberBytes = RawReader.ReadBytes(sof3HeaderBytes, sof3HeaderLinesNumberOffset, 2);
            UInt16 sof3HeaderLinesNumber = ConvertHelper.ConvertToUInt16(sof3HeaderLinesNumberBytes, true);

            UInt32 sof3HeaderSamplesPerLineNumberOffset = 7;
            byte[] sof3HeaderSamplesPerLineNumberBytes = RawReader.ReadBytes(sof3HeaderBytes, sof3HeaderSamplesPerLineNumberOffset, 2);
            UInt16 sof3HeaderSamplesPerLineNumber = ConvertHelper.ConvertToUInt16(sof3HeaderSamplesPerLineNumberBytes, true);

            UInt32 sof3HeaderImageComponentsPerFrameNumberOffset = 9;
            byte sof3HeaderImageComponentsPerFrameNumber = RawReader.ReadByte(sof3HeaderBytes, sof3HeaderImageComponentsPerFrameNumberOffset);

            Sof3HeaderComponentInfo[] components = new Sof3HeaderComponentInfo[sof3HeaderImageComponentsPerFrameNumber];
            UInt32 sof3HeaderComponentsOffset = 10;
            for (byte k = 0; k < sof3HeaderImageComponentsPerFrameNumber; k++)
            {
                Sof3HeaderComponentInfo componentInfo = RawReader.ReadSof3HeaderComponentInfo(sof3HeaderBytes, sof3HeaderComponentsOffset);
                components[k] = componentInfo;
                sof3HeaderComponentsOffset += 3;
            }

            return new Sof3Header()
            {
                Offset = sof3HeaderOffset,

                HeaderLength = sof3HeaderLength,
                SamplePrecision = sof3HeaderSamplePrecision,
                LinesNumber = sof3HeaderLinesNumber,
                SamplesPerLineNumber = sof3HeaderSamplesPerLineNumber,
                Components = components
            };
        }
        
        public static DhtHeader ReadDhtHeader(byte[] bytes)
        {
            UInt32 dhtHeaderOffset = RawReader.FindByteValueIndex(bytes, 0xFF, 0xC4);
            byte[] dhtHeaderBytes = RawReader.ReadBytes(bytes, dhtHeaderOffset);

            UInt32 dhtHeaderLengthOffset = 2;
            byte[] dhtHeaderLengthBytes = RawReader.ReadBytes(dhtHeaderBytes, dhtHeaderLengthOffset, 2);
            UInt16 dhtHeaderLength = ConvertHelper.ConvertToUInt16(dhtHeaderLengthBytes, true);
            UInt32 valueHuffmanCodesOffset;
            UInt32 lengthNumberHuffmanCodesSum = 0;

            //Huffman table 1
            UInt32 dhtHeaderTableIndexOffset = 4;
            byte dhtHeaderTableIndex;
            UInt32 tableOffset = dhtHeaderTableIndexOffset + 1;
                        
            HuffmanTable tableOne = new HuffmanTable();
            dhtHeaderTableIndex = RawReader.ReadByte(dhtHeaderBytes, dhtHeaderTableIndexOffset);
            tableOne.TableClass = (new HalvedByte(dhtHeaderTableIndex)).High;
            tableOne.TableIndex = (new HalvedByte(dhtHeaderTableIndex)).Low;
            
            byte[] lengthNumberHuffmanCodesBytes = 
                RawReader.ReadBytes(dhtHeaderBytes, tableOffset, DhtHeader.LENGTH_NUMBER_HUFFMAN_CODES_COUNT);
            lengthNumberHuffmanCodesSum = Convert.ToUInt32(lengthNumberHuffmanCodesBytes.Sum(x => x));
            valueHuffmanCodesOffset = tableOffset + DhtHeader.LENGTH_NUMBER_HUFFMAN_CODES_COUNT;
            byte[] valueHuffmanCodesBytes = RawReader.ReadBytes(dhtHeaderBytes, valueHuffmanCodesOffset, lengthNumberHuffmanCodesSum);

            tableOne.LengthHuffmanCodes = lengthNumberHuffmanCodesBytes;
            tableOne.ValueHuffmanCodes = valueHuffmanCodesBytes;


            //Huffman table 2
            dhtHeaderTableIndexOffset = valueHuffmanCodesOffset + lengthNumberHuffmanCodesSum;
            tableOffset = dhtHeaderTableIndexOffset + 1;
            
            HuffmanTable tableTwo = new HuffmanTable();
            dhtHeaderTableIndex = RawReader.ReadByte(dhtHeaderBytes, dhtHeaderTableIndexOffset);
            tableTwo.TableClass = (new HalvedByte(dhtHeaderTableIndex)).High;
            tableTwo.TableIndex = (new HalvedByte(dhtHeaderTableIndex)).Low;
            
            lengthNumberHuffmanCodesBytes =
                RawReader.ReadBytes(dhtHeaderBytes, tableOffset, DhtHeader.LENGTH_NUMBER_HUFFMAN_CODES_COUNT);
            lengthNumberHuffmanCodesSum = Convert.ToUInt32(lengthNumberHuffmanCodesBytes.Sum(x => x));
            valueHuffmanCodesOffset = tableOffset + DhtHeader.LENGTH_NUMBER_HUFFMAN_CODES_COUNT;
            valueHuffmanCodesBytes = RawReader.ReadBytes(dhtHeaderBytes, valueHuffmanCodesOffset, lengthNumberHuffmanCodesSum);

            tableTwo.LengthHuffmanCodes = lengthNumberHuffmanCodesBytes;
            tableTwo.ValueHuffmanCodes = valueHuffmanCodesBytes;
                        
            return new DhtHeader()
            {
                Offset = dhtHeaderOffset,

                HeaderLength = dhtHeaderLength,
                HuffmanTables = new HuffmanTable[] { tableOne, tableTwo }
            };
        }

        public static IntegerMatrix ReadScanData(byte[] scanDataBytes, DhtHeader dhtHeader, Sof3Header sof3Header, SosHeader sosHeader, UInt16[] slices)
        {
            
            int columnsCount = GetImageWidth(slices);
            int rowsCount = sof3Header.LinesNumber;
            int rowIndex = 0, columnIndex = 0;
            IntegerMatrix resMatrix = new IntegerMatrix(rowsCount, columnsCount);

            ushort normalSliceWidth = slices[1];

            List<int> decompressionValueList = new List<int>();
            Dictionary<string, byte>[] huffmanCodeValueMapsArray = RawReader.GetHuffmanCodeValueMapsArray(dhtHeader);

            BitArray scanDataBitArray = RawReader.GetScanDataBitArray(scanDataBytes);
                        
            int index = 0, huffmanCodeIndex = 0;
            BitArray huffmanCodeBitArray = new BitArray(1);
            string huffmanCodeString = null;
            byte huffmanValue = 0;
                        
            int componentIndex = 0;
            int componentValueIndex = 0, componentValue = 0;
            int[] previousComponentValues = RawReader.GetInitialComponentValues(sof3Header);

            int lastIndex = GetLastDecompressionValueIndex(sof3Header, slices);
            int lastSliceFirstDecompressionValueIndex = GetLastSliceFirstDecompressionValueIndex(sof3Header, slices);
            int sliceSize = slices[1] * sof3Header.LinesNumber;

            Dictionary<string, byte> huffmanCodeValueMap = huffmanCodeValueMapsArray[GetHuffmanTableIndex(rowIndex, columnIndex, sosHeader)];

            while (componentValueIndex <= lastIndex && index < scanDataBitArray.Length)
            {
                huffmanCodeBitArray[huffmanCodeIndex] = scanDataBitArray[index];
                huffmanCodeString = huffmanCodeBitArray.ToBitString();
                
                if (huffmanCodeValueMap.TryGetValue(huffmanCodeString, out huffmanValue))
                {
                    int differenceCodeValue = 0;
                    
                    index++;
                    if (huffmanValue > 0)
                    {
                        //Read difference code
                        int finalIndex = index + huffmanValue - 1;
                        differenceCodeValue = GetDifferenceCodeValue(scanDataBitArray, index, finalIndex, huffmanValue);
                        index = finalIndex + 1;
                    }
                    else
                    {
                        differenceCodeValue = 0;
                    }
                                        
                    componentValue = previousComponentValues[componentIndex] + differenceCodeValue;
                    previousComponentValues[componentIndex] = componentValue;

                    decompressionValueList.Add(componentValue);
                    resMatrix[rowIndex, columnIndex] = componentValue;
                    componentValueIndex++;
                                       
                    huffmanCodeBitArray = new BitArray(1);
                    huffmanCodeIndex = 0;
                                        
                    rowIndex = GetNextRowIndex(componentValueIndex, lastSliceFirstDecompressionValueIndex, sliceSize, slices);
                    columnIndex = GetNextColumnIndex(componentValueIndex, lastSliceFirstDecompressionValueIndex, sliceSize, slices);

                    huffmanCodeValueMap = huffmanCodeValueMapsArray[GetHuffmanTableIndex(rowIndex, columnIndex, sosHeader)];
                                                                                              
                    bool initComponentValuesRequired = 
                        ( columnIndex == 0 ) ||
                        ( (columnIndex > 0) && (columnIndex % normalSliceWidth == 0) );

                    if (initComponentValuesRequired)
                    {
                        previousComponentValues = GetInitialComponentValues(sof3Header);
                    }
                    
                    componentIndex = GetComponentIndex(rowIndex, columnIndex);
                
                }
                else
                {
                    //Read huffman code
                    index++;
                    huffmanCodeIndex++;
                    huffmanCodeBitArray.Length++;
                }
            }
                        
            return resMatrix;
        }
        
        private static int GetDifferenceCodeValue(BitArray scanDataBitArray, int startIndex, int endIndex, byte huffmanValue)
        {
            BitArray differenceCodeBitArray = new BitArray(huffmanValue);
            for (int j = 0, index = startIndex; index <= endIndex; j++, index++)
            {
                differenceCodeBitArray[j] = scanDataBitArray[index];
            }
            
            string differenceCodeString = differenceCodeBitArray.ToBitString();

            int differenceCodeValue = diiferenceCodeValueMapsArray[huffmanValue][differenceCodeString];
            return differenceCodeValue;
        }
        
        private static BitArray GetScanDataBitArray(byte[] scanDataBytes)
        {
            byte[] dataBytes = DecodingHelper.GetCorrectedScanDataBytes(scanDataBytes);
            BitArray scanDataBitArray = new BitArray(dataBytes);
            return scanDataBitArray;
        }

        private static Dictionary<string, byte>[] GetHuffmanCodeValueMapsArray(DhtHeader dhtHeader)
        {
            int count = dhtHeader.HuffmanTables.Length;
            Dictionary<string, byte>[] huffmanCodeValueMapsArray = new Dictionary<string, byte>[count];
            int tableIndex = 0;
            for (int i = 0; i < count; i++)
            {
                huffmanCodeValueMapsArray[i] = dhtHeader.HuffmanTables[tableIndex].GetHuffmanCodeValueMap();
                tableIndex++;
            }
            return huffmanCodeValueMapsArray;
        }
                
        private static int[] GetInitialComponentValues(Sof3Header sof3Header)
        {
            int componentsCount = 4; //RGGB
            
            //int defaultComponentValue = 0;
            int defaultComponentValue = GetDefaultComponentValue(sof3Header);
            
            int[] componentValues = new int[componentsCount];
            for (int i = 0; i < componentValues.Length; i++)
            {
                componentValues[i] = defaultComponentValue;
            }
            return componentValues;
        }

        private static int GetDefaultComponentValue(Sof3Header sof3Header)
        {
            int defaultComponentValue = Convert.ToInt32(Math.Pow(2, sof3Header.SamplePrecision - 1));
            return defaultComponentValue;
        }

        private static int GetHuffmanTableIndex(int rowIndex, int columnIndex, SosHeader sosHeader)
        {
                       
            int rowMod = rowIndex % 2;
            int columnMod = columnIndex % 2;

            if (sosHeader.Components.Length == 2)
            {
                if (rowMod == 0 && columnMod == 0) //Red
                {
                    return sosHeader.Components[0].DCtable;
                }
                if (rowMod == 1 && columnMod == 0) //Green
                {
                    return sosHeader.Components[0].DCtable;
                }
                if (rowMod == 0 && columnMod == 1) //Green
                {
                    return sosHeader.Components[1].DCtable;
                }
                if (rowMod == 1 && columnMod == 1) //Blue
                {
                    return sosHeader.Components[1].DCtable;
                }
            }

            if (sosHeader.Components.Length == 4)
            {
                if (rowMod == 0 && columnMod == 0) //Red
                {
                    return sosHeader.Components[0].DCtable;
                }
                if (rowMod == 1 && columnMod == 0) //Green
                {
                    return sosHeader.Components[2].DCtable;
                }
                if (rowMod == 0 && columnMod == 1) //Green
                {
                    return sosHeader.Components[1].DCtable;
                }
                if (rowMod == 1 && columnMod == 1) //Blue
                {
                    return sosHeader.Components[3].DCtable;
                }
            }

            return 0;
        }

        private static int GetImageWidth(UInt16[] slices)
        {
            int width = slices[0] * slices[1] + slices[2];
            return width;
        }

        private static int GetImageHeight(Sof3Header sof3Header)
        {
            return sof3Header.LinesNumber;
        }
        
        private static int GetLastDecompressionValueIndex(Sof3Header sof3Header, UInt16[] slices)
        {
            int sliceHeight = sof3Header.LinesNumber;
            int sliceWidth = slices[1];
            int count = slices[0];
            int lastSliceWiddth = slices[2];

            int index = (sliceWidth * sliceHeight) * count + lastSliceWiddth * sliceHeight - 1;
            return index;
        }
        
        private static int GetLastSliceFirstDecompressionValueIndex(Sof3Header sof3Header, UInt16[] slices)
        {
            int sliceHeight = sof3Header.LinesNumber;
            int sliceWidth = slices[1];
            int count = slices[0];

            int index = (sliceWidth * sliceHeight) * count;
            return index;
        }
        
        private static int GetNextRowIndex(int decompressionValueIndex, int lastSliceFirstDecompressionValueIndex, int sliceSize, UInt16[] slices)
        {
            int rowIndex = 0;
            if (decompressionValueIndex < lastSliceFirstDecompressionValueIndex)
            {
                int sliceIndex = decompressionValueIndex / sliceSize;
                int localIndex = decompressionValueIndex % sliceSize;
                rowIndex = localIndex / slices[1];
            }
            else
            {
                int prevSlicesCount = slices[0];
                int localIndex = decompressionValueIndex - prevSlicesCount * sliceSize;
                rowIndex = localIndex / slices[2];
            }
            
            return rowIndex;
        }
        
        private static int GetNextColumnIndex(int decompressionValueIndex, int lastSliceFirstDecompressionValueIndex, int sliceSize, UInt16[] slices)
        {
            int columnIndex = 0;
            if (decompressionValueIndex < lastSliceFirstDecompressionValueIndex)
            {
                int sliceIndex = decompressionValueIndex / sliceSize;
                int localIndex = decompressionValueIndex % sliceSize;
                columnIndex = (sliceIndex * slices[1]) + (localIndex % slices[1]);
            }
            else
            {
                int prevSlicesCount = slices[0];
                int localIndex = decompressionValueIndex - prevSlicesCount * sliceSize;
                columnIndex = (prevSlicesCount * slices[1]) + (localIndex % slices[2]);
            }
                        
            return columnIndex;

        }


        private static int GetComponentIndex(int rowIndex, int columnIndex, int quadrantIndex)
        {
            int rowMod = rowIndex % 2;
            int columnMod = columnIndex % 2;

            if (columnMod == 0)
            {
                if (quadrantIndex == 0 || quadrantIndex == 2) // Red
                {
                    return 0;
                }
                else if (quadrantIndex == 1 || quadrantIndex == 3) // Green2
                {
                    return 2;
                }
                
            }
            if (columnMod == 1) 
            {
                if (quadrantIndex == 0 || quadrantIndex == 2) // Green1
                {
                    return 1;
                }
                else if (quadrantIndex == 1 || quadrantIndex == 3) //Blue
                {
                    return 3;
                }
            }
            
            return 0;
        }
        
        private static int GetComponentIndex(int rowIndex, int columnIndex)
        {
            int rowMod = rowIndex % 2;
            int columnMod = columnIndex % 2;

            if (rowMod == 0 && columnMod == 0) //Red
            {
                return 0;
            }
            if (rowMod == 0 && columnMod == 1) //Green
            {
                return 1;
            }
            if (rowMod == 1 && columnMod == 0) // Green
            {
                return 2;
            }
            if (rowMod == 1 && columnMod == 1) //Blue
            {
                return 3;
            }

            return 0;
        }
        
        private static int GetNextComponentIndex(int componentIndex)
        {
            int resIndex = 0;
            if (componentIndex == 3)
            {
                resIndex = 0;
            }
            else
            {
                resIndex = componentIndex + 1;
            }

            return resIndex;
        }

        private static void CheckMatrix(double[,] matrix, int rowsCount, int columnsCount)
        {
            for (int row = 0; row < rowsCount; row++)
            {
                for (int col = 0; col < columnsCount; col++)
                {
                    double value = matrix[row, col];
                    if (value < 0)
                    {
                        int component = RawReader.GetComponentIndex(row, col);
                        Console.WriteLine(component);
                    }
                }
            }
        }
    }
}