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
using System.IO;

using ExtraControls;
using ExtraMVVM;
using ExtraLibrary.Imaging;
using ExtraLibrary.OS;
using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Arraying.ArrayCreation;
using ExtraLibrary.Converting;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Vectors;
using ExtraLibrary.Mathematics.Statistics;
using ExtraLibrary.Mathematics.Sets;
using ExtraLibrary.Mathematics.Progressions;
using ExtraLibrary.Randomness;
using ExtraLibrary.Mathematics.Transformation;
using ExtraLibrary.Imaging.ImageProcessing;
using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;
using ExtraLibrary.Collections;
using ExtraLibrary.InputOutput;

using Interferometry.InterferogramProcessing;
using Interferometry.InterferogramCreation;
using Interferometry.InterferogramDecoding;
using Interferometry.DeviceControllers;
using Interferometry.Helpers;
#endregion

namespace InterferogramProcessing {
    public class DecodingManager {
        //-------------------------------------------------------------------------------------------------
        public Point3D[] CylinderPoints {
            get;
            private set;
        }
        //-------------------------------------------------------------------------------------------------
        public Point3D[] CylinderCirclePoints {
            get;
            private set;
        }
        //-------------------------------------------------------------------------------------------------
        public PlaneDescriptor CirclePointsPlane {
            get;
            private set;
        }
        //-------------------------------------------------------------------------------------------------
        public RealMatrix LastPhaseMatrix {
            get;
            private set;
        }
        //-------------------------------------------------------------------------------------------------
        public RealVector EigenVector1 {
            get;
            private set;
        }
        //-------------------------------------------------------------------------------------------------
        public RealVector EigenVector2 {
            get;
            private set;
        }
        //-------------------------------------------------------------------------------------------------
        public Point2D[] PlaneXyPoints {
            get;
            private set;
        }

        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        //RestrictedCapacityList<Point3D[]> circlePointsCollection = new RestrictedCapacityList<Point3D[]>( 2 );
        long counter = 0;
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        //Фазовые сдвиги
        private double[] GetPhaseShifts() {
            //double[] phaseShifts = new double[] { 0, 2* Math.PI / 3, 4 * Math.PI / 3 };
            double[] phaseShifts = new double[] { 0, Math.PI / 3, 2 * Math.PI / 3 };
            return phaseShifts;
        }
        //-------------------------------------------------------------------------------------------------
        //Трехточечный алгоритм расшифровки
        public RealMatrix DecodeImageByThreePhaseShiftsAlgorithm(
            RealMatrix[] interferograms, double[] phaseShifts, BitMask2D bitMask
        ) {
            ThreePointInterferogramDecoder threePointInterferogramDecoder =
                new ThreePointInterferogramDecoder();
            InterferogramDecodingResult decodingResult = 
                threePointInterferogramDecoder.DecodeInterferogram( interferograms, phaseShifts, bitMask );
            
            RealMatrix phaseMatrix = decodingResult.ResultMatrix;
            this.LastPhaseMatrix = phaseMatrix;
            
            RealMatrix grayScaledPhaseMatrix =
                MatricesProcessingHelper.GetGrayScaledMatrix( phaseMatrix, bitMask );
            return grayScaledPhaseMatrix;
        }
        //-------------------------------------------------------------------------------------------------
        //Обобщенный алгоритм расшифровки
        public RealMatrix DecodeImageByGenericAlgorithm(
            RealMatrix[] interferograms, double[] phaseShifts, BitMask2D bitMask
        ) {
            GenericInterferogramDecoder genericInterferogramDecoder = new GenericInterferogramDecoder();
                       
            InterferogramDecodingResult decodingResult =
                genericInterferogramDecoder.DecodeInterferogram( interferograms, phaseShifts, bitMask );

            RealMatrix phaseMatrix = decodingResult.ResultMatrix;
            this.LastPhaseMatrix = phaseMatrix;

            RealMatrix grayScaledPhaseMatrix = MatricesProcessingHelper.GetGrayScaledMatrix( phaseMatrix, bitMask );
                
            return grayScaledPhaseMatrix;
        }

        //-------------------------------------------------------------------------------------------------
        public RealMatrix DecodeImageByEstimatedAnglesAlgorithm(
           RealMatrix[] interferograms, BitMask2D bitMask
        ) {
            double phaseShift1 = 0;
            double phaseShift2 =
                Math.Acos( Statistician.GetMatricesPearsonsCorrelationCcoefficient( interferograms[ 0 ], interferograms[ 1 ] ) );
            double phaseShift3 =
                phaseShift2 +
                Math.Acos( Statistician.GetMatricesPearsonsCorrelationCcoefficient( interferograms[ 1 ], interferograms[ 2 ] ) );

            double[] phaseShifts = new double[] { phaseShift1, phaseShift2, phaseShift3 };
            
            //GenericInterferogramDecoder interferogramDecoder = new GenericInterferogramDecoder();
            ThreePointInterferogramDecoder interferogramDecoder = new ThreePointInterferogramDecoder();

            InterferogramDecodingResult decodingResult =
                interferogramDecoder.DecodeInterferogram( interferograms, phaseShifts, bitMask );

            RealMatrix phaseMatrix = decodingResult.ResultMatrix;
            this.LastPhaseMatrix = phaseMatrix;

            RealMatrix grayScaledPhaseMatrix = MatricesProcessingHelper.GetGrayScaledMatrix( phaseMatrix, bitMask );

            return grayScaledPhaseMatrix;
        }

        //-------------------------------------------------------------------------------------------------
        //Усовершенствованный алгоритм расшифровки ( по цилиндру )
        public RealMatrix DecodeImageByTrajectoryDescriptionByCylinderAlgorithm(
            RealMatrix[] interferograms, BitMask2D bitMask
        ) {
            TrajectoryDescriptionByCylinderInterferogramDecoder interferogramDecoder =
                new TrajectoryDescriptionByCylinderInterferogramDecoder();

            try {
                InterferogramDecodingResult decodingResult =
                    interferogramDecoder.DecodeInterferogram( interferograms, bitMask );


                RealMatrix phaseMatrix = decodingResult.ResultMatrix;
                this.LastPhaseMatrix = phaseMatrix;

                this.CylinderPoints = interferogramDecoder.TrajectoryPoints;
                this.CylinderPoints = interferogramDecoder.OrthogonalVectorsPoints;
                this.CylinderCirclePoints = interferogramDecoder.CirclePoints;

                //this.EigenVector1 = interferogramDecoder.EigenVector1;
                //this.EigenVector2 = interferogramDecoder.EigenVector2;
                //this.PlaneXyPoints = interferogramDecoder.PlaneXYPoints;

                /*
                
                */

                this.CirclePointsPlane = interferogramDecoder.CirclePointsPlane;

                RealMatrix grayScaledPhaseMatrix =
                    MatricesProcessingHelper.GetGrayScaledMatrix( phaseMatrix, bitMask );
                return grayScaledPhaseMatrix;
            }
            catch ( EllipseException ellipseException ) {
                return null;
            }
        }
        
        //-------------------------------------------------------------------------------------------------
        private void SaveCylinderInfoToFiles
            ( Point3D[] cylinderPoints, Point3D[] circlePoints, RealMatrix[] interferograms ) {
            
            //Saving Info to files
            double[] arrayX1 = SpaceManager.GetCoordinatesX( this.CylinderPoints );
            double[] arrayY1 = SpaceManager.GetCoordinatesY( this.CylinderPoints );
            double[] arrayZ1 = SpaceManager.GetCoordinatesZ( this.CylinderPoints );

            double[] arrayX2 = SpaceManager.GetCoordinatesX( this.CylinderCirclePoints );
            double[] arrayY2 = SpaceManager.GetCoordinatesY( this.CylinderCirclePoints );
            double[] arrayZ2 = SpaceManager.GetCoordinatesZ( this.CylinderCirclePoints );

            double minX1 = arrayX1.Min();
            double minY1 = arrayY1.Min();
            double minZ1 = arrayZ1.Min();

            double minX2 = arrayX2.Min();
            double minY2 = arrayY2.Min();
            double minZ2 = arrayZ2.Min();


            double maxX1 = arrayX1.Max();
            double maxY1 = arrayY1.Max();
            double maxZ1 = arrayZ1.Max();

            double maxX2 = arrayX2.Max();
            double maxY2 = arrayY2.Max();
            double maxZ2 = arrayZ2.Max();

            Interval<double> finishInterval = new Interval<double>( 0, 255 );

            RealIntervalTransform transformX1 =
                new RealIntervalTransform( new Interval<double>( minX1, maxX1 ), finishInterval );
            RealIntervalTransform transformY1 =
                new RealIntervalTransform( new Interval<double>( minY1, maxY1 ), finishInterval );
            RealIntervalTransform transformZ1 =
                new RealIntervalTransform( new Interval<double>( minZ1, maxZ1 ), finishInterval );


            RealIntervalTransform transformX2 =
                new RealIntervalTransform( new Interval<double>( minX2, maxX2 ), finishInterval );
            RealIntervalTransform transformY2 =
                new RealIntervalTransform( new Interval<double>( minY2, maxY2 ), finishInterval );
            RealIntervalTransform transformZ2 =
                new RealIntervalTransform( new Interval<double>( minZ2, maxZ2 ), finishInterval );
            
            int width = interferograms[ 0 ].ColumnCount;
            int height = interferograms[ 0 ].RowCount;

            WriteableBitmap image1 =
                WriteableBitmapCreator.CreateWriteableBitmap
                ( width, height, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, PixelFormats.Bgra32 );
            WriteableBitmapWrapper wrapper1 = WriteableBitmapWrapper.Create( image1 );

            WriteableBitmap image2 =
                WriteableBitmapCreator.CreateWriteableBitmap
                ( width, height, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, PixelFormats.Bgra32 );
            WriteableBitmapWrapper wrapper2 = WriteableBitmapWrapper.Create( image2 );

            int x = 0;
            int y = 0;

            for ( int index = 0; index < this.CylinderPoints.Length; index++ ) {

                Point3D point1 = this.CylinderPoints[ index ];
                Point3D point2 = this.CylinderCirclePoints[ index ];

                byte x1 = ( byte )( int )transformX1.TransformToFinishIntervalValue( point1.X );
                byte y1 = ( byte )( int )transformY1.TransformToFinishIntervalValue( point1.Y );
                byte z1 = ( byte )( int )transformZ1.TransformToFinishIntervalValue( point1.Z );

                byte x2 = ( byte ) ( int )transformX2.TransformToFinishIntervalValue( point2.X );
                byte y2 = ( byte ) ( int )transformY2.TransformToFinishIntervalValue( point2.Y );
                byte z2 = ( byte ) ( int )transformZ2.TransformToFinishIntervalValue( point2.Z );

                System.Windows.Media.Color color1 = System.Windows.Media.Color.FromArgb( 255, x1, y1, z1 );
                System.Windows.Media.Color color2 = System.Windows.Media.Color.FromArgb( 255, x2, y2, z2 );

                wrapper1.SetPixelColor( x, y, color1 );
                wrapper2.SetPixelColor( x, y, color2 );
                
                x++;
                if ( x == width - 1 ) {
                    x = 0;
                    y++;
                }
            }

            //wrapper1.SaveToBmpFile( "image1.bmp" );
            //wrapper2.SaveToBmpFile("")


            //End Saving
        }

        //-------------------------------------------------------------------------------------------------
        //Супер алгоритм расшифровки ( числители и знаменатели )
        public RealMatrix DecodeImageByNumeratorsDenominatorsPatteringAlgorithm(
            RealMatrix[] interferograms, BitMask2D bitMask
        ) {
            NumeratorsDenominatorsPatteringInterferogramDecoder interferogramDecoder =
                new NumeratorsDenominatorsPatteringInterferogramDecoder();
            InterferogramDecodingResult decodingResult = 
                interferogramDecoder.DecodeInterferogram( bitMask, interferograms );
            
            RealMatrix phaseMatrix = decodingResult.ResultMatrix;
            this.LastPhaseMatrix = phaseMatrix;

            RealMatrix grayScaledPhaseMatrix = MatricesProcessingHelper.GetGrayScaledMatrix( phaseMatrix, bitMask );
            return grayScaledPhaseMatrix;
        }
        //-------------------------------------------------------------------------------------------------
        //Алгоритм расшифровки по ортогональным векторам
        public RealMatrix DecodeImageByOrthogonalVectorsAlgorithm(
            RealMatrix[] interferograms, BitMask2D bitMask
        ) {
            AdvancedInterferogramDecoder interferogramDecoder = new AdvancedInterferogramDecoder();
            InterferogramDecodingResult decodingResult =
                interferogramDecoder.DecodeInterferogram( bitMask, interferograms );
            
            RealMatrix phaseMatrix = decodingResult.ResultMatrix;
            this.LastPhaseMatrix = phaseMatrix;

            RealMatrix grayScaledPhaseMatrix =
                MatricesProcessingHelper.GetGrayScaledMatrix( phaseMatrix, bitMask );
            return grayScaledPhaseMatrix;
        }
        //-------------------------------------------------------------------------------------------------
        public RealMatrix DecodeImageByFirstHarmonicRotationAlgorithm( RealMatrix[] interferograms) {
            FirstHarmonicRotationInterferogramDecoder decoder = new FirstHarmonicRotationInterferogramDecoder();
            InterferogramDecodingResult decodingResult = decoder.DecodeInterferogram( interferograms );

            RealMatrix phaseMatrix = decodingResult.ResultMatrix;
            this.LastPhaseMatrix = phaseMatrix;
            RealMatrix grayScaledPhaseMatrix =
                MatricesProcessingHelper.GetGrayScaledMatrixFromMinusPI( phaseMatrix );
            return grayScaledPhaseMatrix;
        }
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
    }
}

