using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.Imaging {
    public class WriteableBitmapConverter {
        //-----------------------------------------------------------------------------------------------
        //Конвертирование в указанный формат
        public static WriteableBitmap ConvertWriteableBitmap(
            WriteableBitmap bitmap,
            PixelFormat destinationPixelFormat
        ) {

            FormatConvertedBitmap formatConvertedBitmap =
                new FormatConvertedBitmap( bitmap, destinationPixelFormat, null, 0 );
            
            
            WriteableBitmap newBitmap = new WriteableBitmap( formatConvertedBitmap );
            return newBitmap;
            
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
    }
}
