using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExtraLibrary.ImageProcessing
{
    public class WriteableBitmapWrapperIndexed8 : WriteableBitmapWrapper {
        //-----------------------------------------------------------------------------------------------------
        public WriteableBitmapWrapperIndexed8( WriteableBitmap writeableBitmap ) {
            bool checkFormat =
                writeableBitmap.Format == PixelFormats.Indexed8;
            if ( !checkFormat ) {
                throw new ImageException();
            }
            this.writeableBitmap = writeableBitmap;
        }
        //-----------------------------------------------------------------------------------------------------
        //Получить цвет пикселя
        public override Color GetPixelColor( int x, int y ) {
            throw new ImageException();
            /*
            byte[] pixelBytes = new byte[ 1 ];
            int stride = this.GetStride();
            Int32Rect rect = new Int32Rect( x, y, 1, 1 );
            this.writeableBitmap.CopyPixels( rect, pixelBytes, stride, 0 );

            byte index = pixelBytes[ 0 ];

            Color color = this.Image.Palette.Colors[ index ];
            return color;
            */
            //return Colors.Black;
        }
        //-----------------------------------------------------------------------------------------------------
        //Задать цвет пикселя
        public override void SetPixelColor( int x, int y, Color color ) {
            
        }
        //-----------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------
        public override double GetGrayValue( int x, int y ) {
            throw new Exception();
        }
        //----------------------------------------------------------------------------------------
        public override double SetGrayValue( int x, int y, double grayValue ) {
            throw new Exception();
        }
        //----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------
    }
}
