using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.Imaging {
    public class BitmapCreator {
        //-------------------------------------------------------------------------------------------------------
        //Создание изображения из файла
        public static Bitmap CreateBitMapFromFile( string fileName ) {
            Bitmap image = new Bitmap( fileName );
            return image;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //Создание полутонового изображения из матрицы
        public static Bitmap CreateGrayScaleBitmapFromMatrix( RealMatrix matrix ) {
            int width = matrix.ColumnCount;
            int height = matrix.RowCount;
            Bitmap bitmap = new Bitmap( width, height, PixelFormat.Format24bppRgb );
            for ( int x = 0; x < width; x++ ) {
                for ( int y = 0; y < height; y++ ) {
                    int intensity = ( int )matrix[ y, x ];
                    Color color = Color.FromArgb( intensity, intensity, intensity );
                    bitmap.SetPixel( x, y, color );
                }
            }
            return bitmap;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
    }
}
