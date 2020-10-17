#region namespeces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;

using ExtraControls;
using ExtraMVVM;
using ExtraLibrary.ImageProcessing;
using ExtraLibrary.OS;
using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Arraying.ArrayCreation;
using ExtraLibrary.Converting;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Statistics;
using ExtraLibrary.Mathematics.Sets;
using ExtraLibrary.Mathematics.Progressions;
using ExtraLibrary.Randomness;
using ExtraLibrary.Mathematics.Transformation;
using ExtraLibrary.ImageProcessing;
using ExtraLibrary.Geometry2D;

using Interferometry.InterferogramProcessing;
using Interferometry.InterferogramCreation;
using Interferometry.InterferogramDecoding;
using Interferometry.DeviceControllers;
using Interferometry.Helpers;

#endregion


namespace InterferogramProcessing {
    public class InterferogramProcessingHelper {
        //--------------------------------------------------------------------------------------------------
        //Создание изображения из файла
        //public static WriteableBitmap CreateImageFromFile( string fileName ) {
        public static ExtraImageInfo CreateImageFromFile( string fileName ) {

            WriteableBitmap resultImage = null;

            double dpiX = OS.SystemDpiX;
            double dpiY = OS.SystemDpiY;

            ExtraImageInfo extraImageInfo = WriteableBitmapCreator.CreateWriteableBitmapFromFile( fileName, dpiX, dpiY );
                       
            bool isImageFormatGrayScale = WriteableBitmapWrapper.IsImageFormatGrayScale(extraImageInfo.Image);
            
            if ( isImageFormatGrayScale ) {
                resultImage = extraImageInfo.Image;
            }
            else if (extraImageInfo.Image.Format != PixelFormats.Bgra32) {
                PixelFormat pixelFormat = PixelFormats.Bgra32;
                WriteableBitmap newImage = WriteableBitmapConverter.ConvertWriteableBitmap( extraImageInfo.Image, pixelFormat );
                resultImage = newImage;
                extraImageInfo.Image = resultImage;
            }

            return extraImageInfo;
        }
        //--------------------------------------------------------------------------------------------------
        //Полутоновое изображение из матрицы
        public static WriteableBitmap GetGrayScaleImage( RealMatrix imageMatrix ) {
            int dpiX = Convert.ToInt32( OS.GetSystemDpiX() );
            int dpiY = Convert.ToInt32( OS.GetSystemDpiY() );
            WriteableBitmap image = WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                ( imageMatrix, dpiX, dpiY );
            return image;
        }
        //--------------------------------------------------------------------------------------------------
        //Фазовые сдвиги
        public static double[] GetPhaseShifts() {
            //double[] phaseShifts = new double[] { 0, Math.PI / 3, 4 * Math.PI / 3 };
            //double[] phaseShifts = new double[] { 0, 2* Math.PI / 9, 4 * Math.PI / 9 };
            //double[] phaseShifts = new double[] { 0, Math.PI / 2, Math.PI };

            //double[] phaseShifts = new double[] { 0, Math.PI / 2, 4 * Math.PI / 3 };

            List<double> list = new List<double>();
            for (double phase = 0; phase < 2 * Math.PI; phase += Math.PI / 3)
            {
                list.Add(phase);
            }

            return list.ToArray();
        }
        //--------------------------------------------------------------------------------------------------
        public static double[] GetPhaseShiftsForGenericAlgorithm() {
            //double[] phaseShifts = new double[] { 0, Math.PI / 3, 4 * Math.PI / 3 };
            //double[] phaseShifts = new double[] { 0, 2* Math.PI / 9, 4 * Math.PI / 9 };
            //double[] phaseShifts = new double[] { 0, Math.PI / 2, Math.PI, 3 * Math.PI / 2 };
            
            double[] phaseShifts = InterferogramProcessingHelper.GetPhaseShifts();
            return phaseShifts;
        }
        //--------------------------------------------------------------------------------------------------
        //Фазовые сдвиги для двухточечного алгоритма определения фазовых сдвигов
        public static double[] GetPhaseShiftsForTwoPointAlgorithmPhaseShiftEstimation() {
            double startPhaseShift= 0;
            double finishPhaseShift = 2 * Math.PI;
            double step = 0.5;

            List<double> phaseShifts = new List<double>();
            for ( double phaseShift = startPhaseShift; phaseShift <= finishPhaseShift; phaseShift += step ) {
                phaseShifts.Add( phaseShift );
            }

            return phaseShifts.ToArray();
        }
        //--------------------------------------------------------------------------------------------------
        //Зашумленные фазовые сдвиги
        public static double[] GetNoisyPhaseShifts( double noisePercent ) {
            double maxNoise = 2 * Math.PI / 100 * noisePercent;
            RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
            
            double[] truePhaseShifts = InterferogramProcessingHelper.GetPhaseShifts();
            double[] noisyPhaseShifts = new double[ truePhaseShifts.Length ];

            for ( int index = 0; index < truePhaseShifts.Length; index++ ) {
                double noise =
                    ( randomNumberGenerator.GetNextDouble() - 0.5 ) * 2 * maxNoise;
                noisyPhaseShifts[ index ] = truePhaseShifts[ index ] + noise;
            }
            
            return noisyPhaseShifts;
        }
        //--------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
    }
}
