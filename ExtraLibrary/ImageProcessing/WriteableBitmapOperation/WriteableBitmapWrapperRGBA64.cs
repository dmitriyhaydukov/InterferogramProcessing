using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExtraLibrary.ImageProcessing
{
    class WriteableBitmapWrapperRGBA64 : WriteableBitmapWrapper {
        //----------------------------------------------------------------------------------------
        //Конструктор
        public WriteableBitmapWrapperRGBA64( WriteableBitmap writeableBitmap ) {
            bool checkFormat =
                writeableBitmap.Format == PixelFormats.Rgba64;
            if ( !checkFormat ) {
                throw new ImageException();
            }
            this.writeableBitmap = writeableBitmap;
        }
        //----------------------------------------------------------------------------------------
        //Получить цвет пикселя
        public override Color GetPixelColor( int x, int y ) {
            ushort[] pixelBytes = new ushort[ 4 ];
            int stride = this.GetStride();
            Int32Rect rect = new Int32Rect( x, y, 1, 1 );
            int offset = this.GetOffset( x, y );
            this.writeableBitmap.CopyPixels( rect, pixelBytes, stride, 0 );

            ushort red = pixelBytes[ 0 ];
            ushort green = pixelBytes[ 1 ];
            ushort blue = pixelBytes[ 2 ];
            ushort alpha = pixelBytes[ 3 ];
            
            Color color = Color.FromArgb
                (
                    Convert.ToByte(alpha),
                    Convert.ToByte(red),
                    Convert.ToByte(green),
                    Convert.ToByte(blue)
                );

            return color;
        }
        //----------------------------------------------------------------------------------------
        //Задать цвет пикселя
        public override void SetPixelColor( int x, int y, Color color ) {
            byte red = color.R;
            byte green = color.G;
            byte blue = color.B;
            byte alpha = color.A;

            ushort[] pixelBytes = new ushort[] { red, green, blue, alpha };
            Int32Rect rect = new Int32Rect( x, y, 1, 1 );
            int stride = this.GetStride();
            this.writeableBitmap.WritePixels( rect, pixelBytes, stride, 0 );
        }
        //----------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------
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
