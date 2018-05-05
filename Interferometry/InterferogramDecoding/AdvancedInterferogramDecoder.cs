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

namespace Interferometry.InterferogramDecoding {
    public class AdvancedInterferogramDecoder : BaseAdvancedInterferogramDecoder {
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        public AdvancedInterferogramDecoder() {
            //--
        }
        //---------------------------------------------------------------------------------------------
        public InterferogramDecodingResult DecodeInterferogram(
            BitMask2D bitMask, params RealMatrix[] interferograms
        ) {
            //Выбранные точки изображения
            Point[] selectedImagePoints = bitMask.GetTruePoints();
            
            //3D точки в плоскости, параллельной плоскости XY
            Point3D[] points3D = 
                this.GetParallelToCoordinatePlanePoints3D( interferograms, selectedImagePoints );

            Point2D[] points2D = SpaceManager.GetProjectionXY( points3D );
            this.ProjectionPoints = points2D;
            points2D = PlaneManager.DisplacePointsToFirstQuadrant( points2D );

            Point2D[] transformedPoints = this.GetTransformedPoints( points2D );           
            int sizeX = interferograms[0].ColumnCount;
            int sizeY = interferograms[0].RowCount;

            RealMatrix phaseMatrix =
                this.CalculatePhaseMatrix( transformedPoints, selectedImagePoints, sizeX, sizeY );

            InterferogramDecodingResult decodingResult = new InterferogramDecodingResult( phaseMatrix );

            return decodingResult;
        }
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        //Поворот до оси X
        private Point2D[] RotatePoints( Point2D[] points2D, double angle ) {
            RealVector[] coordinatesVectors = PlaneManager.CreateVectorsFromPoints( points2D );
            RealMatrix rotationMatrix = PlaneManager.GetRotationMatrixAnticlockwise( angle );
            RealVector[] newCoordinatesVectors = 
                PlaneManager.RotateVectros( coordinatesVectors, rotationMatrix );
            Point2D[] newPoints = PlaneManager.CreatePoints2DFromVectors( newCoordinatesVectors );
            return newPoints; 
        }
        //---------------------------------------------------------------------------------------------
        //Корректировка точек к эллипсу    
        private Point2D[] CorrectPointsToCanonicalEllipse(
            Point2D[] points,
            CanonicalEllipseDescriptor canonicalEllipseDescriptor
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
        //Растяжение до окружности
        private Point2D[] StretchToCircleAlongAxisY(
            Point2D[] points,
            CircleDescriptor circleDescriptor,
            double principalAxisCoordinateY
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
        //Преобразовать точки к окружности
        private Point2D[] GetTransformedPoints( Point2D[] points2D ) {
            EllipseApproximator ellipseApproximator = new EllipseApproximator();
            EllipseDescriptor ellipseDescriptor = ellipseApproximator.Approximate( points2D );

            Point2D ellipseCentre = ellipseDescriptor.GetCentre();
            points2D = PlaneManager.DisplacePoints( points2D, -ellipseCentre.X, -ellipseCentre.Y );

            double rotationAngle =
                ellipseDescriptor.GetAngleBetweenOxAndPrincipalAxis();

            points2D = this.RotatePoints( points2D, -rotationAngle );
            EllipseOrientaion pointsOrientation = this.GetPointsOrientation( points2D );
            if ( pointsOrientation == EllipseOrientaion.AxisY ) {
                points2D = this.RotatePoints( points2D, Math.PI / 2 );
            }
            
            double radiusX = ellipseDescriptor.GetSemiMajorAxis();
            double radiusY = ellipseDescriptor.GetSemiMinorAxis();

            CanonicalEllipseDescriptor canonicalEllipseDescriptor = 
                new CanonicalEllipseDescriptor( radiusX, radiusY );
            Point2D[] correctedToEllipsePoints =
                this.CorrectPointsToCanonicalEllipse( points2D, canonicalEllipseDescriptor );
            this.EllipsePoints = correctedToEllipsePoints;

            CircleDescriptor circleDescriptor = new CircleDescriptor( new Point2D( 0, 0 ), radiusX );
            Point2D[] correctedToCirclePoints =
                this.StretchToCircleAlongAxisY( correctedToEllipsePoints, circleDescriptor, 0 );

            return correctedToCirclePoints;
        }
        //---------------------------------------------------------------------------------------------
        //Определение ориентации точек
        private EllipseOrientaion GetPointsOrientation( Point2D[] points2D ) {
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
