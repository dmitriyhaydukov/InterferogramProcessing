using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExtraLibrary.ImageProcessing
{
    class WriteableBitmapWrapperGray16 : WriteableBitmapWrapper {
        //----------------------------------------------------------------------------------------
        //Конструктор
        public WriteableBitmapWrapperGray16( WriteableBitmap writeableBitmap ) {
            bool checkFormat = writeableBitmap.Format == PixelFormats.Gray16;
            if ( !checkFormat ) {
                throw new ImageException();
            }
            this.writeableBitmap = writeableBitmap;
        }
        //----------------------------------------------------------------------------------------
        //Получить цвет пикселя
        public override Color GetPixelColor( int x, int y ) {
            throw new Exception();
        }
        //----------------------------------------------------------------------------------------
        //Получить значение пикселя
        public override double GetGrayValue( int x, int y ) {

            ushort[] pixelBytes = new ushort[ 1 ];
            int stride = this.GetStride();
            Int32Rect rect = new Int32Rect( x, y, 1, 1 );
            int offset = this.GetOffset( x, y );
            this.writeableBitmap.CopyPixels( rect, pixelBytes, stride, 0 );
            ushort grayValue = pixelBytes[ 0 ];

            return grayValue;
        }
        //----------------------------------------------------------------------------------------
        //Задать цвет пикселя
        public override void SetPixelColor( int x, int y, Color color ) {
            throw new Exception();
        }
        //----------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------
        public override double SetGrayValue( int x, int y, double grayValue ) {
            throw new Exception();
        }
        //----------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------
    }
}
