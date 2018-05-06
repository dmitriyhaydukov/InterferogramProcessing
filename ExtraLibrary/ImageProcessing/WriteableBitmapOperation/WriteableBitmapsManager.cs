using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.ImageProcessing
{
    public class WriteableBitmapsManager {
        //------------------------------------------------------------------------------------------------
        //Создание изображений из файлов
        public static WriteableBitmap[] CreateWriteableBitmapsFromFiles( params string[] fileNames ) {
            WriteableBitmap[] writeableBitmaps = new WriteableBitmap[ fileNames.Length ];
            for ( int index = 0; index < fileNames.Length; index++ ) {
                string fileName = fileNames[ index ];
                WriteableBitmap writeableBitmap =
                    WriteableBitmapCreator.CreateWriteableBitmapFromFile( fileName );
                writeableBitmaps[ index ] = writeableBitmap;
            }
            return writeableBitmaps;
        }
        //--------------------------------------------------------------------------------------------------
        //Создание изображений из файлов в директории
        public static WriteableBitmap[] CreateWriteableBitmapsFromFilesInDirectory( string path ) {
            string[] filesNames = Directory.GetFiles( path );
            WriteableBitmap[] writeableBitmaps = 
                WriteableBitmapsManager.CreateWriteableBitmapsFromFiles( filesNames );
            return writeableBitmaps;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //Матрицы интенсивностей серого
        public static RealMatrix[] GetGrayScaleMatricesFromWriteableBitmaps( params WriteableBitmap[] bitmaps ) {
            RealMatrix[] grayScaleMatrices = new RealMatrix[ bitmaps.Length ];
            for ( int index = 0; index < bitmaps.Length; index++ ) {
                WriteableBitmap bitmap = bitmaps[ index ];
                RealMatrix grayScaleMatrix =
                    WriteableBitmapWrapper.GetGrayScaleMatrixFromWriteableBitmap( bitmap );
                grayScaleMatrices[ index ] = grayScaleMatrix;
            }
            return grayScaleMatrices;
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //Создание изображений из матриц интенсивностей серого
        public static WriteableBitmap[] CreateGrayScaleWriteableBitmapsFromMatrices( 
            int dpiX, int dpiY, params RealMatrix[] matrices
        ) {
            WriteableBitmap[] writeableBitmaps = new WriteableBitmap[ matrices.Length ];
            for ( int index = 0; index < matrices.Length; index++ ) {
                RealMatrix matrix = matrices[index];
                WriteableBitmap writeableBitmap =
                    WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix( matrix, dpiX, dpiY );
                writeableBitmaps[ index ] = writeableBitmap;
            }
            return writeableBitmaps;
        }
        //--------------------------------------------------------------------------------------------------
        //Сохранение изображений в файлы
        public static void SaveWrtieableBitmapsToPngFile( WriteableBitmap[] bitmaps, string[] fileNames ) {
            for ( int index = 0; index < 0; index++ ) {
                WriteableBitmap bitmap = bitmaps[ index ];
                WriteableBitmapWrapper bitmapWrapper = WriteableBitmapWrapper.Create( bitmap );
                string fileName = fileNames[ index ];
                bitmapWrapper.SaveToPngFile( fileName );
            }
        }
        //-------------------------------------------------------------------------------------------------
        //Создание изображений из матриц R G B
        public static WriteableBitmap[] CreateWriteableBitmapsFromMatricesRGB(
            RealMatrix[] redMatrices, RealMatrix[] greenMatrices, RealMatrix[] blueMatrices,
            int dpiX, int dpiY
        ) {
            int count = redMatrices.Length;
            WriteableBitmap[] bitmaps = new WriteableBitmap[ count ];
            for ( int index = 0; index < count; index++ ) {
                RealMatrix redMatrix = redMatrices[ index ];
                RealMatrix greenMatrix = greenMatrices[ index ];
                RealMatrix blueMatrix = blueMatrices[ index ];
                WriteableBitmap bitmap =
                    WriteableBitmapCreator.CreateWriteableBitmapFromMatricesRGB
                    ( redMatrix, greenMatrix, blueMatrix, dpiX, dpiY );
                bitmaps[ index ] = bitmap;
            }
            return bitmaps;
        }
        //-------------------------------------------------------------------------------------------------
        //Обрезанные изображения
        public static WriteableBitmap[] GetSubBitmaps( 
            WriteableBitmap[] bitmaps, System.Drawing.Point leftTop, System.Drawing.Point rightBottom
        ) {
            WriteableBitmap[] subBitmaps = new WriteableBitmap[ bitmaps.Length ];
            for ( int index = 0; index < bitmaps.Length; index++ ) {
                WriteableBitmap bitmap = bitmaps[ index ];
                WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( bitmap );
                WriteableBitmap subBitmap = wrapper.GetSubBitmap( leftTop, rightBottom );
                subBitmaps[ index ] = subBitmap;
            }
            return subBitmaps;
        }
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------

    }
}
