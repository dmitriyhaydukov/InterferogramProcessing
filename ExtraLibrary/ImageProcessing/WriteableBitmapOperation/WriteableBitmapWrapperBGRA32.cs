using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExtraLibrary.ImageProcessing
{
    class WriteableBitmapWrapperBGRA32 : WriteableBitmapWrapper {
        //----------------------------------------------------------------------------------------
        //Конструктор
        public WriteableBitmapWrapperBGRA32( WriteableBitmap writeableBitmap ) {
            bool checkFormat =
                writeableBitmap.Format == PixelFormats.Bgra32;
            if ( !checkFormat ) {
                throw new ImageException();
            }
            this.writeableBitmap = writeableBitmap;
        }
        //----------------------------------------------------------------------------------------
        //Получить цвет пикселя
        public override Color GetPixelColor( int x, int y ) {
            byte[] pixelBytes = new byte[ 4 ];
            int stride = this.GetStride();
            Int32Rect rect = new Int32Rect( x, y, 1, 1 );
            int offset = this.GetOffset( x, y );
            this.writeableBitmap.CopyPixels( rect, pixelBytes, stride, 0 );

            byte blue = pixelBytes[ 0 ];
            byte green = pixelBytes[ 1 ];
            byte red = pixelBytes[ 2 ];
            byte alpha = pixelBytes[ 3 ];

            Color color = Color.FromArgb( alpha, red, green, blue );
            return color;
        }
        //----------------------------------------------------------------------------------------
        //Задать цвет пикселя
        public override void SetPixelColor( int x, int y, Color color ) {
            byte red = color.R;
            byte green = color.G;
            byte blue = color.B;
            byte alpha = color.A;

            byte[] pixelBytes = new byte[] { blue, green, red, alpha };
            Int32Rect rect = new Int32Rect( x, y, 1, 1 );
            int stride = this.GetStride();
            this.writeableBitmap.WritePixels( rect, pixelBytes, stride, 0 );
        }
        //----------------------------------------------------------------------------------------
        public override double GetGrayValue( int x, int y ) {
            throw new Exception();
        }
        //----------------------------------------------------------------------------------------
        public override double SetGrayValue( int x, int y, double grayValue ) {
            throw new Exception();
        }
        //----------------------------------------------------------------------------------------
    }
}
