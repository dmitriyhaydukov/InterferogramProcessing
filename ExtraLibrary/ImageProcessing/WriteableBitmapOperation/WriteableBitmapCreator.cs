using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

using ExtraLibrary.ImageProcessing.Raw;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Sets;
using ExtraLibrary.Mathematics.Transformation;
using ExtraLibrary.OS;

using ImageMagick;
using EDSDKLib;


namespace ExtraLibrary.ImageProcessing
{
    public class WriteableBitmapCreator {
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        /*
        //Создание изображения из файла
        public static WriteableBitmap CreateWriteableBitmapFromFile( string fileName, double dpiX, double dpiY ) {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap( fileName );

            //System.Windows.Media.PixelFormat targetPixelFormat = PixelFormats.Rgba64;
            System.Windows.Media.PixelFormat targetPixelFormat = PixelFormats.Pbgra32;

            WriteableBitmap writeableBitmap = 
                new WriteableBitmap( bitmap.Width, bitmap.Height, dpiX, dpiY, targetPixelFormat, null );

            //WriteableBitmap writeableBitmap =
            //    new WriteableBitmap( bitmap.Width, bitmap.Height, dpiX, dpiY, PixelFormats.Rgba64, null );

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle( 0, 0, bitmap.Width, bitmap.Height );
            System.Drawing.Imaging.ImageLockMode lockMode = System.Drawing.Imaging.ImageLockMode.ReadOnly;
            
            System.Drawing.Imaging.PixelFormat pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            //System.Drawing.Imaging.PixelFormat pixelFormat = System.Drawing.Imaging.PixelFormat.Format64bppArgb;
            //System.Drawing.Imaging.PixelFormat pixelFormat = System.Drawing.Imaging.PixelFormat.Format16bppGrayScale;

            System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits( rect, lockMode, pixelFormat );
            Int32Rect int32Rect = new System.Windows.Int32Rect( 0, 0, bitmapData.Width, bitmapData.Height );
            
            writeableBitmap.WritePixels
                ( int32Rect, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride );
            bitmap.UnlockBits( bitmapData );
            bitmap.Dispose();

            return writeableBitmap;
        }
        */
        //------------------------------------------------------------------------------------------------
        public static ExtraImageInfo CreateWriteableBitmapFromFile( string fileName, double dpiX, double dpiY ) {
        //public static WriteableBitmap CreateWriteableBitmapFromFile( string fileName, double dpiX, double dpiY ) {

            /*
            if ( fileName.ToUpper().EndsWith( "CR2" ) ) {

                MagickImage magicImage = new MagickImage( fileName );
                                
                int width = magicImage.Width;
                int height = magicImage.Height;
                PixelFormat pixelFormat = PixelFormats.Bgra32;

                WriteableBitmap resultWriteableBitmap = 
                    WriteableBitmapCreator.CreateWriteableBitmap( width, height, (int)dpiX, (int)dpiY, pixelFormat );
                WriteableBitmapWrapper imageWrapper = WriteableBitmapWrapper.Create( resultWriteableBitmap );

                PixelCollection pixelCollection = magicImage.GetPixels();

                Interval<double> interval1 = new Interval<double>( 0, ushort.MaxValue );
                Interval<double> interval2 = new Interval<double>( 0, byte.MaxValue );
                RealIntervalTransform intervalTransform = new RealIntervalTransform( interval1, interval2 );

                RealMatrix redMatrix = new RealMatrix( height, width );
                
                for ( int x = 0; x < width; x++ ) {
                    for ( int y = 0; y < height; y++ ) {
                        Pixel pixel = pixelCollection.GetPixel(x, y);
                        MagickColor magicColor = pixel.ToColor();

                        redMatrix[ y, x ] = intervalTransform.TransformToFinishIntervalValue(magicColor.R);
                        
                        byte a = Convert.ToByte( intervalTransform.TransformToFinishIntervalValue( magicColor.A ) );
                        byte r = Convert.ToByte( intervalTransform.TransformToFinishIntervalValue( magicColor.R ) );
                        byte g = Convert.ToByte( intervalTransform.TransformToFinishIntervalValue( magicColor.G ) );
                        byte b = Convert.ToByte( intervalTransform.TransformToFinishIntervalValue( magicColor.B ) );

                        Color color = Color.FromArgb( a, r, g, b );

                        imageWrapper.SetPixelColor( x, y, color );
                    }
                }

                ExtraImageInfo extraImageInfo = new ExtraImageInfo();
                extraImageInfo.RedMatrix = redMatrix;
                extraImageInfo.Image = resultWriteableBitmap;
                
                //return resultWriteableBitmap;
                return extraImageInfo;

            }
            */
            
            /*
            if ( fileName.ToUpper().EndsWith( "CR2" ) ) {

                
                //System.GC.Collect();

                //BitmapDecoder bitmapDecoder =
                //    BitmapDecoder.Create( new Uri( fileName ), 
                //    BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None );

                //BitmapFrame frame = bitmapDecoder.Frames[ 0 ];
                
                //WriteableBitmap bitmap = new WriteableBitmap( frame );
                //WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( bitmap );

                //Color c = wrapper.GetPixelColor( 300, 300 );
                

                System.GC.Collect();

                uint error;

                IntPtr imgRef;
                //Open image
                IntPtr inStream;
                error = EDSDK.EdsCreateFileStream(fileName, EDSDK.EdsFileCreateDisposition.OpenExisting, EDSDK.EdsAccess.Read, out inStream);
                EDSDK.EdsCreateImageRef(inStream, out imgRef);
                EDSDK.EdsRelease(inStream);

                string convertedFileName = Path.ChangeExtension(fileName, "TIFF");

                //Save image
                IntPtr outStream;
                var settings = new EDSDK.EdsSaveImageSetting();
                error = EDSDK.EdsCreateFileStream(convertedFileName, EDSDK.EdsFileCreateDisposition.CreateAlways, EDSDK.EdsAccess.Write, out outStream);
                error = EDSDK.EdsSaveImage(imgRef, EDSDK.EdsTargetImageType.TIFF16, settings, outStream);
                EDSDK.EdsRelease(outStream);

                ExtraImageInfo extraImageInfo = new ExtraImageInfo();
                return extraImageInfo;

            }
            */
            if (fileName.ToUpper().EndsWith("CR2"))
            {
                Interval<double> finishInterval = new Interval<double>(0, 255);

                IntegerMatrix matrix = RawReader.ReadImageFromFile(fileName);
                //matrix = matrix.GetSubMatrix(0, 2640 - 1, matrix.RowCount - 1, matrix.ColumnCount - 1);
                               
                
                int min = matrix.GetMinValue();
                int max = matrix.GetMaxValue();
                                                
                IntegerMatrix redMatrix = InterpolationHelper.GetRedMatrix(matrix);
                int minRed = redMatrix.GetMinValue();
                int maxRed = redMatrix.GetMaxValue();

                IntegerMatrix greenMatrix = InterpolationHelper.GetGreenMatrix(matrix);
                int minGreen = greenMatrix.GetMinValue();
                int maxGreen = greenMatrix.GetMaxValue();
                
                IntegerMatrix blueMatrix = InterpolationHelper.GetBlueMatrix(matrix);
                int minBlue = blueMatrix.GetMinValue();
                int maxBlue = blueMatrix.GetMaxValue();

                int minValue = (new int[] { minRed, minGreen, minBlue }).Min();
                int maxValue = (new int[] { maxRed, maxGreen, maxBlue }).Max();
                
                RealIntervalTransform redIntervalTransform = 
                    new RealIntervalTransform(new Interval<double>(minValue, maxValue), finishInterval);
                IntegerMatrix resRedMatrix = 
                    IntegerMatrixValuesTransform.TransformMatrixValues(redMatrix, redIntervalTransform);
                                
                RealIntervalTransform greenIntervalTransform =
                    new RealIntervalTransform(new Interval<double>(minValue, maxValue), finishInterval);
                IntegerMatrix resGreenMatrix =
                    IntegerMatrixValuesTransform.TransformMatrixValues(greenMatrix, greenIntervalTransform);
                                
                RealIntervalTransform blueIntervalTransform =
                    new RealIntervalTransform(new Interval<double>(minValue, maxValue), finishInterval);
                IntegerMatrix resBlueMatrix =
                    IntegerMatrixValuesTransform.TransformMatrixValues(blueMatrix, blueIntervalTransform);
                                
                WriteableBitmap resImage =
                    WriteableBitmapCreator.CreateWriteableBitmapFromMatricesRGB
                    (resRedMatrix, resGreenMatrix, resBlueMatrix, OS.OS.IntegerSystemDpiX, OS.OS.IntegerSystemDpiY);
                                
                /*
                IntegerMatrix resMatrix =
                    IntegerMatrixValuesTransform.TransformMatrixValuesToFinishIntervalValues(matrix, finishInterval);

                WriteableBitmap resImage =
                    WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                    (resMatrix, OS.OS.IntegerSystemDpiX, OS.OS.IntegerSystemDpiY);
                */

                ExtraImageInfo extraImageInfo = new ExtraImageInfo();
                extraImageInfo.Image = resImage;
                
                return extraImageInfo;
            }

            else
            {

                Stream imageStreamSource = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                                
                BitmapDecoder decoder = BitmapDecoder.Create(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                BitmapSource bitmapSource = decoder.Frames[0];

                WriteableBitmap resultWriteableBitmap = new WriteableBitmap(bitmapSource);

                ExtraImageInfo extraImageInfo = new ExtraImageInfo();
                extraImageInfo.Image = resultWriteableBitmap;

                return extraImageInfo;
            }
        }
        //------------------------------------------------------------------------------------------------
        //Создание изображения из файла
        public static WriteableBitmap CreateWriteableBitmapFromFile( string fileName ) {
            BitmapImage bitmapImage = BitmapImageCreator.CreateBitmapImageFromFile( fileName );
            WriteableBitmap writeableBitmap = new WriteableBitmap( bitmapImage );
            return writeableBitmap;
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //Создание изображения
        public static WriteableBitmap CreateWriteableBitmap(
            int pixelWidth, int pixelHeight,
            int dpiX, int dpiY,
            PixelFormat pixelFormat
        ) {
            WriteableBitmap writeableBitmap = new WriteableBitmap
                ( pixelWidth, pixelHeight, dpiX, dpiY, pixelFormat, null );
            return writeableBitmap;
        }
        //------------------------------------------------------------------------------------------------
        //Создание изображения из матрицы интенсивностей серого
        public static WriteableBitmap CreateGrayScaleWriteableBitmapFromMatrix(
            RealMatrix grayScaleMatrix,
            int dpiX, int dpiY
        ) {
            int pixelWidth = grayScaleMatrix.ColumnCount;
            int pixelHeight = grayScaleMatrix.RowCount;
            WriteableBitmap writeableBitmap = WriteableBitmapCreator.CreateWriteableBitmap
                ( pixelWidth, pixelHeight, dpiX, dpiY, PixelFormats.Bgra32 );
            WriteableBitmapWrapper bitmapWrapper = WriteableBitmapWrapper.Create( writeableBitmap );
            for ( int x = 0; x < pixelWidth; x++ ) {
                for ( int y = 0; y < pixelHeight; y++ ) {
                    int grayIntensity = ( int )Math.Round( grayScaleMatrix[ y, x ] );
                    byte red, green, blue;
                    red = green = blue = Convert.ToByte( grayIntensity );
                    Color color = Color.FromRgb( red, green, blue );
                    bitmapWrapper.SetPixelColor( x, y, color );
                }
            }
            return writeableBitmap;
        }
        //------------------------------------------------------------------------------------------------
        //Создание изображения из матрицы интенсивностей серого
        public static WriteableBitmap CreateGrayScaleWriteableBitmapFromMatrix(
            IntegerMatrix grayScaleMatrix,
            int dpiX, int dpiY
        )
        {
            int pixelWidth = grayScaleMatrix.ColumnCount;
            int pixelHeight = grayScaleMatrix.RowCount;
            WriteableBitmap writeableBitmap = WriteableBitmapCreator.CreateWriteableBitmap
                (pixelWidth, pixelHeight, dpiX, dpiY, PixelFormats.Bgra32);
            WriteableBitmapWrapper bitmapWrapper = WriteableBitmapWrapper.Create(writeableBitmap);
            for (int x = 0; x < pixelWidth; x++)
            {
                for (int y = 0; y < pixelHeight; y++)
                {
                    int grayIntensity = grayScaleMatrix[y, x];
                    byte red, green, blue;
                    red = green = blue = Convert.ToByte(grayIntensity);
                    Color color = Color.FromRgb(red, green, blue);
                    bitmapWrapper.SetPixelColor(x, y, color);
                }
            }
            return writeableBitmap;
        }
        //------------------------------------------------------------------------------------------------
        //Создание изображения из матрицы интенсивностей серого
        public static WriteableBitmap CreateGrayScaleWriteableBitmapFromMatrix(
            RealMatrix grayScaleMatrix,
            int dpiX, int dpiY, Color negativeIntensityReplacedColor
        ) {
            int pixelWidth = grayScaleMatrix.ColumnCount;
            int pixelHeight = grayScaleMatrix.RowCount;
            WriteableBitmap writeableBitmap = WriteableBitmapCreator.CreateWriteableBitmap
                ( pixelWidth, pixelHeight, dpiX, dpiY, PixelFormats.Bgra32 );
            WriteableBitmapWrapper bitmapWrapper = WriteableBitmapWrapper.Create( writeableBitmap );
            for ( int x = 0; x < pixelWidth; x++ ) {
                for ( int y = 0; y < pixelHeight; y++ ) {
                    Color color;
                    int grayIntensity = ( int )Math.Round( grayScaleMatrix[ y, x ] );
                    if ( grayIntensity >= 0 ) {
                        byte red, green, blue;
                        red = green = blue = Convert.ToByte( grayIntensity );
                        color = Color.FromRgb( red, green, blue );
                    }
                    else {
                        color = negativeIntensityReplacedColor;
                    }
                    bitmapWrapper.SetPixelColor( x, y, color );
                }
            }
            return writeableBitmap;
        }
        //------------------------------------------------------------------------------------------------
        //Создание изображения из матрицы интенсивностей серого по шаблону
        public static WriteableBitmap CreateGrayScaleWriteableBitmapFromMatrix(
            RealMatrix grayScaleMatrix,
            int dpiX, int dpiY, BitMask2D bitMask
        ) {
            int pixelWidth = grayScaleMatrix.ColumnCount;
            int pixelHeight = grayScaleMatrix.RowCount;
            WriteableBitmap writeableBitmap = WriteableBitmapCreator.CreateWriteableBitmap
                ( pixelWidth, pixelHeight, dpiX, dpiY, PixelFormats.Bgra32 );
            WriteableBitmapWrapper bitmapWrapper = WriteableBitmapWrapper.Create( writeableBitmap );
            for ( int x = 0; x < pixelWidth; x++ ) {
                for ( int y = 0; y < pixelHeight; y++ ) {
                    if ( bitMask[ y, x ] == true ) {
                        int grayIntensity = ( int )Math.Round( grayScaleMatrix[ y, x ] );
                        byte red, green, blue;
                        red = green = blue = Convert.ToByte( grayIntensity );
                        Color color = Color.FromRgb( red, green, blue );
                        bitmapWrapper.SetPixelColor( x, y, color );
                    }
                }
            }
            return writeableBitmap;
        }
        //------------------------------------------------------------------------------------------------
        //Создание изображения из матриц R G B
        public static WriteableBitmap CreateWriteableBitmapFromMatricesRGB(
            RealMatrix redMatrix, RealMatrix greenMatrix, RealMatrix blueMatrix,
            int dpiX, int dpiY
        ) {
            int width = redMatrix.ColumnCount;
            int height = redMatrix.RowCount;
            PixelFormat pixelFormat = PixelFormats.Bgra32;
            WriteableBitmap newImage = WriteableBitmapCreator.CreateWriteableBitmap
                ( width, height, dpiX, dpiY, pixelFormat );
            WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( newImage );

            byte[] pixelBytes = new byte[4 * width * height];   //BGRA
            int pixelByteIndex = 0;

            for (int y = 0; y < newImage.PixelHeight; y++)
            {
                for ( int x = 0; x < newImage.PixelWidth; x++ ) {
                
                    pixelBytes[pixelByteIndex++] = Convert.ToByte(blueMatrix[y, x]);
                    pixelBytes[pixelByteIndex++] = Convert.ToByte(greenMatrix[y, x]);
                    pixelBytes[pixelByteIndex++] = Convert.ToByte(redMatrix[y, x]);
                    pixelBytes[pixelByteIndex++] = byte.MaxValue;
                }
            }

            wrapper.WritePixels(pixelBytes);

            return newImage;
        }
        //------------------------------------------------------------------------------------------------
        //Создание изображения из матриц R G B
        public static WriteableBitmap CreateWriteableBitmapFromMatricesRGB(
            IntegerMatrix redMatrix, IntegerMatrix greenMatrix, IntegerMatrix blueMatrix,
            int dpiX, int dpiY
        )
        {
            int width = redMatrix.ColumnCount;
            int height = redMatrix.RowCount;
            PixelFormat pixelFormat = PixelFormats.Bgra32;
            WriteableBitmap newImage = WriteableBitmapCreator.CreateWriteableBitmap
                (width, height, dpiX, dpiY, pixelFormat);
            WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create(newImage);

            byte[] pixelBytes = new byte[4 * width * height];   //BGRA
            int pixelByteIndex = 0;

            for (int y = 0; y < newImage.PixelHeight; y++)
            {
                for (int x = 0; x < newImage.PixelWidth; x++)
                {
                                
                    pixelBytes[pixelByteIndex++] = Convert.ToByte(blueMatrix[y, x]);
                    pixelBytes[pixelByteIndex++] = Convert.ToByte(greenMatrix[y, x]);
                    pixelBytes[pixelByteIndex++] = Convert.ToByte(redMatrix[y, x]);
                    pixelBytes[pixelByteIndex++] = byte.MaxValue;
                }
            }

            wrapper.WritePixels(pixelBytes);

            return newImage;
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
    }
}
