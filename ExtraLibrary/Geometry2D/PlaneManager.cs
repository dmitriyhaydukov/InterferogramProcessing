using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using ExtraLibrary.Mathematics.Equations;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Vectors;
using ExtraLibrary.Mathematics.Numbers;

namespace ExtraLibrary.Geometry2D {
    public class PlaneManager {
        //-----------------------------------------------------------------------------------------------
        //Расстояние между двумя точками
        public static double DistanceBetweenTwoPoints( Point2D pointOne, Point2D pointTwo ) {
            double subX = pointOne.X - pointTwo.X;
            double subY = pointOne.Y - pointTwo.Y;
            double distance = Math.Sqrt( subX * subX + subY * subY );
            return distance;
        }
        //-----------------------------------------------------------------------------------------------
        //Расстояния между точками массива и заданной точкой
        public static double[] GetDistances( Point2D[] points, Point2D targetPoint ) {
            double[] distances = new double[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point2D point = points[ index ];
                double distance = PlaneManager.DistanceBetweenTwoPoints( point, targetPoint );
                distances[ index ] = distance;
            }
            return distances;
        }
        //-----------------------------------------------------------------------------------------------
        //Расстояние от точки до прямой
        public static double DistanceFromPointToLine( Point2D point, LineDescriptor lineDescriptor ) {
            double a = lineDescriptor.CoefficientOfX;
            double b = lineDescriptor.CoefficientOfY;
            double c = lineDescriptor.AbsoluteTerm;
            double distance = ( a * point.X + b * point.Y + c ) / Math.Sqrt( a * a + b * b );
            return distance;
        }
        //-----------------------------------------------------------------------------------------------
        //Точка пересечения двух прямых линий
        public static Point2D IntersectionPointOfLines( LineDescriptor lineOne, LineDescriptor lineTwo ) {
            double a1 = lineOne.CoefficientOfX;
            double b1 = lineOne.CoefficientOfY;
            double c1 = lineOne.AbsoluteTerm;

            double a2 = lineTwo.CoefficientOfX;
            double b2 = lineTwo.CoefficientOfY;
            double c2 = lineTwo.AbsoluteTerm;
            
            double y = ( ( a2 * c1 / a1 ) - c2 ) / ( b2 - ( a2 * b1 / a1 ) );
            double x = ( -c1 - b1 * y ) / a1;

            Point2D intersectionPoint = new Point2D( x, y );
            return intersectionPoint;
        }
        //-----------------------------------------------------------------------------------------------
        //Точки пересечения прямой и окружности
        public static Point2D[] IntersectionPointsOfLineAndCircle(
            LineDescriptor lineDescriptor,
            CircleDescriptor circleDescriptor
        ) {
            double a = lineDescriptor.CoefficientOfX;
            double b = lineDescriptor.CoefficientOfY;
            double c = lineDescriptor.AbsoluteTerm;

            double xc = circleDescriptor.Centre.X;
            double yc = circleDescriptor.Centre.Y;
            double r = circleDescriptor.Radius;

            double x1, x2, y1, y2;
            
            if ( b != 0 ) {
                double koefficientA = 1 + ( ( a * a ) / ( b * b ) );
                double koefficientB = 2 * ( c * a / ( b * b ) + yc * a / b - xc );
                double koefficientC =
                    xc * xc + yc * yc + ( c * c ) / ( b * b ) + 2 * yc * c / b - r * r;
                QuadraticEquation quadraticEquation =
                    new QuadraticEquation( koefficientA, koefficientB, koefficientC );

                Complex[] complexRoots = quadraticEquation.GetRoots();
                double[] realRoots = NumbersManager.GetRealParts( complexRoots );
                
                x1 = realRoots[ 0 ];
                y1 = ( -c - a * x1 ) / b;
                x2 = realRoots[ 1 ];
                y2 = ( -c - a * x2 ) / b;
            }
            else {
                double x = -c / a;
                x1 = x2 = x;
                double[] coordinatesY = circleDescriptor.GetCoordinatesY( x );
                y1 = coordinatesY[ 0 ];
                y2 = coordinatesY[ 1 ];
            }

            Point2D intersectionPoint1 = new Point2D( x1, y1 );
            Point2D intersectionPoint2 = new Point2D( x2, y2 );
            Point2D[] intersectionPoints = new Point2D[] { intersectionPoint1, intersectionPoint2 };
            
            return intersectionPoints;
        }
        //-----------------------------------------------------------------------------------------------
        //Перемещение точек
        public static Point2D[] DisplacePoints(
            Point2D[] points,
            double displacementX,
            double displacementY
        ) {
            Point2D[] newPoints = new Point2D[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point2D point = points[ index ];
                double newX = point.X + displacementX;
                double newY = point.Y + displacementY;
                Point2D newPoint = new Point2D( newX, newY );
                newPoints[ index ] = newPoint;
            }
            return newPoints;
        }
        //-----------------------------------------------------------------------------------------------
        //Минимальные координаты точек
        public static double[] GetMinimalCoordinates( Point2D[] points ) {
            double[] coordinatesX = PlaneManager.GetCoordinatesX( points );
            double[] coordinatesY = PlaneManager.GetCoordinatesY( points );
            
            double minX = coordinatesX.Min();
            double minY = coordinatesY.Min();
            
            double[] minimalCoordinates = new double[] { minX, minY };
            return minimalCoordinates;
        }
        //-----------------------------------------------------------------------------------------------
        //Координаты X точек
        public static double[] GetCoordinatesX( Point2D[] points ) {
            double[] coordinatesX = new double[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point2D point = points[ index ];
                coordinatesX[ index ] = point.X;
            }
            return coordinatesX;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //Координаты Y точек
        public static double[] GetCoordinatesY( Point2D[] points ) {
            double[] coordinatesY = new double[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point2D point = points[ index ];
                coordinatesY[ index ] = point.Y;
            }
            return coordinatesY;
        }
        //-----------------------------------------------------------------------------------------------
        //Перемещение точек в первый квадрант
        public static Point2D[] DisplacePointsToFirstQuadrant(Point2D[] points) {
            double[] minimalCoordinates = PlaneManager.GetMinimalCoordinates( points );
            double displacementX = minimalCoordinates[ 0 ];
            double displacementY = minimalCoordinates[ 1 ];
            
            Point2D[] newPoints = PlaneManager.DisplacePoints( points, -displacementX, -displacementY );
            return newPoints;
        }
        //-----------------------------------------------------------------------------------------------
        //Матрица поворота против часовой стрелки (угол в радианах)
        public static RealMatrix GetRotationMatrixAnticlockwise(double angle) {
            double[ , ] arrayMatrix = new double[ , ] {
                {   Math.Cos(angle),    -Math.Sin(angle)    },
                {   Math.Sin(angle),    Math.Cos(angle)     }
            };
            RealMatrix rotationMatrix = new RealMatrix( arrayMatrix );
            return rotationMatrix;
        }
        //-----------------------------------------------------------------------------------------------
        //Повернуть вектор
        public static RealVector RotateVector( RealVector vector, RealMatrix rotationMatrix ) {
            RealVector resultVector = rotationMatrix * vector;
            return resultVector;
        }
        //----------------------------------------------------------------------------------------------
        //Повернуть векторы
        public static RealVector[] RotateVectros(
            RealVector[] vectors,
            RealMatrix rotationMatrix
        ) {
            RealVector[] newVectors = new RealVector[ vectors.Length ];
            for ( int index = 0; index < vectors.Length; index++ ) {
                RealVector vector = vectors[ index ];
                RealVector newVector = PlaneManager.RotateVector( vector, rotationMatrix );
                newVectors[ index ] = newVector;
            }
            return newVectors;
        }
        //-----------------------------------------------------------------------------------------------
        //Векторы координат точек
        public static RealVector[] CreateVectorsFromPoints( Point2D[] points ) {
            RealVector[] coordinatesVectors = new RealVector[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point2D point = points[ index ];
                RealVector coordinatesVector = point.GetCoordinatesVector();
                coordinatesVectors[ index ] = coordinatesVector;
            }
            return coordinatesVectors;
        }
        //-----------------------------------------------------------------------------------------------
        //Создание точек
        public static Point2D[] CreatePoints2D( double[] coordinatesX, double[] coordinatesY ) {
            Point2D[] points = new Point2D[ coordinatesX.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                double x = coordinatesX[ index ];
                double y = coordinatesY[ index ];
                Point2D point = new Point2D( x, y );
                points[ index ] = point;
            }
            return points;
        }
        //-----------------------------------------------------------------------------------------------
        //Создание точек из векторов координат
        public static Point2D[] CreatePoints2DFromVectors( RealVector[] vectors ) {
            Point2D[] points = new Point2D[ vectors.Length ];
            for ( int index = 0; index < vectors.Length; index++ ) {
                RealVector coordinatesVector = vectors[ index ];
                Point2D point = new Point2D( coordinatesVector );
                points[ index ] = point;
            }
            return points;
        }
        //-----------------------------------------------------------------------------------------------
        //Выбрать ближайшую к данной точке точку из массива
        public static Point2D GetNearestPoint( Point2D point, Point2D[] points ) {
            if ( points.Length == 1 ) {
                return points[ 0 ];
            }
            Point2D nearestPoint = points[0];
            double minDistance = PlaneManager.DistanceBetweenTwoPoints( point, nearestPoint );
            for ( int index = 1; index < points.Length; index++ ) {
                Point2D currentPoint = points[ index ];
                double distance = PlaneManager.DistanceBetweenTwoPoints( point, currentPoint );
                if ( distance < minDistance ) {
                    minDistance = distance;
                    nearestPoint = currentPoint;
                }
            }
            return nearestPoint;
        }
        //-----------------------------------------------------------------------------------------------
        //Прямя по двум точкам
        public static LineDescriptor GetLineDescriptor( Point2D pointOne, Point2D pointTwo ) {
            double coefficientOfX = pointTwo.Y;
            double coefficientOfY = pointOne.X - pointTwo.X;
            double absoluteTerm = pointOne.X * pointTwo.Y + pointOne.Y * pointTwo.X;

            LineDescriptor lineDescriptor = 
                new LineDescriptor( coefficientOfX, coefficientOfY, absoluteTerm );
            return lineDescriptor;
        }
        //-----------------------------------------------------------------------------------------------
        //Средняя точка на плоскости
        public static Point2D GetMidPoint( Point2D[] points ) {
            Point2D accumulatedPoint = new Point2D( 0, 0 );
            for ( int index = 0; index < points.Length; index++ ) {
                Point2D point = points[ index ];
                accumulatedPoint += point;
            }
            int n = points.Length;
            double resultX = accumulatedPoint.X / n;
            double resultY = accumulatedPoint.Y / n;
            
            Point2D resultPoint = new Point2D( resultX, resultY );
            return resultPoint;
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
    }
}
