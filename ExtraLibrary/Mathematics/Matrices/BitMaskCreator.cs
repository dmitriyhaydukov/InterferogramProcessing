using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ExtraLibrary.ImageProcessing;

namespace ExtraLibrary.Mathematics.Matrices {
    
    public class BitMaskCreator {
        //----------------------------------------------------------------------------------------------
        //Создание битовой маски из изображения
        //( изображение должно  иметь только два цвета - черный и белый )
        public static BitMask2D CreateBitMaskFormWriteableBitmap( WriteableBitmap bitmap ) {
            WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( bitmap );
            BitMask2D bitMask = new BitMask2D( bitmap.PixelHeight, bitmap.PixelWidth, false );
            for ( int x = 0; x < wrapper.Image.PixelWidth; x++ ) {
                for ( int y = 0; y < wrapper.Image.PixelHeight; y++ ) {
                    Color color = wrapper.GetPixelColor( x, y );
                    bitMask[ y, x ] = color.Equals( Colors.White );
                }
            }
            return bitMask;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
    }
    
}
