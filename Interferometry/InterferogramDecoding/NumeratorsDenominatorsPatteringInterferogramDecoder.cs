using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;
using ExtraLibrary.Mathematics;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Vectors;
using ExtraLibrary.Mathematics.Approximation;
using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Arraying.ArrayCreation;

namespace Interferometry.InterferogramDecoding {
    
    public class NumeratorsDenominatorsPatteringInterferogramDecoder : InterferogramDecoder {
        //---------------------------------------------------------------------------------------------
        //Точки эллипса
        public Point2D[] EllipsePoints {
            get;
            protected set;
        }
        //---------------------------------------------------------------------------------------------
        public bool ExecutePointsCorrecting {
            get;
            set;
        }
        //---------------------------------------------------------------------------------------------
        public NumeratorsDenominatorsPatteringInterferogramDecoder() {
            this.ExecutePointsCorrecting = true;
        }
        //---------------------------------------------------------------------------------------------
        //Случайные фазовые сдвиги
        private double[] GetRandomPhaseShifts( int count ) {
            double[] phaseShifts = ArrayCreator.CreateRandomArray( count );
            ComputationalFunction function = new ComputationalFunction( delegate( double x ) {
                return x * 2 * Math.PI;
            } );
            phaseShifts = ArrayOperator.ComputeFunction( phaseShifts, function );

            phaseShifts = new double[] { 0, 2 * Math.PI / 3, 4 * Math.PI / 3 };
            return phaseShifts;
        }
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        public InterferogramDecodingResult DecodeInterferogram(
            BitMask2D bitMask, params RealMatrix[] interferograms
        ) {
            //Выбранные точки изображения
            Point[] selectedImagePoints = bitMask.GetTruePoints();

            double[] phaseShifts = this.GetRandomPhaseShifts( interferograms.Length );
            GenericInterferogramDecoder genericInterferogramDecoder = new GenericInterferogramDecoder();
            double[] numerators = genericInterferogramDecoder.GetDecodingFormulaNumerators
                ( interferograms, phaseShifts, bitMask );
            double[] denominators = genericInterferogramDecoder.GetDecodingFormulaDenominators
                ( interferograms, phaseShifts, bitMask );

            Point2D[] points2D = PlaneManager.CreatePoints2D( numerators, denominators );

            if ( this.ExecutePointsCorrecting ) {
                points2D = this.GetTransformedPoints( points2D );
            }
            int sizeX = interferograms[ 0 ].ColumnCount;
            int sizeY = interferograms[ 0 ].RowCount;
            RealMatrix resultMatrix =
                this.CalculatePhaseMatrix( points2D, selectedImagePoints, sizeX, sizeY );
            InterferogramDecodingResult decodingResult = new InterferogramDecodingResult( resultMatrix );
            return decodingResult;
        }
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        //Преобразовать точки к окружности
        private Point2D[] GetTransformedPoints( params Point2D[] points2D ) {
            //this.EllipsePoints = points2D;

            EllipseApproximator ellipseApproximator = new EllipseApproximator();
            EllipseDescriptor ellipseDescriptor = ellipseApproximator.Approximate( points2D );

            Point2D ellipseCentre = ellipseDescriptor.GetCentre();
            points2D = PlaneManager.DisplacePoints( points2D, -ellipseCentre.X, -ellipseCentre.Y );

            double rotationAngle =
                ellipseDescriptor.GetAngleBetweenOxAndPrincipalAxis();

            points2D = this.RotatePoints( -rotationAngle, points2D );
            EllipseOrientaion pointsOrientation = this.GetPointsOrientation( points2D );
            if ( pointsOrientation == EllipseOrientaion.AxisY ) {
                points2D = this.RotatePoints( Math.PI / 2, points2D  );
            }

            double radiusX = ellipseDescriptor.GetSemiMajorAxis();
            double radiusY = ellipseDescriptor.GetSemiMinorAxis();

            double stretchCoefficient = radiusX / radiusY;
            Point2D[] stretchPoints = this.StretchPointsAlongAxisY( stretchCoefficient, points2D );

            this.EllipsePoints = stretchPoints;
            return stretchPoints;
        }
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        //Растяжение вдоль оси Y
        private Point2D[] StretchPointsAlongAxisY( double stretchCoefficient, params Point2D[] points ) {
            Point2D[] newPoints = new Point2D[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point2D point = points[ index ];
                double newX = point.X;
                double newY = point.Y * stretchCoefficient;
                Point2D newPoint = new Point2D( newX, newY );
                newPoints[ index ] = newPoint;
            }
            return newPoints;
        }
        //---------------------------------------------------------------------------------------------
        //Растяжение вдоль оси X
        private Point2D[] StretchPointsLongAxisX( double stretchCoefficient, params Point2D[] points ) {
            Point2D[] newPoints = new Point2D[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point2D point = points[ index ];
                double newX = point.X * stretchCoefficient;
                double newY = point.Y;
                Point2D newPoint = new Point2D( newX, newY );
                newPoints[ index ] = newPoint;
            }
            return newPoints;
        }
        //---------------------------------------------------------------------------------------------
        //Корректировка точек к эллипсу    
        private Point2D[] CorrectPointsToCanonicalEllipse(
            CanonicalEllipseDescriptor canonicalEllipseDescriptor,
            params Point2D[] points
        ) {
            Point2D[] ellipsePoints = new Point2D[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point2D point = points[ index ];
                Point2D ellipsePoint = canonicalEllipseDescriptor.GetNearestEllipsePoint( point );
                ellipsePoints[ index ] = ellipsePoint;
            }

            return ellipsePoints;
        }
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        //Растяжение до окружности
        private Point2D[] StretchToCircleAlongAxisY(
            CircleDescriptor circleDescriptor,
            double principalAxisCoordinateY,
            params Point2D[] points
        ) {
            Point2D[] newPoints = new Point2D[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point2D point = points[ index ];
                double x = point.X;
                double y = point.Y;
                double[] coordinatesY = circleDescriptor.GetCoordinatesY( x );
                double y1 = coordinatesY[ 0 ];
                double y2 = coordinatesY[ 1 ];
                double newY;
                if ( y < principalAxisCoordinateY ) {
                    newY = y1 < circleDescriptor.Centre.Y ? y1 : y2;
                }
                else {
                    newY = circleDescriptor.Centre.Y <= y1 ? y1 : y2;
                }
                Point2D newPoint = new Point2D( x, newY );
                newPoints[ index ] = newPoint;
            }
            return newPoints;
        }
        //---------------------------------------------------------------------------------------------
        //Определение ориентации точек
        private EllipseOrientaion GetPointsOrientation( params Point2D[] points2D ) {
            EllipseOrientaion orientation = EllipseOrientaion.Undefined;
            double sumCoordinatesX = 0;
            double sumCoordinatesY = 0;
            for ( int index = 0; index < points2D.Length; index++ ) {
                Point2D point = points2D[ index ];
                sumCoordinatesX += point.X;
                sumCoordinatesY += point.Y;
            }
            double sumsRatio = sumCoordinatesX / sumCoordinatesY;
            if ( sumsRatio < 1 ) {
                orientation = EllipseOrientaion.AxisY;
            }
            if ( 1 <= sumsRatio ) {
                orientation = EllipseOrientaion.AxisX;
            }
            return orientation;
        }
        //---------------------------------------------------------------------------------------------
        //Поворот до оси X
        private Point2D[] RotatePoints( double angle, params Point2D[] points2D ) {
            RealVector[] coordinatesVectors = PlaneManager.CreateVectorsFromPoints( points2D );
            RealMatrix rotationMatrix = PlaneManager.GetRotationMatrixAnticlockwise( angle );
            RealVector[] newCoordinatesVectors =
                PlaneManager.RotateVectros( coordinatesVectors, rotationMatrix );
            Point2D[] newPoints = PlaneManager.CreatePoints2DFromVectors( newCoordinatesVectors );
            return newPoints;
        }
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        //Вычисление матрицы фаз
        private RealMatrix CalculatePhaseMatrix(
            Point2D[] points, Point[] imagePoints,
            int sizeX, int sizeY
        ) {
            bool sizeEuqality = ArrayOperator.IsArraySizesEqual( points, imagePoints );
            if ( !sizeEuqality ) {
                throw new Exception();
            }
            RealMatrix phaseMatrix = new RealMatrix( sizeY, sizeX, this.DefaultPhaseValue );
            for ( int index = 0; index < points.Length; index++ ) {
                Point2D point = points[ index ];
                Point imagePoint = imagePoints[ index ];
                double phase = this.CalculatePhase( point.X, point.Y );
                phaseMatrix[ imagePoint.Y, imagePoint.X ] = phase;
            }
            return phaseMatrix;
        }
        //---------------------------------------------------------------------------------------------
        //Вычисление фазы в точке
        private double CalculatePhase( double x, double y ) {
            double phase = Mathem.Arctg( y, x );
            return phase;
        }
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
    }
}
