using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.ImageProcessing
{
    public class BitmapsManager {
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //Создание изображений из файлов
        public static Bitmap[] CreateBitMapsFromFiles( string[] filesNames ) {
            Bitmap[] bitmaps = new Bitmap[ filesNames.Length ];
            for ( int index = 0; index < filesNames.Length; index++ ) {
                string fileName = filesNames[ index ];
                Bitmap bitmap = BitmapCreator.CreateBitMapFromFile( fileName );
                bitmaps[ index ] = bitmap;
            }
            return bitmaps;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //Матрицы яркостей из изображений (диапазон 0..1)
        public static RealMatrix[] GetBrightnessMatricesFromBitmaps( Bitmap[] bitmaps ) {
            RealMatrix[] brightnessMatrices = new RealMatrix[ bitmaps.Length ];
            for ( int index = 0; index < bitmaps.Length; index++ ) {
                Bitmap bitmap = bitmaps[ index ];
                RealMatrix brightnessMatrix = BitmapWrapper.GetBrightnessMatrixFromBitmap( bitmap );
                brightnessMatrices[ index ] = brightnessMatrix;
            }
            return brightnessMatrices;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //Матрицы интенсивностей серого из изображений
        public static RealMatrix[] GetGrayScaleMatricesFromBitmaps( Bitmap[] bitmaps ) {
            RealMatrix[] grayScaleMatrices = new RealMatrix[ bitmaps.Length ];
            for ( int index = 0; index < bitmaps.Length; index++ ) {
                Bitmap bitmap = bitmaps[ index ];
                RealMatrix grayScaleMatrix = BitmapWrapper.GetGrayScaleMatrixFromBitmap( bitmap );
                grayScaleMatrices[ index ] = grayScaleMatrix;
            }
            return grayScaleMatrices;
        }
        //-------------------------------------------------------------------------------------------------------
        
        //-------------------------------------------------------------------------------------------------------
    }
}
