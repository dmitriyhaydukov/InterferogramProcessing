using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;

using ExtraLibrary.Mathematics.Sets;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Transformation;
using ExtraLibrary.Randomness;
using ExtraLibrary.ImageProcessing;
using ExtraLibrary.OS;
using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;

using Interferometry.InterferogramCreation;

namespace Interferometry.Helpers {
    public class InterferometryHelper {
        //--------------------------------------------------------------------------------------------------------------
        //Приведение матрицы фаз к диапазону 0..255 ( уровень серого )
        public static RealMatrix TrnsformPhaseMatrixToGrayScaleMatrix(
            RealMatrix matrix, BitMask2D bitMask, double prickedValue
        ) {
            Interval<double> startInterval = new Interval<double>( 0, 2 * Math.PI );
            Interval<double> finishInterval = new Interval<double>( 0, 255 );
            RealIntervalTransform intervalTransform = new RealIntervalTransform( startInterval, finishInterval );
            RealMatrix scaledMatrix =
                RealMatrixValuesTransform.TransformMatrixValues
                ( matrix, bitMask, intervalTransform, prickedValue );
            return scaledMatrix;
        }
        //--------------------------------------------------------------------------------------------------------------
        //Приведение матрицы фаз к диапазону 0..255 ( уровень серого )
        public static RealMatrix TrnsformPhaseMatrixToGrayScaleMatrix( RealMatrix matrix ) {
            Interval<double> startInterval = new Interval<double>( 0, 2 * Math.PI );
            Interval<double> finishInterval = new Interval<double>( 0, 255 );
            RealIntervalTransform intervalTransform = new RealIntervalTransform( startInterval, finishInterval );
            RealMatrix scaledMatrix =
                RealMatrixValuesTransform.TransformMatrixValues( matrix, intervalTransform );
            return scaledMatrix;
        }
        //--------------------------------------------------------------------------------------------------------------
        //Случайные фазовые сдвиги
        public static double[] GetRandomPhaseShifts( int count ) {
            RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
            double[] randomPhaseShifts = new double[ count ];
            for ( int index = 0; index < count; index++ ) {
                double randomValue = randomNumberGenerator.GetNextDouble();
                double randomPhaseShift = randomValue * 2 * Math.PI;
                randomPhaseShifts[ index ] = randomPhaseShift;
            }
            return randomPhaseShifts;
        }
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //Интерферограммы с линейными полосами
        public static RealMatrix[] GetLinearInterferograms(
            int width, int height, int noisePercent, int fringeCount, params double[] phaseShifts
        ) {
            InterferogramInfo interferogramInfo = new InterferogramInfo( width, height, noisePercent );
            InterferogramCreator interferogramCreator = 
                new LinearFringeInterferogramCreator( interferogramInfo, fringeCount );
            InterferogramGenerator interferogramGenerator = new InterferogramGenerator( interferogramCreator );
            RealMatrix[] interferograms = interferogramGenerator.GenerateInterferograms( phaseShifts );
            return interferograms;
        }
        //--------------------------------------------------------------------------------------------------------------
        //Интерферограммы с круговыми полосами
        public static RealMatrix[] GetCircleInterferograms(
            int width, int height, int noisePercent, params double[] phaseShifts
        ) {
            InterferogramInfo interferogramInfo = new InterferogramInfo( width, height, noisePercent );
            InterferogramCreator interferogramCreator = new CircleFringeInterferogramCreator( interferogramInfo );
            InterferogramGenerator interferogramGenerator = new InterferogramGenerator( interferogramCreator );
            RealMatrix[] interferograms = interferogramGenerator.GenerateInterferograms( phaseShifts );
            return interferograms;
        }
        //--------------------------------------------------------------------------------------------------------------
        //Интерферограммы с линейными полосами ( изображения )
        public static WriteableBitmap[] GetLinearInterferogramImages(
            int width, int height, int noisePercent, int fringeCount, params double[] phaseShifts
        ) {
            RealMatrix[] interferograms = InterferometryHelper.GetLinearInterferograms
                ( width, height, noisePercent, fringeCount, phaseShifts );
            int dpiX = Convert.ToInt32( OS.SystemDpiX );
            int dpiY = Convert.ToInt32( OS.SystemDpiY );
            WriteableBitmap[] images = WriteableBitmapsManager.CreateGrayScaleWriteableBitmapsFromMatrices
                ( dpiX, dpiY, interferograms );
            return images;
        }
        //--------------------------------------------------------------------------------------------------------------
        //Интерферограммы с круговыми полосами ( изображения )
        public static WriteableBitmap[] GetLinearInterferogramImages(
            int width, int height, int noisePercent, params double[] phaseShifts
        ) {
            RealMatrix[] interferograms = InterferometryHelper.GetCircleInterferograms
                ( width, height, noisePercent, phaseShifts );
            int dpiX = Convert.ToInt32( OS.SystemDpiX );
            int dpiY = Convert.ToInt32( OS.SystemDpiY );
            WriteableBitmap[] images = WriteableBitmapsManager.CreateGrayScaleWriteableBitmapsFromMatrices
                ( dpiX, dpiY, interferograms );
            return images;
        }
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //Точки в пространстве из интенсивностей интерферограмм
        public static Point3D[] GetSpatialPointsFromInterferograms( 
            RealMatrix[] interferograms, BitMask2D bitMask 
        ) {
            if ( interferograms.Length != 3 ) {
                throw new Exception();
            }
            int width = interferograms[ 0 ].ColumnCount;
            int height = interferograms[ 0 ].RowCount;

            List<Point3D> points = new List<Point3D>();
            for ( int x = 0; x < width; x++ ) {
                for ( int y = 0; y < height; y++ ) {
                    if ( bitMask[ y, x ] ) {
                        double[] intensities = MatricesManager.GeValuesFromMatrices( y, x, interferograms );
                        Point3D point = new Point3D( intensities );
                        points.Add( point );
                    }
                }
            }
            return points.ToArray();
        }
        //--------------------------------------------------------------------------------------------------------------
        //Точки в пространстве из интенсивностей для области интерферограммы, заданной прямоугольником
        public static Point3D[] GetSpatialPointsFromInterferograms(
            RealMatrix[] interferograms, Rectangle rectangle
        ) {
            List<Point3D> points = new List<Point3D>();
            int maxX = rectangle.X + rectangle.Width;
            int maxY = rectangle.Y + rectangle.Height;
            for ( int x = rectangle.X; x < maxX; x++ ) {
                for ( int y = rectangle.Y; y < maxY; y++ ) {
                    double[] intensities = MatricesManager.GeValuesFromMatrices( y, x, interferograms );
                    Point3D point = new Point3D( intensities );
                    points.Add( point );
                }
            }
            return points.ToArray();
        }
        //--------------------------------------------------------------------------------------------------------------
        //Точки в пространстве из интенсивностей для строки интерферограммы
        public static Point3D[] GetSpatialPointsFromInterferogramsRow(
            RealMatrix[] interferograms, BitMask2D bitMask, int row
        ) {
            if ( interferograms.Length != 3 ) {
                throw new Exception();
            }
            int width = interferograms[ 0 ].ColumnCount;
            int height = interferograms[ 0 ].RowCount;

            int y = row;
            List<Point3D> points = new List<Point3D>();
            for ( int x = 0; x < width; x++ ) {
                if ( bitMask[ y, x ] ) {
                    double[] intensities = MatricesManager.GeValuesFromMatrices( row, x, interferograms );
                    Point3D point = new Point3D( intensities );
                    points.Add( point );
                }
            }
            return points.ToArray();
        }
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
    }
}
