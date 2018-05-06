using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.ImageProcessing
{
    public class BitmapWrapper {
        //-------------------------------------------------------------------------------------------------------
        //Матрица яркости из изображения (диапазон 0..1)
        public static RealMatrix GetBrightnessMatrixFromBitmap( Bitmap bitmap ) {
            int sizeX = bitmap.Width;
            int sizeY = bitmap.Height;
            RealMatrix brightnessMatrix = new RealMatrix( sizeY, sizeX );
            for ( int x = 0; x < sizeX; x++ ) {
                for ( int y = 0; y < sizeY; y++ ) {
                    System.Drawing.Color pixelColor = bitmap.GetPixel( x, y );
                    double brightness = pixelColor.GetBrightness();
                    brightnessMatrix[ y, x ] = brightness;
                }
            }
            return brightnessMatrix;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //Матрица интенсивностей серого из изображения
        public static RealMatrix GetGrayScaleMatrixFromBitmap( Bitmap bitmap ) {
            int sizeX = bitmap.Width;
            int sizeY = bitmap.Height;
            RealMatrix grayScaleMatrix = new RealMatrix( sizeY, sizeX );
            for ( int x = 0; x < sizeX; x++ ) {
                for ( int y = 0; y < sizeY; y++ ) {
                    System.Drawing.Color pixelColor = bitmap.GetPixel( x, y );
                    double grayIntensity = ColorWrapper.GetGrayIntensity( pixelColor );
                    grayScaleMatrix[ y, x ] = grayIntensity;
                }
            }

            return grayScaleMatrix;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
    }
}
